using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation_Floating : MonoBehaviour
{
    public float speed = 50f; // (px/sec)
    public float changeDirectionTime = 0.5f;

    private RectTransform rect;
    private Vector2 direction = Vector2.up;
    private float timer = 0f;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    void Update()
    {
        rect.anchoredPosition += direction * speed * Time.deltaTime;

        timer += Time.deltaTime;
        if (timer >= changeDirectionTime)
        {
            timer = 0f;
            direction *= -1;
            // rect.localScale = new Vector3(-rect.localScale.x, rect.localScale.y, rect.localScale.z);
        }
    }
}
