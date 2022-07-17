using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using DG.Tweening;
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

    public GameObject vinePrefab;

    public GameObject keyPrefab;
    private GameObject key;

    public GameObject lockPrefab;
    private List<LockObject> locks;

    public GameObject[] wallPrefab;
    private List<GameObject> walls;

    public ArrowKeyManager arrowKeyManager;

    public PostProcessVolume volume;
    public RectTransform ClearUI;

    public bool isEnd;
    protected override void Awake()
    {
        base.Awake();
        enemies = new List<Enemy>();
        walls = new List<GameObject>();
        locks = new List<LockObject>();
    }
    public void CreatePlayer(Vector3 worldPosition)
    {
        player = Instantiate(playerPrefab, worldPosition, new Quaternion(0, 0, 0, 0)).GetComponent<Player>();
        player.CurrentPosition = worldPosition;
        arrowKeyManager.Player = player.gameObject;
    }

    public void CreateGrandma(Vector3 worldPosition)
    {
        grandma = Instantiate(grandmaPrefab, worldPosition, new Quaternion());
    }
    public void CreateMonster(Vector3 worldPosition, ENEMY_TYPE enemyType)
    {
        Enemy enemy = null;
        switch (enemyType)
        {
            case ENEMY_TYPE.VINE:
                enemy = Instantiate(vinePrefab, worldPosition, new Quaternion()).GetComponent<Enemy>();
                break;
            case ENEMY_TYPE.CAT:
                int rnd = Random.Range(0, enemyPrefab.Length);
                enemy = Instantiate(enemyPrefab[rnd], worldPosition, new Quaternion()).GetComponent<Enemy>();
                break;
                
        }
        enemies.Add(enemy);
    }

    public void CreateKey(Vector3 worldPosition)
    {
        var go = Instantiate(keyPrefab, worldPosition, new Quaternion());
        key = go;
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
        StartCoroutine(ClearEffect());
    }

    public void PlayerHasKey()
    {
        player.HasKey = true;
        for (int i = 0; i < locks.Count; i++)
        {
            locks[i].Unlock();
        }
    }
    IEnumerator ClearEffect()
    {
        player.transform.GetChild(0).gameObject.SetActive(true);
        float value = 0;
        volume.profile.TryGetSettings(out Vignette vignette);
        Camera.main.transform.DOMove(FindObjectOfType<Player>().transform.position + new Vector3(0, 0, -10), 1).SetEase(Ease.InCirc);
        while (value < 1)
        {
            vignette.intensity.value = value;
            value += Time.deltaTime;
            yield return null;
        }
        player.GetComponent<Animator>().Play("JumpAnim");
        player.transform.DOMove(player.transform.position + (player.GetComponent<SpriteRenderer>().flipX ? Vector3.left : Vector3.right) * 5, 3);
        yield return new WaitForSeconds(2);
        ClearUI.DOScaleX(1, 1);
        yield return new WaitForSeconds(2);
        Camera.main.transform.position = new Vector3(0, 0, -10);
        ClearUI.localScale = new Vector3(0, 1, 1);
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

        for (int i = 0; i < locks.Count; i++)
        {
            if(locks[i].gameObject != null)
                Destroy(locks[i].gameObject);
        }
        Destroy(player.gameObject);
        if(key != null) Destroy(key.gameObject);
        enemies.Clear();
        walls.Clear();
        locks.Clear();

        arrowKeyManager.ReRoll();

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
        if (!isEnd)
        {
            isEnd = true;
            StartCoroutine(GameOverCoroutine());
        }
    }
    IEnumerator GameOverCoroutine()
    {
        player.GetComponent<Animator>().Play("HitAnim");
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

        Camera.main.transform.position = new Vector3(0, 0, -10);
        vignette.intensity.value = 0;

        GameReset();
        GameRestart();
        isEnd = false;
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
