using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.IAJ.Unity.Pathfinding.DataStructures
{
    public class ClosedListHashmap : IClosedSet
    {
        private Dictionary<Vector3, NodeRecord> NodeRecords { get; set; }

        public ClosedListHashmap()
        {
            this.NodeRecords = new Dictionary<Vector3, NodeRecord>();
        }

        public void AddToClosed(NodeRecord nodeRecord)
        {
            this.NodeRecords.Add(nodeRecord.node.Position, nodeRecord);
        }

        public ICollection<NodeRecord> All()
        {
            return this.NodeRecords.Values;
        }

        public void Initialize()
        {
            this.NodeRecords.Clear();
        }

        public void RemoveFromClosed(NodeRecord nodeRecord)
        {
            this.NodeRecords.Remove(nodeRecord.node.Position);
        }

        public NodeRecord SearchInClosed(NodeRecord nodeRecord)
        {
            this.NodeRecords.TryGetValue(nodeRecord.node.Position, out NodeRecord record);
            return record;
        }
    }
}
