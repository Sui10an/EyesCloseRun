using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform spawnPoint;

    public float bulletSpeed = 10f;

    public float fireInterval = 2f;

    private float timer;

    [Header("サウンド")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip fireSound;

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
            spawnPoint.position,
            spawnPoint.rotation
        );

        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        // X軸プラス方向へ飛ばす
        rb.linearVelocity = transform.right * bulletSpeed;

        // ★追加:発射SEを再生
        if (audioSource != null && fireSound != null)
        {
            audioSource.PlayOneShot(fireSound);
        }
    }

}