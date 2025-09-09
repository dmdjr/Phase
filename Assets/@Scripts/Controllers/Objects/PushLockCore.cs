using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PushLockCore : InvertibleObject
{
    public Sprite unpushedNormalSprite;
    public Sprite unpushedInvertedSprite;
    public Sprite pushedNormalSprite;
    public Sprite pushedInvertedSprite;

    [SerializeField]
    public bool isPushed = false;

    private bool previousIsPushedState = false;

    // LockCore와 동일한 Awake 로직
    private void Awake()
    {
        anim = GetComponentInParent<Animator>(); 
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

        isPushed = false;
        previousIsPushedState = false;
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
        if (isPushed != previousIsPushedState)
        {
            UpdateStateVisuals();
            previousIsPushedState = isPushed;
        }
    }

    private void UpdateStateVisuals()
    {
        if (anim != null)
        {
            anim.SetBool("IsPushed", isPushed);
        }

        if (InversionManager.Instance != null)
        {
            SetInvertedState(InversionManager.Instance.IsInvertedState);
        }
    }
}
