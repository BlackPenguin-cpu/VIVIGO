using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ArrowKey
{
    RIGHT,
    LEFT,
    UP,
    DOWN
}
public class ArrowKeyManager : MonoBehaviour
{
    public List<ArrowKey> arrowKeys = new List<ArrowKey>(5);
    public GameObject Player;
    public GameObject ArrowKeyParentObj;
    public GameObject[] ArrowKeyObj;

    public Animator animator;

    private void Start()
    {
        ReRoll();
        PannalSetting();


        SoundManager.Instance.SoundPlay("BackGroundMusic", SoundType.BGM, 1, 1);
    }
    private void Update()
    {
        MoveInput();
        if (Input.GetKeyDown(KeyCode.Space))
            ReRoll();
    }
    public void MoveInput()
    {
        float ver = Input.GetAxisRaw("Vertical");
        float hor = Input.GetAxisRaw("Horizontal");
        if (Input.anyKeyDown)
        {
            if (hor == -1)
            {
                MoveAction(0);
            }
            else if (hor == 1)
            {
                MoveAction(1);
            }
            else if (ver == 1)
            {
                MoveAction(2);
            }
            else if (ver == -1)
            {
                MoveAction(3);
            }
        }
    }
    private void PannalSetting()
    {
        for (int i = 0; i < ArrowKeyParentObj.transform.childCount; i++)
        {
            Destroy(ArrowKeyParentObj.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < arrowKeys.Count; i++)
        {
            Instantiate(ArrowKeyObj[(int)arrowKeys[i]], ArrowKeyParentObj.transform);
        }
    }
    public void MoveAction(int index)
    {
        for (int i = 0; i < arrowKeys.Count + 1; i++)
        {
            if (i == arrowKeys.Count)
            {
                Debug.Log("해당 방향키 없음");
                return;
            }
            if (arrowKeys[i] == (ArrowKey)index)
            {
                arrowKeys.RemoveAt(i);
                break;
            }
        }

        Vector2 dir = Vector2.zero;
        switch ((ArrowKey)index)
        {
            case ArrowKey.LEFT:
                dir = Vector2.left;
                break;
            case ArrowKey.RIGHT:
                dir = Vector2.right;
                break;
            case ArrowKey.UP:
                dir = Vector2.up;
                break;
            case ArrowKey.DOWN:
                dir = Vector2.down;
                break;
        }

        if (PathfindManager.Instance.CanMove(Player.transform.position + (Vector3)dir))
        {
            StartCoroutine(PlayerMoveAction(Player.transform.position + (Vector3)dir));
            // 얼음 이동

        }
        if (PathfindManager.Instance.ReachedGoal(Player.transform.position))
        {
            GameManager.Instance.PlayerReachedGoal();
        }
        PannalSetting();
        SoundManager.Instance.SoundPlay("BB_Jump_Sound", SoundType.SFX, 1, 1);
    }
    void TileCheck()
    {
        if (PathfindManager.Instance.GetTileType(Player.transform.position) == TILE_TYPE.ICE)
        {
            if (PathfindManager.Instance.CanMove(Player.transform.position + (Vector3)dir))
            {
                StartCoroutine(PlayerMoveAction(Player.transform.position + (Vector3)dir));
            }
        }
    }
    IEnumerator PlayerMoveAction(Vector3 vec)
    {
        animator = Player.GetComponent<Animator>();

        animator.Play("JumpAnim");
        while (Player.transform.position != vec)
        {
            Debug.Log("asd");
            Player.transform.position = Vector3.MoveTowards(Player.transform.position, vec, Time.deltaTime);
            yield return null;
        }
        animator.Play("IdleAnim");

    }

    public void ReRoll()
    {
        if (arrowKeys.Count > 4) return;

        arrowKeys.Clear();
        while (arrowKeys.Count < 5)
        {
            ArrowKey key = (ArrowKey)Random.Range(0, 4);
            if (arrowKeys.FindAll(x => x == key).Count >= 2)
                continue;
            arrowKeys.Add(key);
        }
        PannalSetting();
    }
}
