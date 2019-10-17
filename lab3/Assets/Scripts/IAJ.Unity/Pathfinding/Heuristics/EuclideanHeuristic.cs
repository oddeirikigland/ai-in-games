using RAIN.Navigation.Graph;
using UnityEngine;

namespace Assets.Scripts.IAJ.Unity.Pathfinding.Heuristics
{
    public class EuclideanHeuristic : IHeuristic
    {
        public float H(NavigationGraphNode node, NavigationGraphNode goalNode)
        {
            Vector3 delta = goalNode.Position - node.Position;
            return Mathf.Sqrt(delta.x * delta.x + delta.z * delta.z);
        }
    }
}
