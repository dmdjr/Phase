using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class InversionManager : MonoBehaviour
{
    public static InversionManager Instance { get; private set; }
    public bool IsInvertedState { get; private set; }
    [Header("타일맵 오브젝트 설정")]
    public TilemapRenderer normalTilemapRenderer;
    public TilemapRenderer normalHazardTileRenderer;
    public TilemapCollider2D normalTilemapCollider;
    public TilemapCollider2D normalHazardTileCollider;

    public TilemapRenderer invertedTilemapRenderer;
    public TilemapRenderer invertedHazardTileRenderer;
    public TilemapCollider2D invertedTilemapCollider;
    public TilemapCollider2D invertedHazardTileCollider;

    [Header("일반 오브젝트 설정")]
    private List<InvertibleObject> allInvertibleObjects = new List<InvertibleObject>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    private void Start()
    {
        // [변경점 2] 컴포넌트의 enabled 속성을 사용하여 초기 상태 설정
        // normalTilemap.SetActive(true); -> 아래와 같이 변경
        normalTilemapRenderer.enabled = true;
        normalTilemapCollider.enabled = true;
        normalHazardTileRenderer.enabled = true;
        normalHazardTileCollider.enabled = true;

        // invertedTilemap.SetActive(false); -> 아래와 같이 변경
        invertedTilemapRenderer.enabled = false;
        invertedTilemapCollider.enabled = false;
        invertedHazardTileRenderer.enabled = false;
        invertedHazardTileCollider.enabled = false;
    }
    public void RegisterObject(InvertibleObject obj)
    {
        if (!allInvertibleObjects.Contains(obj))
        {
            allInvertibleObjects.Add(obj);
        }
    }
    
    public void UnregisterObject(InvertibleObject obj)
    {
        if (allInvertibleObjects.Contains(obj))
        {
            allInvertibleObjects.Remove(obj);
        }
    }
    public void ToggleInversionState(bool isInverted)
    {
        IsInvertedState = isInverted;
        // [변경점 3] SetActive 대신 컴포넌트의 enabled 속성을 제어 (훨씬 가볍고 빠름)
        normalTilemapRenderer.enabled = !isInverted;
        normalTilemapCollider.enabled = !isInverted;
        normalHazardTileRenderer.enabled = !isInverted;
        normalHazardTileCollider.enabled = !isInverted;

        invertedTilemapRenderer.enabled = isInverted;
        invertedTilemapCollider.enabled = isInverted;
        invertedHazardTileRenderer.enabled = isInverted;
        invertedHazardTileCollider.enabled = isInverted;
        foreach (InvertibleObject obj in allInvertibleObjects)
        {
            if (obj != null)
            {
                obj.SetInvertedState(isInverted);
            }
        }
    }
}
