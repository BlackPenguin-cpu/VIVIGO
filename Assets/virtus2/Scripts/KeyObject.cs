using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyObject : MonoBehaviour
{
    public Vector3 WorldPosition;

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.LogWarning("Trigger Enter");
        if (collision.tag == "Player")
        {
            var player = collision.gameObject.GetComponent<Player>();
            GameManager.Instance.PlayerHasKey();

            Destroy(this.gameObject);
        }
    }
}
