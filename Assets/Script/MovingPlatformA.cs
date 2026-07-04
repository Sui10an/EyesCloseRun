
using UnityEngine;
using System.Collections;

public class MovingPlatformA : MonoBehaviour
{
    public float moveDistance = 5f;
    public float speed = 2f;
    public float waitTime = 1f;

    private Vector3 startPos;
    private bool movingForward = true;
    private bool isWaiting = false;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        if (isWaiting) return;

        Vector3 targetPos = movingForward
            ? startPos + new Vector3(0, 0, moveDistance)
            : startPos;

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPos,
            speed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, targetPos) < 0.01f)
        {
            StartCoroutine(WaitAndChangeDirection());
        }
    }

    IEnumerator WaitAndChangeDirection()
    {
        isWaiting = true;

        yield return new WaitForSeconds(waitTime);

        movingForward = !movingForward;
        isWaiting = false;
    }


    void OnCollisionStay(Collision collision)
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
            if (collision.transform.parent == transform)
            {
                collision.transform.parent.SetParent(null);
            }
        }
    }

}
