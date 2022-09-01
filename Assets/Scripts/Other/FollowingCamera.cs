using UnityEngine;

public class FollowingCamera : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _smoothTime = 0.3f;
    private Vector3 _velocity = Vector3.zero;

    private void Start()
    {
        enabled = _target;
    }

    public void SetTarget(Transform target)
    {
        _target = target;
        transform.position = _target.position;
        enabled = true;
    }

    private void LateUpdate()
    {
        transform.position = Vector3.SmoothDamp(transform.position, _target.position, ref _velocity, _smoothTime);
    }
}
