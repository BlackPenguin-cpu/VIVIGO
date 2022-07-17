using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceRoll : MonoBehaviour
{
    public GameObject dice;
    public Transform src;
    public Transform dst;
    public Animation animation;

    private void Start()
    {
        dice.SetActive(false);
    }
    public void RollTheDice()
    {
        dice.SetActive(true);
        animation.Play();
        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.5f);
        dice.SetActive(false);
    }
    
}
