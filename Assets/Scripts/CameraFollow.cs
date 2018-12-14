using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public Transform target;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;
    void FixedUpdate()
    {
        //if (target.position.x > -10)
        //{
            Vector3 desiredPoistion = target.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPoistion, smoothSpeed);
            transform.position = smoothedPosition;
        //}
    }
}
