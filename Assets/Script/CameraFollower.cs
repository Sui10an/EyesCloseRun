using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    public Transform target; // Playerをここにアサイン
    public Vector3 offset = new Vector3(0f, 5f, -10f);

    void LateUpdate()
    {
        transform.position = target.position + offset;
    }
}