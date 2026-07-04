using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    public Transform target; // Playerをここにアサイン
    public Vector3 offset = new Vector3(0f, 50f, -100f);

    void Update()
    {
        transform.position = target.position + offset;
    }
}