using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;

public class GameStage : SingletonManager<GameStage>
{
    public WorldTilemap[] Stages;
    public GameObject StageGameObject;
    public int currentStage;

    private void Awake()
    {
        base.Awake();
        Stages = StageGameObject.GetComponentsInChildren<WorldTilemap>(true);
        Debug.Log(currentStage);
        Stages[currentStage].gameObject.SetActive(true);
        for (int i = 0; i < Stages.Length; i++)
        {
            if(i != currentStage)
                Stages[i].gameObject.SetActive(false);
        }
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
