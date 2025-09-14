using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class InversionManager : MonoBehaviour
{
    public static InversionManager Instance { get; private set; }
    public bool IsInvertedState { get; private set; }
    [Header("타일맵 오브젝트 설정")]
    public TilemapRenderer normalTilemap;
    public TilemapRenderer normalHazardTile_isTrigger;
    public TilemapRenderer normalHazardTile_notTrigger;
    public TilemapCollider2D normalTilemapCollider;
    public TilemapCollider2D normalHazardTileCollider_isTrigger;
    public TilemapCollider2D normalHazardTileCollider_notTrigger;

    public TilemapRenderer invertedTilemap;
    public TilemapRenderer invertedHazardTile__isTrigger;
    public TilemapRenderer invertedHazardTile_notTrigger;
    public TilemapCollider2D invertedTilemapCollider;
    public TilemapCollider2D invertedHazardTileCollider_isTrigger;
    public TilemapCollider2D invertedHazardTileCollider_notRigger;

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
        normalTilemap.enabled = true;
        normalTilemapCollider.enabled = true;
        normalHazardTile_isTrigger.enabled = true;
        normalHazardTileCollider_isTrigger.enabled = true;
        normalHazardTile_notTrigger.enabled = true;
        normalHazardTileCollider_notTrigger.enabled = true;

        invertedTilemap.enabled = false;
        invertedTilemapCollider.enabled = false;
        invertedHazardTile__isTrigger.enabled = false;
        invertedHazardTileCollider_isTrigger.enabled = false;
        invertedHazardTile_notTrigger.enabled = false;
        invertedHazardTileCollider_notRigger.enabled = false;
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
        normalTilemap.enabled = !isInverted;
        normalTilemapCollider.enabled = !isInverted;
        normalHazardTile_isTrigger.enabled = !isInverted;
        normalHazardTileCollider_isTrigger.enabled = !isInverted;
        normalHazardTile_notTrigger.enabled = !isInverted;
        normalHazardTileCollider_notTrigger.enabled = !isInverted;

        invertedTilemap.enabled = isInverted;
        invertedTilemapCollider.enabled = isInverted;
        invertedHazardTile__isTrigger.enabled = isInverted;
        invertedHazardTileCollider_isTrigger.enabled = isInverted;
        invertedHazardTile_notTrigger.enabled = isInverted;
        invertedHazardTileCollider_notRigger.enabled = isInverted;
        foreach (InvertibleObject obj in allInvertibleObjects)
        {
            if (obj != null)
            {
                obj.SetInvertedState(isInverted);
            }
        }
    }
}
