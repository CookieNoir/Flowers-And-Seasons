using System;
using UnityEngine;

public class MovementDirectionAction : MonoBehaviour
{
    [SerializeField] private MovementDirections _direction;
    private Action<MovementDirections> _action;

    public void SetAction(Action<MovementDirections> action)
    {
        _action = action;
    }

    public void DoAction()
    {
        if (_action != null)
        {
            _action(_direction);
        }
    }
}