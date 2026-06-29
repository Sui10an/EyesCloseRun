using UnityEngine;
public class MovingPlatform : MonoBehaviour
{
    public float moveDistance = 5f;
    public float speed = 2f;
    private Vector3 startPos;
    void Start()
    {
        startPos = transform.position;
    }
    void Update()
    {
        float offset = Mathf.PingPong(Time.time * speed, moveDistance);
        transform.position = startPos + new Vector3(0, 0, offset);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.parent.SetParent(transform);
        }
    }
    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.parent.SetParent(null);
        }
    }
}