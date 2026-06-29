using UnityEngine;

public class ConveyorPlatform : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private float textureScrollScale = 0.1f;
    [SerializeField] private Renderer targetRenderer;

    private Transform playerTransform;
    private Vector2 textureOffset = Vector2.zero;

    void Update(){
        if(playerTransform != null){
            playerTransform.position +=Vector3.forward * speed * Time.deltaTime;
        }
        if(targetRenderer != null){
            textureOffset.y +=speed * textureScrollScale * Time.deltaTime;
            targetRenderer.material.mainTextureOffset = textureOffset;
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
