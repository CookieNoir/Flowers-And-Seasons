using System;
using UnityEngine;
[RequireComponent(typeof(Collider))]
public class MovementButton : MonoBehaviour
{
    [SerializeField] private MovementDirectionAction _movementDirectionAction;
    [SerializeField] private VertexAlphaColorMesh _vertexAlphaColorMesh;
    [SerializeField] private Color _defaultColor;
    [SerializeField] private Color _highlightedColor;
    private bool _canInteract = true;

    private void Start()
    {
        _vertexAlphaColorMesh.CreateMeshWithColor(_defaultColor);
    }

    public void SetButtonAction(Action<MovementDirections> action)
    {
        _movementDirectionAction.SetAction(action);
    }

    private void OnMouseDown()
    {
        if (_canInteract)
        {
            _movementDirectionAction.DoAction();
            _vertexAlphaColorMesh.SetColor(_highlightedColor);
            _canInteract = false;
        }
    }

    public void SetInteraction(bool value)
    {
        _canInteract = value;
        if (_canInteract) _vertexAlphaColorMesh.SetColor(_defaultColor);
    }

    public void SetAlpha(float value)
    {
        _vertexAlphaColorMesh.SetAlpha(value);
    }
    
    private void OnMouseEnter()
    {
        if (_canInteract) _vertexAlphaColorMesh.SetColor(_highlightedColor);
    }

    private void OnMouseExit()
    {
        if (_canInteract) _vertexAlphaColorMesh.SetColor(_defaultColor);
    }
}