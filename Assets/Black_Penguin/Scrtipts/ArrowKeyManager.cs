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

    private void Start()
    {
        ReRoll();
    }
    private void Update()
    {
        MoveInput();
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
    public void MoveAction(int index)
    {
        if (arrowKeys[index] > 0)
        {
            arrowKeys[index]--;
        }
        else
        {
            Debug.Log("키가 없네요 ㅠㅠ");
            return;
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

        Player.transform.position += (Vector3)dir;
    }
    public void ReRoll()
    {
        for (int i = 0; i < 5; i++)
        {
            arrowKeys[i] = (ArrowKey)Random.Range(0, 5);
        }
    }
}
