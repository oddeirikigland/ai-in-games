using RAIN.Navigation.Graph;
using UnityEngine;

namespace Assets.Scripts.IAJ.Unity.Pathfinding.Heuristics
{
    public class EuclideanDistanceHeuristic : IHeuristic
    {
        public float H(NavigationGraphNode node, NavigationGraphNode goalNode)
        {
            Vector3 delta = goalNode.Position - node.Position;
            return Mathf.Sqrt(delta.x * delta.x + delta.z * delta.z);
        }

        public float H(NavigationGraphNode node, Vector3 gatewayPosition)
        {
            Vector3 delta = gatewayPosition - node.Position;
            return Mathf.Sqrt(delta.x * delta.x + delta.z * delta.z);
        }
    }
}
