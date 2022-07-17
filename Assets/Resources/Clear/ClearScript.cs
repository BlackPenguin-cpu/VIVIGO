using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ClearScript : MonoBehaviour
{
    public GameObject GrandMaMa;
    public GameObject ViVi;
    public GameObject Text;
    public GameObject Effect;
    void Start()
    {
        GrandMaMa.transform.DOMoveX(GrandMaMa.transform.position.x - 5, 2).SetEase(Ease.InBack);
        ViVi.transform.DOMoveX(ViVi.transform.position.x + 5, 2).SetEase(Ease.InBack).OnComplete(() => OnComplete());
    }

    void OnComplete()
    {
        Text.transform.DOLocalMoveY(300, 2).SetEase(Ease.OutBounce);
        Effect.SetActive(true);
    }
}
