using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObstacleMover : MonoBehaviour
{
    private Vector3 moveDirection;
    private float moveSpeed;

    public void Initialize(Vector3 direction, float speed)
    {
        moveDirection = direction.normalized;
        moveSpeed = speed;
        Invoke("OnBecameInvisible", 10);
    }

    void Update()
    {
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.TakeDamage(); // ← ダメージを通知
            Destroy(gameObject);
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
        //Debug.Log("Destroy");
    }
}