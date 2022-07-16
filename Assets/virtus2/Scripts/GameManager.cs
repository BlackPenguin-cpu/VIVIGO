using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonManager<GameManager>
{
    public GameObject playerPrefab;
    private Player player;
    public ArrowKeyManager arrowKeyManager;
    public void CreatePlayer(Vector3 worldPosition)
    {
        player = Instantiate(playerPrefab, worldPosition, new Quaternion(0, 0, 0, 0)).GetComponent<Player>();
        arrowKeyManager.Player = player.gameObject;
    }
}
