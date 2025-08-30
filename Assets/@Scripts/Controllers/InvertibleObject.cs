using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class InvertibleObject : MonoBehaviour
{
    public enum InversionType { Sprite, Animation }
    [Header("���� ��� ����")]
    public InversionType type = InversionType.Sprite;

    [Header("��������Ʈ ��ü��")]
    [Tooltip("���� ��������Ʈ")]
    public Sprite normalSprite;
    [Tooltip("������ ��������Ʈ")]
    public Sprite invertedSprite;

    [Header("�ִϸ��̼� ��ü��")]
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
