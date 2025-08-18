using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LockObject : MonoBehaviour
{
    // 현재 스테이지 정보를 받기
    // int currentStage = 0;
    // 현재 스테이지가 탈출 가능 상태인지 알기
    // bool isOpen = false;
    // 탈출 가능 상태라면 문 열기
    /* 현재 스테이지에서 하나의 타일이라도 깨지지 않은 상태(isBroken == false)라면 isOpen == false */
    /* 위 작업은 StageManager에서 하기?? */

    public Sprite originalTile; // 플레이어 죽으면 다시 놀려놓기용
    public Sprite brokenTile;
    [SerializeField]
    public bool isBroken = false;

    private SpriteRenderer spriteRenderer;

    // 타일이 미사일에 의해 파괴된 경우 타일의 이미지를 변경, isBroken 상태 변경
    void Update()
    {
        if (isBroken)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = brokenTile;
        }
    }
}
