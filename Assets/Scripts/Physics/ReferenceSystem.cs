﻿using UnityEngine;

public class ReferenceSystem
{
	private readonly Vector3 _planePoint;
	public readonly Vector3 Right;
	public readonly Vector3 JumpDir;
	public readonly Vector3 ScreenDir;

	public ReferenceSystem(Vector3 point, Vector3 right)
	{
		_planePoint = point;

		right.Normalize();
		JumpDir = -Physics.gravity.normalized;
		ScreenDir = Vector3.Cross(JumpDir, right);
		Right = Vector3.Cross(ScreenDir, JumpDir);
	}

	public Vector3 ProjectOnPlane(Vector3 position)
	{
		return position - Vector3.Project(position - _planePoint, ScreenDir);
	}
}