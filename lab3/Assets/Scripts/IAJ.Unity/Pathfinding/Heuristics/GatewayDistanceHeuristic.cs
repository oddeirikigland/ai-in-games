using Assets.Scripts.IAJ.Unity.Pathfinding.DataStructures.HPStructures;
using Assets.Scripts.IAJ.Unity.Pathfinding.Heuristics;
using RAIN.Navigation.Graph;
using UnityEngine;

public class GatewayDistanceHeuristic : IHeuristic
{
    public ClusterGraph ClusterGraph { get; set; }
    public EuclideanDistanceHeuristic EuclideanDistanceHeuristic { get; set; }
    public Cluster NodeCluster { get; set; }
    public Cluster GoalNodeCluster { get; set; }
    public float DistanceNodeToGoalNode { get; set; }

    public GatewayDistanceHeuristic()
    {
        this.ClusterGraph = Resources.Load<ClusterGraph>("ClusterGraph");
        this.EuclideanDistanceHeuristic = new EuclideanDistanceHeuristic(); 
    }

    public float H(Vector3 nodePosition, Vector3 goalNodePosition)
    {
        this.NodeCluster = this.ClusterGraph.Quantize(nodePosition);
        if (this.NodeCluster == null) return float.PositiveInfinity;

        this.GoalNodeCluster = this.ClusterGraph.Quantize(goalNodePosition);
        this.DistanceNodeToGoalNode = float.PositiveInfinity;
        float distance;

        if (this.NodeCluster.Localize() == this.GoalNodeCluster.Localize())
        {
            return this.EuclideanDistanceHeuristic.H(nodePosition, goalNodePosition);
        }

        foreach (Gateway nodeGateway in this.NodeCluster.gateways)
        {
            distance = this.EuclideanDistanceHeuristic.H(nodePosition, nodeGateway.Localize());
            foreach (Gateway goalNodeGateway in this.GoalNodeCluster.gateways)
            {
                distance += this.ClusterGraph.gatewayDistanceTable[nodeGateway.id].entries[goalNodeGateway.id].shortestDistance;
                distance += this.EuclideanDistanceHeuristic.H(goalNodePosition, goalNodeGateway.Localize());
                this.DistanceNodeToGoalNode = Mathf.Min(this.DistanceNodeToGoalNode, distance);
            }
        }        
        return this.DistanceNodeToGoalNode;
    }
}
