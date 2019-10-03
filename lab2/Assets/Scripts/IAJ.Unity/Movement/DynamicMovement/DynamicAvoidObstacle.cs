using System;
using UnityEngine;

namespace Assets.Scripts.IAJ.Unity.Movement.DynamicMovement
{
    public class DynamicAvoidObstacle : DynamicSeek
    {
        public float MaxLookAhead { get; set; }
        public float AvoidMargin { get; set; }
        public Collider Obstacle { get; set; }


        public override string Name
        {
            get { return "Avoid Obstacle"; }
        }

        public DynamicAvoidObstacle(GameObject obstacle)
        {
            this.Obstacle = obstacle.GetComponent<Collider>();
            this.Target = new KinematicData();
        }

        public override MovementOutput GetMovement()
        {
            bool collision = false;
            RaycastHit hit;
            var normedVelocity = this.Character.velocity;
            normedVelocity.Normalize();
            if (normedVelocity.sqrMagnitude > 0)
            {
                var auxPos = this.Character.Position + normedVelocity * 2.0f;
                Ray RayVector = new Ray(auxPos, normedVelocity);
                collision = this.Obstacle.Raycast(RayVector, out hit, this.MaxLookAhead);
                if (!collision) return new MovementOutput();
                base.Target.Position = hit.point + hit.normal * this.AvoidMargin;
                return base.GetMovement();
            }
            else return new MovementOutput();
        }
    }
}
