using RAIN.Navigation.Graph;
using UnityEngine;

namespace Assets.Scripts.IAJ.Unity.Pathfinding.Heuristics
{
    public class EuclideanDistanceHeuristic : IHeuristic
    {
        public float H(Vector3 nodePosition, Vector3 goalNodePosition)
        {
            Vector3 delta = goalNodePosition - nodePosition;
            return Mathf.Sqrt(delta.x * delta.x + delta.z * delta.z);
        }
    }
}
