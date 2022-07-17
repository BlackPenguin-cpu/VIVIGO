using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject JumpEffect;
    public Player player;
    public bool pursuit;
    private bool OnMove = false;
    public int movementCost;
    private void Awake()
    {
        player = GameManager.Instance.GetPlayerObject();
    }

    public void Pursuit()
    {
        if (!pursuit) return;
        if (player == null) player = GameManager.Instance.GetPlayerObject();
        // 이동하려는 타일 찾기
        var path = PathfindManager.Instance.GetPath(this.transform.position, player.transform.position);
        if (path != null)
        {
            Node dstNode = null;
            for (int i = 0; i < path.Count; i++)
            {
                if (path[i].Cost > movementCost) break;
                dstNode = path[i];
            }
            // for DEBUG
            for (int i = 1; i < path.Count; i++)
            {
                Debug.DrawLine(path[i - 1].WorldPosition, path[i].WorldPosition, Color.red, 1.0f);
            }

            // dstNode가 이동하려는 노드임
            MoveTo(dstNode.WorldPosition);
            //transform.position = dstNode.WorldPosition;
            Debug.Log("추적: " +dstNode.WorldPosition);
        }
        else
        {
            Debug.Log("추적 실패");
            // 이동하려는 타일이 없음. 가만히 있기
            GameManager.Instance.EnemyMoveFinished();
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Player Attack");
        GameManager.Instance.GameOver();
    }

    public void MoveTo(Vector3 dst)
    {
        Vector3 dir = (dst - transform.position);
        Debug.Log(dir.normalized);
        StartCoroutine(MoveAction(transform.position + dir));
        /*
        if (PathfindManager.Instance.CanMove(transform.position + dir))
        {
            transform.position += dir;
            // 얼음 이동
            while (PathfindManager.Instance.GetTileType(transform.position) == TILE_TYPE.ICE)
            {
                if (PathfindManager.Instance.CanMove(transform.position + (Vector3)dir))
                {
                    transform.position += (Vector3)dir;
                }
                else
                {
                    break;
                }
            }
        }*/
    }

    void TileCheck(Vector3 dir)
    {
        // 얼음 이동
        if (PathfindManager.Instance.GetTileType(transform.position) == TILE_TYPE.ICE)
        {
            if (PathfindManager.Instance.CanMove(transform.position + dir))
            {
                //StartCoroutine(MoveAction(transform.position + dir));
            }
        }
        else
        {

        }
    }

    IEnumerator MoveAction(Vector3 vec)
    {
        GameManager.Instance.EnemyMoveStarted();
        OnMove = true;
        GameObject obj = Instantiate(JumpEffect, transform.position + Vector3.up * 0.3f, Quaternion.identity);
        Vector3 dir = vec - transform.position;
        var animator = GetComponent<Animator>();
        SoundManager.Instance.SoundPlay("Enemy_Move_Sound", SoundType.SFX, 1, 1);

        animator.Play("JumpAnim");
        while (transform.position != vec)
        {
            transform.position = Vector3.MoveTowards(transform.position, vec, Time.deltaTime * 3);
            yield return null;
        }
        animator.Play("IdleAnim");
        Destroy(obj);
        TileCheck(dir);
        OnMove = false;
        GameManager.Instance.EnemyMoveFinished();

        var v = player.transform.position - transform.position;
        if(v.normalized.y < 0)
            GetComponent<SpriteRenderer>().flipX = false;
        else
            GetComponent<SpriteRenderer>().flipX = true;
    }
}
