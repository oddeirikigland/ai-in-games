using Assets.Scripts.IAJ.Unity.Movement;
using Assets.Scripts.IAJ.Unity.Movement.DynamicMovement;
using Assets.Scripts.IAJ.Unity.Pathfinding.Path;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicFollowPath : DynamicSeek
{
	private Path path;
	public Path Path
	{
		get { return this.path; }
		set
		{
			this.path = value;
			if (this.path != null)
			{
				this.currentParam = 0;
				//this.Character.rotation = 0;
				//this.Character.velocity = new Vector3();
			}
		}
	}

	public float pathOffset = 0.8f;
	private float currentParam = 0;

	public override string Name
	{
		get { return "FollowPath"; }
	}

	public DynamicFollowPath(): base()
	{
		path = null;
		this.Target = new KinematicData();
	}

	public override MovementOutput GetMovement()
	{
		if (path == null) return new MovementOutput();
		currentParam = path.GetParam(Character.Position, currentParam);
		if (path.PathEnd(currentParam)) return new MovementOutput()
		{
			linear = - Character.velocity,
			angular = - Character.rotation,
		};
		var targetParam = currentParam + pathOffset;
		base.Target.Position = path.GetPosition(targetParam);
		return base.GetMovement();
	}
}
