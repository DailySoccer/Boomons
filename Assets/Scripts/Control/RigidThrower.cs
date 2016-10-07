﻿using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RigidThrower : Touchable
{
	#region Public Methods

	public override void OnSwipe(GameObject go, Vector2 swipeVector, float speedRatio)
	{
		Throw(CalcThrowVelocity(swipeVector, speedRatio));
	}


	/// <summary>
	/// 
	/// </summary>
	/// <param name="velocity"></param>
	/// <param name="setupRef"></param>
	public void Throw(Vector3 velocity)
	{
		Debug.Log("RigidThrower::Throw>> " + name + " throwed @ " + velocity, this);
		_rigid.AddForce(velocity, ForceMode.VelocityChange);

		// TODO FRS 161007 Sería interesante aplicar la fuerza sobre el punto de contacto
		// y añadir de ese modo un torque
	}

	#endregion

	//==============================================================

	#region Mono

	protected override void Awake()
	{
		base.Awake();

		if (_rigid == null)
			_rigid = GetComponentInChildren<Rigidbody>();
		
	}

	protected override void OnDestroy()
	{
		
		_rigid = null;
		base.OnDestroy();
	}

	#endregion

	//==============================================================

	#region Private Methods

	/// <summary>
	/// 
	/// </summary>
	/// <param name="swipeVector"></param>
	/// <param name="swipeSpeedRatio"></param>
	/// <returns></returns>
	private Vector3 CalcThrowVelocity(Vector2 swipeVector, float swipeSpeedRatio)
	{
		Camera camera = Camera.main;
		Vector3 dir = swipeVector.x * camera.transform.right 
			        + swipeVector.y * camera.transform.up;

/*
		return swipeSpeedRatio * _throwSpeedMax * dir.normalized;
/*/
		return _throwSpeedMax * dir.normalized;
/**/
	}
	#endregion


	//==============================================================

	#region Private Fields

	protected Rigidbody Rigid
	{
		get { return _rigid; }
	}
	
	

	private Rigidbody _rigid;
	[SerializeField, Range(0.5f, 50f)] private float _throwSpeedMax = 30f;
	


	#endregion





}
