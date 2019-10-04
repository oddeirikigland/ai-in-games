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
                int vectorLenght = 6;
                var auxPos = this.Character.Position + normedVelocity * 2.0f;
                var leftWhisker = normedVelocity - new Vector3(0.5f,0,0);
                var rightWhisker = normedVelocity - new Vector3(-0.5f,0,0);

                Ray RayVector = new Ray(auxPos, normedVelocity * vectorLenght);
                Ray LeftRayVector = new Ray(auxPos, leftWhisker * vectorLenght / 2);
                Ray RightRayVector = new Ray(auxPos, rightWhisker * vectorLenght / 2);

                Debug.DrawRay(auxPos, normedVelocity * vectorLenght, Color.black);
                Debug.DrawRay(auxPos, leftWhisker * vectorLenght / 2, Color.black);
                Debug.DrawRay(auxPos, rightWhisker * vectorLenght / 2, Color.black);
                
                collision = this.Obstacle.Raycast(RayVector, out hit, this.MaxLookAhead);
                collision = this.Obstacle.Raycast(LeftRayVector, out hit, this.MaxLookAhead / 2);
                collision = this.Obstacle.Raycast(RightRayVector, out hit, this.MaxLookAhead / 2);
                if (!collision) return new MovementOutput();
                base.Target.Position = hit.point + hit.normal * this.AvoidMargin;
                return base.GetMovement();
            }
            else return new MovementOutput();
        }
    }
}
