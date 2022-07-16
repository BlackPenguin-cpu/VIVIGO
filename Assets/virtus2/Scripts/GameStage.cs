using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStage : MonoBehaviour
{
    public WorldTilemap[] Stages;
    public int currentStage = 0;

    private void Start()
    {
        Stages = GetComponentsInChildren<WorldTilemap>();
        Debug.Log(Stages.Length);

    }

    public void NextStage()
    {
        Stages[currentStage].gameObject.SetActive(false);
        currentStage++;
        Stages[currentStage].gameObject.SetActive(true);
    }
}
