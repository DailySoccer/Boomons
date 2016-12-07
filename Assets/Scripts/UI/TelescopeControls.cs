using UnityEngine;
using System.Collections;

public class TelescopeControls : MonoBehaviour {

    void Start()
    {
        _vertical = _horizontal = 0;
    }
    public void ClickLeft()
    {
        _horizontal = -1;
    }

    public void ClickRight()
    {
        _horizontal = 1;
    }

    public void ClickUp()
    {
        _vertical = 1;
    }

    public void ClickDown()
    {
        _vertical = -1;
    }

    public void ClearHorizontal()
    {
        _horizontal = 0;
    }

    public void ClearVertical()
    {
        _vertical = 0;
    }

    public float GetHorizontal()
    {
        return _horizontal;
    }

    public float GetVertical()
    {
        return _vertical;
    }

    private float _horizontal;
    private float _vertical;
}
