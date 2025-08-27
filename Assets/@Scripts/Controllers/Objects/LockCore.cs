using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LockCore : MonoBehaviour
{
    public Sprite originalTile; // 플레이어 죽으면 다시 놀려놓기용
    public Sprite brokenTile;
    [SerializeField]
    public bool isBroken = false;

    private SpriteRenderer spriteRenderer;

    void OnEnable()
    {
        isBroken = false;
    }
    // 타일이 미사일에 의해 파괴된 경우 타일의 이미지를 변경, isBroken 상태 변경
    void Update()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (isBroken)
        {
            spriteRenderer.sprite = brokenTile;
        }
        else
        {
            spriteRenderer.sprite = originalTile;
        }
    }
}
