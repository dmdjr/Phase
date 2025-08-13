using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class InvertibleObject : MonoBehaviour
{
    [Tooltip("기존 스프라이트")]
    public Sprite normalSprite;
    [Tooltip("반전된 스프라이트")]
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
