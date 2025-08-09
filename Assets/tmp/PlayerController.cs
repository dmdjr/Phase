using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Vector3 cameraTargetPosition;

    public Sprite player0Sprite; // ���� ��������Ʈ
    public Sprite player1Sprite; // ������ ��������Ʈ

    private SpriteRenderer playerRenderer;

    private Dictionary<GameObject, Color> originalColors = new Dictionary<GameObject, Color>();
    private Dictionary<GameObject, Color> invertedColors = new Dictionary<GameObject, Color>();
    private Color originalCameraColor;
    private Color invertedCameraColor;

    void Start()
    {
        playerRenderer = GetComponent<SpriteRenderer>();

        // Map, Escape �±� ������Ʈ �� ���� �� ���� �̸� ���
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

        // ī�޶� ���� ���� ���� �� ���� ���
        originalCameraColor = Camera.main.backgroundColor;
        invertedCameraColor = Invert(originalCameraColor);
    }

    void Update()
    {
        // �̵� ó��
        Vector3 move = Vector3.zero;
        if (Input.GetKey(KeyCode.UpArrow)) move += Vector3.up;
        if (Input.GetKey(KeyCode.DownArrow)) move += Vector3.down;
        if (Input.GetKey(KeyCode.LeftArrow)) move += Vector3.left;
        if (Input.GetKey(KeyCode.RightArrow)) move += Vector3.right;
        transform.position += move * moveSpeed * Time.deltaTime;

        // �����̽� Ȧ�� �߿� �̸� ����ص� ���� + ��������Ʈ ��ȯ ����
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

    // RGB ���� ���
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
