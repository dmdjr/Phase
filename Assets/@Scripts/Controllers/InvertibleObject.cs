using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class InvertibleObject : MonoBehaviour
{
    public enum InversionType { Sprite, Animation }
    [Header("동작 방식 설정")]
    public InversionType type = InversionType.Sprite;

    [Header("스프라이트 교체용")]
    [Tooltip("기존 스프라이트")]
    public Sprite normalSprite;
    [Tooltip("반전된 스프라이트")]
    public Sprite invertedSprite;

    [Header("애니메이션 교체용")]
    public string animatorParameterName = "isInverted";

    private SpriteRenderer spriteRenderer;
    private Animator anim;
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        if (InversionManager.Instance != null)
        {
            InversionManager.Instance.RegisterObject(this);
        }
    }
    private void OnDisable()
    {
        if (InversionManager.Instance != null)
        {
            InversionManager.Instance.UnregisterObject(this);
        }
    }
    public void SetInvertedState(bool isInverted)
    {
        switch (type)
        {
            case InversionType.Sprite:
                if (spriteRenderer != null)
                {
                    spriteRenderer.sprite = isInverted ? invertedSprite : normalSprite;
                }
                break;
            case InversionType.Animation:
                if (anim != null)
                {
                    anim.SetBool(animatorParameterName, isInverted);
                }
                break;
        }

    }
}
