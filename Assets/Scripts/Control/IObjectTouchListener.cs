


using UnityEngine;

/// <summary>
/// 
/// </summary>
public interface IObjectTouchListener
{
	void OnTapStart(Toucher toucher, Vector2 position);
	void OnTapStop(Toucher toucher, Vector2 position);
	void OnTapStay(Toucher toucher, Vector2 position);
	void OnDoubleTap(Toucher toucher, Vector2 position);
	void OnSwipe(Toucher toucher, Vector2 position, Vector2 direction, float speedRatio);
}
