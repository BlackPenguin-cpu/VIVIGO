using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonManager<GameManager>
{
    public GameObject playerPrefab;
    private Player player;

    public GameObject[] enemyPrefab;
    private List<Enemy> enemies;
    public ArrowKeyManager arrowKeyManager;

    public void CreatePlayer(Vector3 worldPosition)
    {
        player = Instantiate(playerPrefab, worldPosition, new Quaternion(0, 0, 0, 0)).GetComponent<Player>();
        arrowKeyManager.Player = player.gameObject;
    }

    public void CreateMonster(Vector3 worldPosition, ENEMY_TYPE enemyType)
    {
        var go = Instantiate(enemyPrefab[(int)enemyType], worldPosition, new Quaternion()).GetComponent<Enemy>();
        enemies.Add(go);
    }

    public Player GetPlayerObject()
    {
        return player;
    }
    
    
}
