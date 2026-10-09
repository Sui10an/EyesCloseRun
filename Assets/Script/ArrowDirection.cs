using UnityEngine;

public class ArrowDirection : MonoBehaviour
{
    public Transform arrow;

    private Vector3 previousPosition;

    void Start()
    {
        previousPosition = transform.position;
    }

    void Update()
    {
        Vector3 moveDirection = transform.position - previousPosition;

        if (moveDirection.sqrMagnitude > 0.0001f)
        {
            arrow.forward = moveDirection.normalized;
        }

        previousPosition = transform.position;
    }
}