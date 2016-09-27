


using UnityEngine;

/// <summary>
/// 
/// </summary>
public interface IObjectTouchListener
{
	void OnTapStart(GameObject go, Vector2 position);
	void OnTapStop(GameObject go, Vector2 position);
	void OnTapStay(GameObject go, Vector2 position);
	void OnDoubleTap(GameObject go, Vector2 position);
	void OnSwipe(GameObject go, Vector2 swipeVector, float speedRatio);
}
