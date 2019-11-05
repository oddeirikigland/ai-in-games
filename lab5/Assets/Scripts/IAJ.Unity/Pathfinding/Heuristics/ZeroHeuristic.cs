using RAIN.Navigation.Graph;
using UnityEngine;

namespace Assets.Scripts.IAJ.Unity.Pathfinding.Heuristics
{
    public class ZeroHeuristic : IHeuristic
    {
        public float H(Vector3 nodePosition, Vector3 goalNodePosition)
        {
            return 0;
        }
    }
}
