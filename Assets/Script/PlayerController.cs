using UnityEngine;


public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    private Animator animator;
    public Rigidbody rb;
    public float jumpPower = 5f;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if ((BlinkDetector.isclose || Input.GetKey(KeyCode.W)) && GameManager.isGameActive == true)
        {
            animator.SetBool("isWalking", true);
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime, Space.World);
            // Debug.Log("Go!!");
        }
        else
        {
            animator.SetBool("isWalking", false);
        }

        if ((ArmUpDetector.wasJump || Input.GetKey(KeyCode.Space)) && GameManager.isGameActive == true)
        {
            rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        }
    }
}