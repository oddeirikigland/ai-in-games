//class adapted from the HRVO library http://gamma.cs.unc.edu/HRVO/
//adapted to IAJ classes by João Dias

using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.IAJ.Unity.Movement.DynamicMovement;
using Assets.Scripts.IAJ.Unity.Util;
using UnityEngine;

namespace Assets.Scripts.IAJ.Unity.Movement.VO
{
    public class RVOMovement : DynamicMovement.DynamicVelocityMatch
    {
        public override string Name
        {
            get { return "RVO"; }
        }

        protected List<KinematicData> Characters { get; set; }
        protected List<StaticData> Obstacles { get; set; }
        public float CharacterSize { get; set; }
        public float IgnoreDistance { get; set; }
        public float MaxSpeed { get; set; }
        public int NumSamples { get; set; }
        public float ImportanceAvoidCollision { get; set; }

        protected DynamicMovement.DynamicMovement DesiredMovement { get; set; }

        public RVOMovement(DynamicMovement.DynamicMovement goalMovement, List<KinematicData> movingCharacters, List<StaticData> obstacles)
        {
            this.DesiredMovement = goalMovement;
            this.Characters = movingCharacters;
            this.Obstacles = obstacles;
            base.Target = new KinematicData();
            this.NumSamples = 100;
            this.CharacterSize = 2f;
            this.ImportanceAvoidCollision = 8f;
            this.IgnoreDistance = 5f;
        }
        
        protected Vector3 GetBestSample(Vector3 desiredVelocity, List<Vector3> samples)
        {
            Vector3 bestSample = Vector3.zero;
            double minimumPenalty = double.PositiveInfinity;
            double timePenalty;
            int threshold = 4;
            foreach (Vector3 sample in samples)
            {
                int counter = 0;
                float distancePenalty = (desiredVelocity - sample).magnitude;
                double maximumPenalty = 0;
                foreach (StaticData b in this.Obstacles)
                {
                    Vector3 deltaP = b.Position - this.Character.Position;
                    if (deltaP.magnitude > this.IgnoreDistance * 2) continue;
                    Vector3 rayVector = sample - this.Character.velocity;
                    float tc = MathHelper.TimeToCollisionBetweenRayAndCircle(this.Character.Position, rayVector, b.Position, this.CharacterSize);
                    if (tc > 0)
                    {
                        timePenalty = this.ImportanceAvoidCollision * 10 / tc;
                    }
                    else if (tc == 0)
                    {
                        timePenalty = double.PositiveInfinity;
                    }
                    else
                    {
                        timePenalty = 0;
                    }

                    if (timePenalty > maximumPenalty)
                    {
                        maximumPenalty = timePenalty;
                        if (counter > threshold) break;
                        counter++;
                    }
                }
                foreach (KinematicData b in this.Characters)
                {
                    if (this.Character.Position.Equals(b.Position)) continue;
                    Vector3 deltaP = b.Position - this.Character.Position;
                    if (deltaP.magnitude > this.IgnoreDistance) continue;
                    Vector3 rayVector = 2 * sample - this.Character.velocity - b.velocity;
                    float tc = MathHelper.TimeToCollisionBetweenRayAndCircle(this.Character.Position, rayVector, b.Position, this.CharacterSize);
                    if (tc > 0)
                    {
                        timePenalty = this.ImportanceAvoidCollision / tc;
                    }
                    else if (tc == 0)
                    {
                        timePenalty = double.PositiveInfinity;
                    }
                    else
                    {
                        timePenalty = 0;
                    }

                    if (timePenalty > maximumPenalty)
                    {
                        maximumPenalty = timePenalty;
                        if (counter > threshold) break;
                        counter++;
                    }
                }
                double penalty = distancePenalty + maximumPenalty;
                if (penalty < minimumPenalty)
                {
                    minimumPenalty = penalty;
                    bestSample = sample;
                }
            }
            return bestSample;
        }

        public override MovementOutput GetMovement()
        {
            List<Vector3> samples = new List<Vector3>();
            MovementOutput desiredOutput = this.DesiredMovement.GetMovement();
            Vector3 desiredVelocity = this.Character.velocity + desiredOutput.linear;
            if (desiredVelocity.magnitude > this.MaxSpeed)
            {
                desiredVelocity.Normalize();
                desiredVelocity *= this.MaxSpeed;
            }
            samples.Add(desiredVelocity);
            for (int i = 0; i < this.NumSamples; i++)
            {
                float angle = Random.Range(0, MathConstants.MATH_2PI);
                float magnitude = Random.Range(0, this.MaxSpeed);
                Vector3 velocitySample = MathHelper.ConvertOrientationToVector(angle) * magnitude;
                samples.Add(velocitySample);
            }
            base.Target.velocity = this.GetBestSample(desiredVelocity, samples);
            return base.GetMovement();
        }
    }
}
