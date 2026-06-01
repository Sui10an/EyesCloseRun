using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] private GameObject obstaclePrefab;
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private float obstacleSpeed = 6f;
    [SerializeField] private float spawnX = 8f;

    // PlayerControllerの代わりにTransformで直接受け取る
    [SerializeField] private Transform player;

    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            timer = 0f;
            SpawnObstacle();
        }
    }

    void SpawnObstacle()
    {
        if (player == null) return;

        float side = (Random.value > 0.5f) ? 1f : -1f; // ランダムに左右どちらかから出す

        Vector3 spawnPos = new Vector3(
            side * spawnX,
            player.position.y + 3f, // プレイヤーと同じ高さに出す
            player.position.z + 6f // 少し前に出すことで、プレイヤーが障害物を見やすくする
        );

        GameObject obj = Instantiate(obstaclePrefab, spawnPos, Quaternion.identity);

        ObstacleMover mover = obj.GetComponent<ObstacleMover>();
        if (mover != null)
        {
            Vector3 dir = new Vector3(-side, 0f, 0f);
            mover.Initialize(dir, obstacleSpeed);
        }
    }
}