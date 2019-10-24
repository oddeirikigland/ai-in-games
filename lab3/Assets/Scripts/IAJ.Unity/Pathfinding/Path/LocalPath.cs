using UnityEngine;

namespace Assets.Scripts.IAJ.Unity.Pathfinding.Path
{
    public abstract class LocalPath : Path
    {
		// TODO: change back to protected
        public Vector3 StartPosition { get; set; }
        public Vector3 EndPosition { get; set; }
    }
}
