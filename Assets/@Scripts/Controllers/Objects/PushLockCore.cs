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

    private SpriteRenderer spriteRenderer;

    void OnEnable()
    {
        isPushed = false;
    }
    
    void Update()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (isPushed)
        {
            spriteRenderer.sprite = pushedTile;
        }
        else
        {
            spriteRenderer.sprite = originalTile;
        }
    }
}
