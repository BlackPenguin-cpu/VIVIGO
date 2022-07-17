using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainTitle : MonoBehaviour
{
    private void Update()
    {
        if (Input.anyKeyDown)
        {
            SoundManager.Instance.SoundPlay("Click", SoundType.SFX, 1, 1);
            SceneManager.LoadScene("GameScene");
        }
    }


}
