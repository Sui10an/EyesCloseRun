using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject bulletPrefab;

    public float bulletSpeed = 10f;

    public float fireInterval = 2f;

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= fireInterval)
        {
            Shoot();
            timer = 0f;
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(
            bulletPrefab,
            transform.position,
            Quaternion.identity
        );

        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        // X軸プラス方向へ飛ばす
        rb.linearVelocity = transform.right * bulletSpeed;
    }

}