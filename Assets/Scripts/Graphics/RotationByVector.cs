using UnityEngine;

public class RotationByVector : MonoBehaviour
{
    [SerializeField] private Vector3 _rotationVector;

    void Update()
    {
        transform.rotation *= Quaternion.Euler(_rotationVector * Time.deltaTime);
    }
}