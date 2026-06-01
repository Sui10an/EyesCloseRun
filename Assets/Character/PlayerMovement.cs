using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        bool isWalking = Input.GetKey(KeyCode.W);
        animator.SetBool("isWalking", isWalking);

        if (isWalking)
        {
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }
    }
}