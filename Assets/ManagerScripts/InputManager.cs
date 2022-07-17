using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : SingletonManager<InputManager>
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (FindObjectOfType<ArrowKeyManager>().OnMove) return;
        if (Input.GetKeyDown(KeyCode.R))
        {
            // Restart
            // TODO: 게임오버한셈 치고 재시작함(야매)
            // TODO: 나중에 바꿔야할 수도 있을까
            GameManager.Instance.GameOver();
        }

        if (Input.GetKeyUp(KeyCode.T))
        {
            // Skip
            // TODO: 클리어한것으로 치고 다음으로 넘어감(야매)
            // TODO: 나중에 바꿔야할 수도 있을까
            GameManager.Instance.PlayerReachedGoal();
        }

    }
}
