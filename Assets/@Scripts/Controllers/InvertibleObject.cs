using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class InvertibleObject : MonoBehaviour
{
    [Tooltip("���� ��������Ʈ")]
    public Sprite normalSprite;
    [Tooltip("������ ��������Ʈ")]
    public Sprite invertedSprite;

    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetInvertedState(bool isInverted)
    {
        spriteRenderer.sprite = !isInverted ? normalSprite : invertedSprite;
    }
}
