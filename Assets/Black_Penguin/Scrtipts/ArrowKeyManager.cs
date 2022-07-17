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

    private void Start()
    {
        ReRoll();
        PannalSetting();
    }
    private void Update()
    {
        MoveInput();
        if (Input.GetKeyDown(KeyCode.Space))
            ReRoll();
    }
    public void MoveInput()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            MoveAction(0);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            MoveAction(1);
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            MoveAction(2);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            MoveAction(3);
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
                Debug.Log("키가 없네요 ㅠㅠ");
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
            Player.transform.position += (Vector3)dir;
        }
        PannalSetting();

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
