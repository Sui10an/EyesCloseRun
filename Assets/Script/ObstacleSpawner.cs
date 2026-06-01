using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("発射する球")]
    public GameObject bulletPrefab;

    [Header("プレイヤー")]
    public Transform player;

    [Header("発射速度")]
    public float bulletSpeed = 10f;

    [Header("発射間隔")]
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
        // 球を生成
        GameObject bullet = Instantiate(
            bulletPrefab,
            transform.position,
            Quaternion.identity
        );

        // プレイヤー方向を計算
        Vector3 direction =
            (player.position - transform.position).normalized;

        // Rigidbody取得
        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        // 発射
        rb.linearVelocity = direction * bulletSpeed;
    }

}