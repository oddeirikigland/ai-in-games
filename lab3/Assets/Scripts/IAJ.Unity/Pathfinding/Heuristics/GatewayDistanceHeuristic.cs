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

    public float H(NavigationGraphNode node, NavigationGraphNode goalNode)
    {
        this.NodeCluster = this.ClusterGraph.Quantize(node);
        this.GoalNodeCluster = this.ClusterGraph.Quantize(goalNode);
        this.DistanceNodeToGoalNode = float.PositiveInfinity;
        float distance;

        Vector3 nodeClusterPos = this.NodeCluster.Localize();
        Vector3 goalNodeClusterPos = this.GoalNodeCluster.Localize();

        if (nodeClusterPos != null && nodeClusterPos == goalNodeClusterPos)
        {
            return this.EuclideanDistanceHeuristic.H(node, goalNode);
        }

        foreach (Gateway nodeGateway in this.NodeCluster.gateways)
        {
            distance = this.EuclideanDistanceHeuristic.H(node, nodeGateway.Localize());
            foreach (Gateway goalNodeGateway in this.GoalNodeCluster.gateways)
            {
                distance += this.ClusterGraph.gatewayDistanceTable[nodeGateway.id].entries[goalNodeGateway.id].shortestDistance;
                distance += this.EuclideanDistanceHeuristic.H(goalNode, goalNodeGateway.Localize());
                this.DistanceNodeToGoalNode = Mathf.Min(this.DistanceNodeToGoalNode, distance);
            }
        }        
        return this.DistanceNodeToGoalNode;
    }
}
