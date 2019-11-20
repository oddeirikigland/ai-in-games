using RAIN.Navigation.Graph;
using UnityEngine;

namespace Assets.Scripts.IAJ.Unity.Pathfinding.Heuristics
{
    public interface IHeuristic
    {
        float H(Vector3 nodePosition, Vector3 goalNodePosition);
    }
}
