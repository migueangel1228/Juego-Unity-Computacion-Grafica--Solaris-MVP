using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;          // Arrastra aquí el Player
    public Vector3 offset = new Vector3(0f, 4f, -7f);  // Posición relativa al player
    public float smoothSpeed = 8f;

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        // La cámara siempre mira al player
        transform.LookAt(target.position + Vector3.up * 1.2f);
    }
}