using UnityEngine;

public class ConveyorPlatform : MonoBehaviour
{
    [SerializeField] private float speed = 2f;

    private Transform playerTransform;

    void Update(){
        if(playerTransform != null){
            playerTransform.position +=Vector3.forward * speed * Time.deltaTime;
        }
    }

    void OnCollisionEnter(Collision collision){
        if(collision.gameObject.CompareTag("Player")){
            playerTransform = collision.transform.root;
        }
    }

    void OnCollisionExit(Collision collision){
        if(collision.gameObject.CompareTag("Player")){
            playerTransform = null;
        }
    }
}
