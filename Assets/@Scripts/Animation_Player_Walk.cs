using UnityEngine;

public class Animation_Player_Walk : MonoBehaviour
{
    public float speed = 100f; // (px/sec)
    public float changeDirectionTime = 10f; 

    private RectTransform rect;
    private Vector2 direction = Vector2.left;
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
            rect.localScale = new Vector3(-rect.localScale.x, rect.localScale.y, rect.localScale.z);
        }
    }
}
