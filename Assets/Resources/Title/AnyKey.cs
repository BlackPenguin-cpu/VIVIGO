using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnyKey : MonoBehaviour
{
    Image image;
    private void Start()
    {
        image = GetComponent<Image>();
    }
    private void Update()
    {
        image.color = new Color(1, 1, 1, Mathf.Abs(Mathf.Sin(Time.time)) + 0.1f);
        transform.position += new Vector3(0, Mathf.Cos(Time.time) * Time.deltaTime * 0.8f, 0);
    }
}
