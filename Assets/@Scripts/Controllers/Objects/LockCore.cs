using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static InvertibleObject;

public class LockCore : InvertibleObject
{
    public Sprite unbrokenNormalSprite;
    public Sprite unbrokenInvertedSprite;
    public Sprite brokenNormalSprite;
    public Sprite brokenInvertedSprite;

    [Header("상태 변수")]
    [SerializeField]
    public bool isBroken = false;

    private bool previousIsBrokenState = false;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        if (anim != null)
        {
            anim.enabled = true;
        }
    }

    private void OnEnable()
    {
        if (InversionManager.Instance != null)
        {
            InversionManager.Instance.RegisterObject(this);
        }

        // 초기 상태 설정
        isBroken = false;
        previousIsBrokenState = false;
        UpdateStateVisuals();

        if (InversionManager.Instance != null)
        {
            SetInvertedState(InversionManager.Instance.IsInvertedState);
        }
    }

    private void OnDisable()
    {
        if (InversionManager.Instance != null)
        {
            InversionManager.Instance.UnregisterObject(this);
        }
    }

    void Update()
    {
        if (isBroken != previousIsBrokenState)
        {
            UpdateStateVisuals();
            previousIsBrokenState = isBroken;
        }
    }

    private void UpdateStateVisuals()
    {
        if (anim != null)
        {
            anim.SetBool("IsBroken", isBroken);
        }


        if (InversionManager.Instance != null)
        {
            SetInvertedState(InversionManager.Instance.IsInvertedState);
        }
    }
}
