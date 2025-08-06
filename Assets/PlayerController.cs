using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Vector3 cameraTargetPosition;

    public Sprite player0Sprite; // 원래 스프라이트
    public Sprite player1Sprite; // 반전된 스프라이트

    private SpriteRenderer playerRenderer;

    private Dictionary<GameObject, Color> originalColors = new Dictionary<GameObject, Color>();
    private Dictionary<GameObject, Color> invertedColors = new Dictionary<GameObject, Color>();
    private Color originalCameraColor;
    private Color invertedCameraColor;

    void Start()
    {
        playerRenderer = GetComponent<SpriteRenderer>();

        // Map, Escape 태그 오브젝트 색 저장 및 역색 미리 계산
        string[] targetTags = { "Map", "Escape" };
        foreach (string tag in targetTags)
        {
            GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);
            foreach (GameObject obj in objects)
            {
                SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
                if (sr != null)
                {
                    originalColors[obj] = sr.color;
                    invertedColors[obj] = Invert(sr.color);
                }
            }
        }

        // 카메라 원래 배경색 저장 및 역색 계산
        originalCameraColor = Camera.main.backgroundColor;
        invertedCameraColor = Invert(originalCameraColor);
    }

    void Update()
    {
        // 이동 처리
        Vector3 move = Vector3.zero;
        if (Input.GetKey(KeyCode.UpArrow)) move += Vector3.up;
        if (Input.GetKey(KeyCode.DownArrow)) move += Vector3.down;
        if (Input.GetKey(KeyCode.LeftArrow)) move += Vector3.left;
        if (Input.GetKey(KeyCode.RightArrow)) move += Vector3.right;
        transform.position += move * moveSpeed * Time.deltaTime;

        // 스페이스 홀드 중엔 미리 계산해둔 역색 + 스프라이트 전환 적용
        if (Input.GetKey(KeyCode.Space))
        {
            foreach (var pair in invertedColors)
            {
                var sr = pair.Key.GetComponent<SpriteRenderer>();
                if (sr != null) sr.color = pair.Value;
            }
            Camera.main.backgroundColor = invertedCameraColor;

            if (playerRenderer.sprite != player1Sprite)
                playerRenderer.sprite = player1Sprite;
        }
        else
        {
            foreach (var pair in originalColors)
            {
                var sr = pair.Key.GetComponent<SpriteRenderer>();
                if (sr != null) sr.color = pair.Value;
            }
            Camera.main.backgroundColor = originalCameraColor;

            if (playerRenderer.sprite != player0Sprite)
                playerRenderer.sprite = player0Sprite;
        }
    }

    // RGB 역색 계산
    private Color Invert(Color c)
    {
        return new Color(1f - c.r, 1f - c.g, 1f - c.b, c.a);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Escape"))
        {
            Camera.main.transform.position = new Vector3(
                cameraTargetPosition.x,
                cameraTargetPosition.y,
                Camera.main.transform.position.z
            );
        }
    }
}
