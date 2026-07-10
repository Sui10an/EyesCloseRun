using UnityEngine;

// Goalオブジェクト(コライダーのみ)にアタッチする。
// Collider の「Is Trigger」にチェックを入れておくこと。
[RequireComponent(typeof(Collider))]
public class GoalTrigger : MonoBehaviour
{
    [SerializeField] private string playerTag = "Player";

    private bool hasTriggered = false;

    private void Reset()
    {
        // アタッチした瞬間に自動でIs TriggerをONにする
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasTriggered) return;
        if (!other.CompareTag(playerTag)) return;

        hasTriggered = true;
        GameManager.Instance.GameClear();
    }
}