using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceRoll : MonoBehaviour
{
    public GameObject dice;
    public Animator animator;

    private void Start()
    {
        dice.SetActive(false);
        animator = GetComponent<Animator>();
    }
    public void RollTheDice()
    {
        dice.SetActive(true);
        animator.Play("Dice");
        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.5f);
        dice.SetActive(false);
    }
    
}
