using System;
using UnityEngine;
[RequireComponent(typeof(Collider))]
public class MouseDownAction : MonoBehaviour
{
    private Action _action;

    public void SetAction(Action action)
    {
        _action = action;
    }

    private void OnMouseDown()
    {
        if (_action != null) _action();
    }
}