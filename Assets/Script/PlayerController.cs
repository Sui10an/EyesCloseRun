using UnityEngine;


public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    private Animator animator;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if ((BlinkDetector.isclose || Input.GetKey(KeyCode.W)) && GameManager.isGameActive == true)
        {
            animator.SetBool("isWalking", true);
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime, Space.World);
            Debug.Log("Go!!");
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
    }
}