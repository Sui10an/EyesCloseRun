using UnityEngine;

public class RemoveManager : MonoBehaviour
{
    public Transform RespaewnPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Transform player = other.transform.parent;
            player.position = new Vector3(player.position.x, 0, player.position.z - 20f);
            other.transform.position = new Vector3(RespaewnPoint.position.x, RespaewnPoint.position.y, RespaewnPoint.position.z);
            GameManager.Instance.TakeDamage(); // ← ダメージを通知
        }
    }
}
