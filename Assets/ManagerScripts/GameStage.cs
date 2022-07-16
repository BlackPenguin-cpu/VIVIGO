using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStage : SingletonManager<GameStage>
{
    public WorldTilemap[] Stages;
    public int currentStage = 0;

    private void Start()
    {
        Stages = GetComponentsInChildren<WorldTilemap>(true);
        Debug.Log(Stages.Length);

    }

    public void NextStage()
    {
        Stages[currentStage].gameObject.SetActive(false);
        currentStage++;
        Stages[currentStage].gameObject.SetActive(true);
    }

    public WorldTilemap GetCurrentTilemap()
    {
        return Stages[currentStage];
    }
}
