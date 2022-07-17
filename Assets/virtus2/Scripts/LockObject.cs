using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockObject : MonoBehaviour
{
    public Vector3 WorldPosition;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Debug.LogWarning("Player Enter");
            var player = collision.gameObject.GetComponent<Player>();
            if (player.HasKey)
            {
                player.HasKey = false;
                Destroy(this.gameObject);
            }
        }
    }
}
