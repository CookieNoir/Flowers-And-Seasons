using System;
using System.Collections;
using UnityEngine;

public class DestinationPoint : MonoBehaviour
{
    [SerializeField] private MovementButton _upButton;
    [SerializeField] private MovementButton _rightButton;
    [SerializeField] private MovementButton _downButton;
    [SerializeField] private MovementButton _leftButton;
    [SerializeField, Min(0.25f)] private float _fadingSpeed = 1f;
    private IEnumerator _buttonsFading;

    private void Awake()
    {
        _buttonsFading = _ButtonsFading(1f);
    }

    public void SetAction(Action<MovementDirections> action)
    {
        _upButton.SetButtonAction(action);
        _rightButton.SetButtonAction(action);
        _downButton.SetButtonAction(action);
        _leftButton.SetButtonAction(action);
    }

    public void SetPosition(Vector2Int destinationPosition)
    {
        transform.position = new Vector3(destinationPosition.x + 0.5f, 0f, destinationPosition.y + 0.5f);
    }

    public void HideButtons()
    {
        _upButton.SetInteraction(false);
        _rightButton.SetInteraction(false);
        _downButton.SetInteraction(false);
        _leftButton.SetInteraction(false);
        StopCoroutine(_buttonsFading);
        _buttonsFading = _ButtonsFading(-1f);
        StartCoroutine(_buttonsFading);
    }

    private IEnumerator _ButtonsFading(float multiplier)
    {
        float startAlpha = 0.5f - multiplier * 0.5f;
        _upButton.SetAlpha(startAlpha);
        _rightButton.SetAlpha(startAlpha);
        _downButton.SetAlpha(startAlpha);
        _leftButton.SetAlpha(startAlpha);
        float factor = 0f;
        while (factor < 1f)
        {
            yield return null;
            factor += _fadingSpeed * Time.deltaTime;
            float alpha = startAlpha + factor * multiplier;
            _upButton.SetAlpha(alpha);
            _rightButton.SetAlpha(alpha);
            _downButton.SetAlpha(alpha);
            _leftButton.SetAlpha(alpha);
        }
        startAlpha = 0.5f + multiplier * 0.5f;
        _upButton.SetAlpha(startAlpha);
        _rightButton.SetAlpha(startAlpha);
        _downButton.SetAlpha(startAlpha);
        _leftButton.SetAlpha(startAlpha);
    }

    public void ChangeMovementButtonsVisibility(bool canMoveUp, bool canMoveRight, bool canMoveDown, bool canMoveLeft)
    {
        _upButton.gameObject.SetActive(canMoveUp);
        _upButton.SetInteraction(canMoveUp);
        _rightButton.gameObject.SetActive(canMoveRight);
        _rightButton.SetInteraction(canMoveRight);
        _downButton.gameObject.SetActive(canMoveDown);
        _downButton.SetInteraction(canMoveDown);
        _leftButton.gameObject.SetActive(canMoveLeft);
        _leftButton.SetInteraction(canMoveLeft);
        StopCoroutine(_buttonsFading);
        _buttonsFading = _ButtonsFading(1f);
        StartCoroutine(_buttonsFading);
    }
}