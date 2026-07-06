using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CollisonFloor : MonoBehaviour
{
    GameObject player;
    PlayerController playerScript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = transform.parent.gameObject;
        playerScript = player.GetComponent<PlayerController>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            playerScript.isJumping = true;
        }
    }
    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            playerScript.isJumping = false;
        }
    }
}
