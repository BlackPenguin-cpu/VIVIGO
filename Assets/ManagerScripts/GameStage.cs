using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;

public class GameStage : SingletonManager<GameStage>
{
    public WorldTilemap[] Stages;
    public GameObject StageGameObject;
    public int currentStage = 0;

    private void Awake()
    {
        base.Awake();
        Stages = StageGameObject.GetComponentsInChildren<WorldTilemap>(true);
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
