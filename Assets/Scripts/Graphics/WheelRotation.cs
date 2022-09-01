using UnityEngine;

public class WheelRotation : MonoBehaviour
{
    [SerializeField] private Transform[] _wheels;
    [SerializeField, Min(0.01f)] private float _wheelRadius;
    [Space(10)]
    [SerializeField] private Transform[] _rotatingWheels;
    [SerializeField, Range(0.01f, 90f)] private float _maxRotationAngle;
    [SerializeField, Min(0.01f)] private float _rotationSpeed = 30f;
    private Vector3 _previousPosition;
    private Quaternion _previousRotation;
    private float _wheelCircumference;

    private void _RandomizeRotations()
    {
        foreach (Transform wheel in _wheels)
        {
            wheel.localRotation *= Quaternion.Euler(Vector3.right * (360f * Random.Range(0f, 1f)));
        }
    }

    private void Start()
    {
        _previousPosition = transform.position;
        _previousRotation = transform.rotation;
        _wheelCircumference = _wheelRadius * Mathf.PI * 2f;
        _RandomizeRotations();
    }

    private void Update()
    {
        Vector3 currentPosition = transform.position;
        Vector3 direction = currentPosition - _previousPosition;
        float distance = direction.magnitude;
        float turn = 360f * (distance / _wheelCircumference) * Mathf.Sign(Vector3.Dot(direction, transform.forward));
        foreach (Transform wheel in _wheels)
        {
            wheel.localRotation *= Quaternion.Euler(Vector3.right * turn);
        }
        Quaternion currentRotation = transform.rotation;
        turn = Mathf.Min(Quaternion.Angle(currentRotation, _previousRotation) / Time.deltaTime, _maxRotationAngle)
            * Mathf.Sign(Vector3.Dot(direction, transform.right)) * -1f;
        foreach (Transform wheel in _rotatingWheels)
        {
            wheel.localRotation = Quaternion.RotateTowards(wheel.localRotation, Quaternion.Euler(Vector3.up * turn), _rotationSpeed * Time.deltaTime);
        }
        _previousPosition = currentPosition;
        _previousRotation = currentRotation;
    }

    private void OnDrawGizmosSelected()
    {
        if (_wheels != null && _wheels.Length > 0)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(_wheels[0].position, _wheelRadius);
        }
    }
}
