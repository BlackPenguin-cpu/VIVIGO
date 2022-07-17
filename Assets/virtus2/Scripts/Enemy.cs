using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Enemy : MonoBehaviour
{
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
                Debug.Log(path[i].Cost);
                if (path[i].Cost > movementCost) break;
                dstNode = path[i];
            }
            // for DEBUG
            for (int i = 1; i < path.Count; i++)
            {
                Debug.Log(i + ": " + path[i].WorldPosition);
                Debug.DrawLine(path[i - 1].WorldPosition, path[i].WorldPosition, Color.red, 1.0f);
            }

            // dstNode가 이동하려는 노드임
            MoveTo(dstNode.WorldPosition);
            //transform.position = dstNode.WorldPosition;
            Debug.Log("추적: " +dstNode.WorldPosition);
        }
        else
        {
            // 이동하려는 타일이 없음. 가만히 있기
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Player Attack");
    }

    public void MoveTo(Vector3 dst)
    {
        Vector3 dir = (dst - transform.position);
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
                StartCoroutine(MoveAction(transform.position + dir));
            }
        }
        else
        {
            if (PathfindManager.Instance.ReachedGoal(transform.position))
            {
                GameManager.Instance.PlayerReachedGoal();
            }
        }
    }

    IEnumerator MoveAction(Vector3 vec)
    {
        GameManager.Instance.EnemyMoveStarted();
        OnMove = true;
        //GameObject obj = Instantiate(JumpEffect, Player.transform.position + Vector3.up * 0.3f, Quaternion.identity);
        Vector3 dir = vec - transform.position;
        //animator = Player.GetComponent<Animator>();
        SoundManager.Instance.SoundPlay("BB_Jump_Sound", SoundType.SFX, 1, 1);

        //animator.Play("JumpAnim");
        while (transform.position != vec)
        {
            transform.position = Vector3.MoveTowards(transform.position, vec, Time.deltaTime * 3);
            yield return null;
        }
        //animator.Play("IdleAnim");
        //Destroy(obj);
        TileCheck(dir);
        OnMove = false;
        GameManager.Instance.EnemyMoveFinished();
    }
}
