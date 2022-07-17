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

    public GameObject[] wallPrefab;
    private List<GameObject> walls;

    public ArrowKeyManager arrowKeyManager;

    private void Awake()
    {
        base.Awake();
        enemies = new List<Enemy>();
        walls = new List<GameObject>();
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

    public void CreateObstacle(Vector3 worldPosition)
    {
        int rnd = Random.Range(0, wallPrefab.Length);
        var go = Instantiate(wallPrefab[rnd], worldPosition, new Quaternion());
        walls.Add(go);
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
        PathfindManager.Instance.ClearExploredTiles();
        Destroy(FindObjectOfType<Player>().gameObject);
        foreach (Enemy enemy in FindObjectsOfType<Enemy>())
        {
            Destroy(enemy.gameObject);
        }
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
