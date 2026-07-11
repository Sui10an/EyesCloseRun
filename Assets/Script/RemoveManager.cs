using UnityEngine;

public class RemoveManager : MonoBehaviour
{
    public Transform RespaewnPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Transform player = other.transform.parent;
            player.position = new Vector3(player.position.x, 0, RespaewnPoint.position.z);
            other.transform.position = new Vector3(RespaewnPoint.position.x, RespaewnPoint.position.y, other.transform.position.z);
            GameManager.Instance.TakeDamage(); // ← ダメージを通知
        }
    }
}
