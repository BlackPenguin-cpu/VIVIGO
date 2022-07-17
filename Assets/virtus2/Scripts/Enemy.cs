using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Player player;
    public int movementCost;
    private void Awake()
    {
        player = GameManager.Instance.GetPlayerObject();
    }

    public void Pursuit()
    {
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
            transform.position = dstNode.WorldPosition;
            Debug.Log("추적: " +dstNode.WorldPosition);
        }
        else
        {
            // 이동하려는 타일이 없음. 가만히 있기
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("ASD");
    }
}
