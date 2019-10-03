using System;
using UnityEngine;

namespace Assets.Scripts.IAJ.Unity.Movement.DynamicMovement
{
    public class DynamicArrive : DynamicVelocityMatch
    {
        public float StopRadius { get; set; }
        public float SlowRadius { get; set; }
        public float MaxSpeed { get; set; }
        public KinematicData DestinationTarget { get; set; }

        public override string Name
        {
            get { return "Arrive"; }
        }

        public DynamicArrive()
        {
            this.StopRadius = 2f;
            this.SlowRadius = this.StopRadius * 4f;
            this.Target = new KinematicData();
        }

        public override MovementOutput GetMovement()
        {
            Vector3 direction = this.DestinationTarget.Position - this.Character.Position;
            float distance = direction.magnitude;
            float desiredSpeed;

            if (distance < this.StopRadius)
            {
                desiredSpeed = 0;
            }
            else if (distance > this.SlowRadius)
            {
                desiredSpeed = this.MaxSpeed;
            }
            else
            {
                desiredSpeed = this.MaxSpeed * (distance / this.SlowRadius);
            }
            base.Target.velocity = direction.normalized * desiredSpeed;
            return base.GetMovement();
        }
    }
}
