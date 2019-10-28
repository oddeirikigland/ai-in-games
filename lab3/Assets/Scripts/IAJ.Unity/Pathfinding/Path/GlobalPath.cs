using System.Collections.Generic;
using Assets.Scripts.IAJ.Unity.Utils;
using RAIN.Navigation.Graph;
using UnityEngine;
using System;

namespace Assets.Scripts.IAJ.Unity.Pathfinding.Path
{
    public class GlobalPath : Path
    {
        public List<NavigationGraphNode> PathNodes { get; protected set; }
        public List<Vector3> PathPositions { get; protected set; } 
        public bool IsPartial { get; set; }
        public float Length { get; set; }
        public List<LocalPath> LocalPaths { get; protected set; } 

        public GlobalPath()
        {
            this.PathNodes = new List<NavigationGraphNode>();
            this.PathPositions = new List<Vector3>();
            this.LocalPaths = new List<LocalPath>();
        }

        public override float GetParam(Vector3 position, float previousParam)
        {
			//TODO: done
			int previousInt = Mathf.FloorToInt(previousParam);
			float newParam = previousInt + LocalPaths[Math.Min(previousInt, LocalPaths.Count - 1)].GetParam(position, previousParam);
			return Math.Min(Math.Max(newParam, previousParam), LocalPaths.Count);
        }

        public override Vector3 GetPosition(float param)
        {
			//TODO: done
			int paramInt = Mathf.FloorToInt(param);
			if (paramInt > LocalPaths.Count - 1) return LocalPaths[LocalPaths.Count - 1].GetPosition(0.99f);
			return LocalPaths[paramInt].GetPosition(param);
        }

        public override bool PathEnd(float param)
        {
			//TODO: done
			return (Mathf.FloorToInt(param) > LocalPaths.Count - 1) || (Mathf.FloorToInt(param) == LocalPaths.Count - 1) && LocalPaths[LocalPaths.Count - 1].PathEnd(param);
        }
    }
}
