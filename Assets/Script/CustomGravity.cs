using UnityEngine;

public class CustomGravity : MonoBehaviour
{
    [SerializeField] private float gravityMultiplier = 3f; // 通常の何倍の重力にするか

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false; // デフォルトの重力を切る
    }

    private void FixedUpdate()
    {
        rb.AddForce(Physics.gravity * gravityMultiplier, ForceMode.Acceleration);
    }
}