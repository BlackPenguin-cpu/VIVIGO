using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using DG.Tweening;
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

    public ArrowKeyManager arrowKeyManager;

    public PostProcessVolume volume;
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

    public void CreateObstacle(Vector3 worldPosition)
    {
        int rnd = Random.Range(0, wallPrefab.Length);
        var go = Instantiate(wallPrefab[rnd], worldPosition, new Quaternion());
    }

    public void PlayerReachedGoal()
    {
        Debug.Log("플레이어 목표 타일 도달");
        StartCoroutine(ClearEffect());
        // TODO: 스테이지또는 적 오브젝트를 Destroy?
    }
    IEnumerator ClearEffect()
    {
        float value = 0;
        volume.profile.TryGetSettings(out Vignette vignette);
        Camera.main.transform.DOMove(FindObjectOfType<Player>().transform.position + new Vector3(0, 0, -10), 1).SetEase(Ease.InCirc);
        while (value < 1)
        {
            vignette.intensity.value = value;
            value += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(1);
        vignette.intensity.value = 0;
        GameReset();
        enemies.Clear();
        GameStage.Instance.NextStage();
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
                enemies[i].Pursuit();
                PlayerCanMove = false;
            }
        }
    }

    public void GameOver()
    {
        //
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
