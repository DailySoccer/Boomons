using UnityEngine;

public class ReferenceSystem
{
	private readonly Vector3 _planePoint;
	public readonly Vector3 Right;
	public readonly Vector3 Up;
	public readonly Vector3 ScreenDir;

	public ReferenceSystem(Vector3 point, Vector3 right)
	{
		_planePoint = point;

		right.Normalize();
		Up = -Physics.gravity.normalized;
		ScreenDir = Vector3.Cross(Up, right);
		Right = Vector3.Cross(ScreenDir, Up);
	}

	public Vector3 ProjectOnPlane(Vector3 v, bool isPoint = true)
	{ 
		return isPoint ? 
			v - Vector3.Project(v - _planePoint, ScreenDir) :
			Vector3.ProjectOnPlane(v, ScreenDir);
	}
}