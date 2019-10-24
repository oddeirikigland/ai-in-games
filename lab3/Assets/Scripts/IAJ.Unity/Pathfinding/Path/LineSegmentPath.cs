using Assets.Scripts.IAJ.Unity.Utils;
using UnityEngine;
using System;

namespace Assets.Scripts.IAJ.Unity.Pathfinding.Path
{
    public class LineSegmentPath : LocalPath
    {
        protected Vector3 LineVector;
		public float EndThreshold = 0.3f;

		public LineSegmentPath(Vector3 start, Vector3 end)
        {
            this.StartPosition = start;
            this.EndPosition = end;
            this.LineVector = end - start;
        }

        public override Vector3 GetPosition(float param)
        {
			//TODO: done
			return StartPosition + LineVector.normalized * LineVector.magnitude * (param % 1.0f);
        }

        public override bool PathEnd(float param)
        {
			//TODO: done
			return (param % 1.0f) >= 1.0f - EndThreshold;
        }

        public override float GetParam(Vector3 position, float lastParam)
        {
			//TODO: done
			return MathHelper.closestParamInLineSegmentToPoint(StartPosition, EndPosition, position);
        }
    }
}
