using System;
using UnityEngine;

namespace Assets.Scripts.IAJ.Unity.Movement.DynamicMovement
{
    public class DynamicAvoidCharacter : DynamicSeek
    {
        public float AvoidMargin { get; set; }
        public float MaxLookAhead { get; set; }
        public KinematicData OtherCharacter { get; set; }


        public override string Name
        {
            get { return "Avoid Character"; }
        }

        public DynamicAvoidCharacter(KinematicData otherCharacter)
        {
            this.Output = new MovementOutput();
            this.OtherCharacter = otherCharacter;
        }

        public override MovementOutput GetMovement()
        {
            Vector3 deltaPos = this.OtherCharacter.Position - this.Character.Position;
            Vector3 deltaVel = this.OtherCharacter.velocity - this.Character.velocity;
            float deltaSqrSpeed = deltaVel.sqrMagnitude;
            if (deltaSqrSpeed == 0) return this.Output;

            float timeToClosest = -Vector3.Dot(deltaPos, deltaVel) / deltaSqrSpeed;
            if (timeToClosest > MaxLookAhead) return this.Output;

            Vector3 futureDeltaPos = deltaPos + deltaVel * timeToClosest;
            float futureDistance = futureDeltaPos.magnitude;
            if (futureDistance > 2 * this.AvoidMargin) return this.Output;

            if (futureDistance <= 0 || deltaPos.magnitude < 2 * this.AvoidMargin)
            {
                this.Output.linear = this.Character.Position - this.OtherCharacter.Position;
            }
            else this.Output.linear = futureDeltaPos * -1;
            this.Output.linear = this.Output.linear.normalized * this.MaxAcceleration;
            return this.Output;
        }
    }
}
