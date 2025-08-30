using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PushLockCore : MonoBehaviour
{
    public Sprite originalTile; // 플레이어 죽으면 다시 놀려놓기용
    public Sprite pushedTile;
    [SerializeField]
    public bool isPushed = false;
    private Animator animator;

    private SpriteRenderer spriteRenderer;

    void OnEnable()
    {
        isPushed = false;
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        animator.enabled = false;
    }
    
    void Update()
    {
        if (isPushed)
        {
            spriteRenderer.sprite = pushedTile;
            animator.enabled = true;
            animator.SetBool("IsPushed", true);
        }
        else
        {
            spriteRenderer.sprite = originalTile;
            animator.SetBool("IsPushed", false);
        }
    }
}
