using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameManager : SingletonManager<GameManager>
{
    public GameObject playerPrefab;
    private Player player;
    public bool PlayerCanMove = true;

    public GameObject grandmaPrefab;
    private GameObject grandma;

    public GameObject[] enemyPrefab;
    private List<Enemy> enemies;
    public int movingEnemies = 0;

    public GameObject keyPrefab;

    public GameObject lockPrefab;
    private List<LockObject> locks;

    public GameObject[] wallPrefab;
    private List<GameObject> walls;

    public ArrowKeyManager arrowKeyManager;

    private void Awake()
    {
        base.Awake();
        enemies = new List<Enemy>();
        walls = new List<GameObject>();
        locks = new List<LockObject>();
    }
    public void CreatePlayer(Vector3 worldPosition)
    {
        player = Instantiate(playerPrefab, worldPosition, new Quaternion(0, 0, 0, 0)).GetComponent<Player>();
        arrowKeyManager.Player = player.gameObject;
    }

    public void CreateGrandma(Vector3 worldPosition)
    {
        grandma = Instantiate(grandmaPrefab, worldPosition, new Quaternion());
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
        locks.Add(go.GetComponent<LockObject>());
    }

    public void CreateObstacle(Vector3 worldPosition)
    {
        int rnd = Random.Range(0, wallPrefab.Length);
        var go = Instantiate(wallPrefab[rnd], worldPosition, new Quaternion());
        walls.Add(go);
    }

    public void PlayerReachedGoal()
    {
        Debug.Log("플레이어 목표 타일 도달");
        GameReset();
        GameStage.Instance.NextStage();
    }

    public void PlayerHasKey()
    {
        player.HasKey = true;
        for(int i=0; i<locks.Count; i++)
        {
            locks[i].Unlock();
        }
    }
    IEnumerator ClearEffect()
    {
        while (true)
        {

            yield return null;
        }
    }
    /// <summary>
    /// 게임 초기화시 사용
    /// </summary>
    public void GameReset()
    {
        player.HasKey = false;
        PlayerCanMove = true;
        Destroy(grandma);
        PathfindManager.Instance.ClearExploredTiles();
        for (int i = 0; i < enemies.Count; i++)
        {
            Destroy(enemies[i].gameObject);
        }

        for (int i = 0; i < walls.Count; i++)
        {
            Destroy(walls[i].gameObject);
        }
        Destroy(player.gameObject);
        enemies.Clear();
        walls.Clear();
        
    }
    public Player GetPlayerObject()
    {
        return player;
    }

    public void NextTurn()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i].pursuit)
            {
                PlayerCanMove = false;
                Debug.Log("now player can't move");
                enemies[i].Pursuit();
            }
        }
    }

    public void GameOver()
    {
        //
        GameReset();
        GameRestart();
    }

    public void GameRestart()
    {
        
        GameStage.Instance.GetCurrentTilemap().SpawnObjects();
    }
    public void EnemyMoveStarted()
    {
        Debug.Log("enemy move started");
        movingEnemies++;
    }
    public void EnemyMoveFinished()
    {
        Debug.Log("enemy move finished");
        movingEnemies--;
        if (movingEnemies <= 0)
        {
            movingEnemies = 0;
            Debug.Log("now player can move");
            PlayerCanMove = true;
        }
    }


}
