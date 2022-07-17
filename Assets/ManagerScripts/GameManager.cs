using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonManager<GameManager>
{

    public GameObject playerPrefab;
    private Player player;
    public bool PlayerCanMove = true;

    public GameObject[] enemyPrefab;
    private List<Enemy> enemies;
    public int movingEnemies = 0;

    public GameObject keyPrefab;

    public GameObject lockPrefab;

    public ArrowKeyManager arrowKeyManager;

    private void Awake()
    {
        base.Awake();
        enemies = new List<Enemy>();
    }
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

    public void CreateKey(Vector3 worldPosition)
    {
        var go = Instantiate(keyPrefab, worldPosition, new Quaternion());   
    }

    public void CreateLock(Vector3 worldPosition)
    {
        var go = Instantiate(lockPrefab, worldPosition, new Quaternion());
    }

    public void PlayerReachedGoal()
    {
        Debug.Log("플레이어 목표 타일 도달");
        GameStage.Instance.NextStage();
        // TODO: 스테이지또는 적 오브젝트를 Destroy?
        for (int i = 0; i < enemies.Count; i++)
        {
            Destroy(enemies[i].gameObject);
        }
        Destroy(player.gameObject);
        enemies.Clear();
    }

    public Player GetPlayerObject()
    {
        return player;
    }

    public void NextTurn()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].Pursuit();
        }
        PlayerCanMove = false;
    }

    public void EnemyMoveStarted()
    {
        movingEnemies++;
    }
    public void EnemyMoveFinished()
    {
        movingEnemies--;
        if (movingEnemies <= 0)
        {
            movingEnemies = 0;
            PlayerCanMove = true;
        }
    }
    
    
}
