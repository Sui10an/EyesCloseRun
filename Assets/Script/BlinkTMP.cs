using TMPro;
using UnityEngine;
using System.Collections;

public class BlinkTMP : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private float speed = 2f;

    void Update()
    {
        Color c = text.color;
        c.a = (Mathf.Sin(Time.time * speed) + 1f) / 2f;
        text.color = c;
    }
}