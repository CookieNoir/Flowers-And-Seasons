using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public event Action<int, int> OnMove;
    public event Action OnDestinationReached;
    [SerializeField, Min(0.25f)] private float _speed;
    [SerializeField, Min(0.25f)] private float _rotationTime;
    [SerializeField] private float _startRotationAngle;
    [SerializeField] private DestinationPoint _destinationPoint;
    [Header("Animation")]
    [SerializeField] private Transform _animatedTransform;
    [SerializeField, Min(0f)] private float _maxAngle;
    [SerializeField, Min(0f)] private float _animationAngleChangeSpeed = 50f;
    private float _targetAngle = 0f;
    private float _angularVelocity = 0f;
    private Vector2Int _destinationPosition;
    private float _animationAngle = 0f;
    private float _angleMultiplier = 1f;
    private bool _isAnimating = false;
    private Vector2 _movementDirection;
    private bool[,] _obstaclesMap;
    public bool IsDestinationReached { get; private set; }

    private void Awake()
    {
        IsDestinationReached = true;
    }

    public Vector2Int SetStartPositionAndNavigationMap(Vector2Int startPosition, bool[,] navigationMap)
    {
        _obstaclesMap = navigationMap;
        _destinationPosition = startPosition;
        transform.position = new Vector3(_destinationPosition.x + 0.5f, 0f, _destinationPosition.y + 0.5f);
        transform.rotation = Quaternion.Euler(0f, _startRotationAngle, 0f);
        _targetAngle = _startRotationAngle;
        _destinationPoint.SetPosition(_destinationPosition);
        SetMovementButtonsVisibility();
        IsDestinationReached = true;
        return _destinationPosition;
    }

    public bool Move(MovementDirections direction)
    {
        if (_obstaclesMap == null) return false;
        else
        {
            bool changedDestination = false;
            Vector2Int newPosition = _destinationPosition;
            switch (direction)
            {
                case MovementDirections.Up:
                    {
                        if (newPosition.y < _obstaclesMap.GetLength(1) - 1)
                        {
                            newPosition.y++;
                            changedDestination = true;
                        }
                        break;
                    }
                case MovementDirections.Right:
                    {
                        if (newPosition.x < _obstaclesMap.GetLength(0) - 1)
                        {
                            newPosition.x++;
                            changedDestination = true;
                        }
                        break;
                    }
                case MovementDirections.Down:
                    {
                        if (newPosition.y > 0)
                        {
                            newPosition.y--;
                            changedDestination = true;
                        }
                        break;
                    }
                case MovementDirections.Left:
                    {
                        if (newPosition.x > 0)
                        {
                            newPosition.x--;
                            changedDestination = true;
                        }
                        break;
                    }
            }
            if (changedDestination && !_obstaclesMap[newPosition.x, newPosition.y])
            {
                _destinationPosition = newPosition;
                _destinationPoint.HideButtons();
                _movementDirection = new Vector2(_destinationPosition.x + 0.5f - transform.position.x, _destinationPosition.y + 0.5f - transform.position.z);
                _movementDirection.Normalize();
                _targetAngle = Mathf.Atan2(_destinationPosition.x + 0.5f - transform.position.x, _destinationPosition.y + 0.5f - transform.position.z) * Mathf.Rad2Deg;
                OnMove?.Invoke(_destinationPosition.x, _destinationPosition.y);
                IsDestinationReached = false;
                _isAnimating = true;
                return true;
            }
            else return false;
        }
    }

    public void SetMovementButtonsVisibility()
    {
        _destinationPoint.ChangeMovementButtonsVisibility(
        _destinationPosition.y + 1 < _obstaclesMap.GetLength(1) && !_obstaclesMap[_destinationPosition.x, _destinationPosition.y + 1],
        _destinationPosition.x + 1 < _obstaclesMap.GetLength(0) && !_obstaclesMap[_destinationPosition.x + 1, _destinationPosition.y],
        _destinationPosition.y > 0 && !_obstaclesMap[_destinationPosition.x, _destinationPosition.y - 1],
        _destinationPosition.x > 0 && !_obstaclesMap[_destinationPosition.x - 1, _destinationPosition.y]);
    }

    private void Update()
    {
        if (!IsDestinationReached)
        {
            Vector2 offset = _movementDirection * Time.deltaTime * _speed;
            Vector2 direction = new Vector2(_destinationPosition.x + 0.5f - transform.position.x, _destinationPosition.y + 0.5f - transform.position.z);
            transform.position += new Vector3(offset.x, 0f, offset.y);
            if (Vector2.Dot(_movementDirection, direction) < 0f)
            {
                transform.position = new Vector3(_destinationPosition.x + 0.5f, 0f, _destinationPosition.y + 0.5f);
                _destinationPoint.SetPosition(_destinationPosition);
                SetMovementButtonsVisibility();
                IsDestinationReached = true;
                OnDestinationReached?.Invoke();
            }
            transform.rotation = Quaternion.Euler(0f, Mathf.SmoothDampAngle(transform.rotation.eulerAngles.y, _targetAngle, ref _angularVelocity, _rotationTime), 0f);
            _animationAngle += Time.deltaTime * _animationAngleChangeSpeed * _angleMultiplier;
            if (Mathf.Abs(_animationAngle) > _maxAngle)
            {
                _animationAngle = _maxAngle * _angleMultiplier;
                _angleMultiplier *= -1f;
            }
            _animatedTransform.localRotation = Quaternion.Euler(0f, 0f, _animationAngle);
        }
        else 
        {
            if (_isAnimating) 
            {
                float prevAngle = _animationAngle;
                _animationAngle += Time.deltaTime * _animationAngleChangeSpeed * _angleMultiplier;
                if (prevAngle * _animationAngle < 0f)
                {
                    _animationAngle = 0f;
                    _isAnimating = false;
                }
                else if (Mathf.Abs(_animationAngle) > _maxAngle)
                {
                    _animationAngle = _maxAngle * _angleMultiplier;
                    _angleMultiplier *= -1f;
                }
                _animatedTransform.localRotation = Quaternion.Euler(0f, 0f, _animationAngle);
            }
        }
    }
}
