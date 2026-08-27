using UnityEngine;


public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float backSpeed = 5f;
    [SerializeField] public float jumpPower = 5f;
    private Animator animator;
    public Rigidbody rb;
    public bool isJumping = true;


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
        else if ((BlinkDetector.isotherclose || Input.GetKey(KeyCode.S)) && GameManager.isGameActive == true)
        {
            animator.SetBool("isWalking", true);
            transform.Translate(Vector3.back * backSpeed * Time.deltaTime, Space.World);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }

        // if ((ArmUpDetector.wasJump || Input.GetKey(KeyCode.Space)) &&
        //     (GameManager.isGameActive && isJumping) == true)
        // {
        //     Debug.Log("Jump!!");
        //     rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        // }
    }
}