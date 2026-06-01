using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    private void Update()
    {
        if (BlinkDetector.isclose)
        {
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime, Space.World);
            Debug.Log("Go!!");
        }
    }
}