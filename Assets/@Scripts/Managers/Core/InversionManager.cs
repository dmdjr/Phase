using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class InversionManager : MonoBehaviour
{
    public static InversionManager Instance { get; private set; }
    public bool IsInvertedState { get; private set; }
    [Header("Ÿ�ϸ� ������Ʈ ����")]
    public TilemapRenderer normalTilemapRenderer;
    public TilemapRenderer normalHazardTileRenderer;
    public TilemapCollider2D normalTilemapCollider;
    public TilemapCollider2D normalHazardTileCollider;

    public TilemapRenderer invertedTilemapRenderer;
    public TilemapRenderer invertedHazardTileRenderer;
    public TilemapCollider2D invertedTilemapCollider;
    public TilemapCollider2D invertedHazardTileCollider;

    [Header("�Ϲ� ������Ʈ ����")]
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
        // [������ 2] ������Ʈ�� enabled �Ӽ��� ����Ͽ� �ʱ� ���� ����
        // normalTilemap.SetActive(true); -> �Ʒ��� ���� ����
        normalTilemapRenderer.enabled = true;
        normalTilemapCollider.enabled = true;
        normalHazardTileRenderer.enabled = true;
        normalHazardTileCollider.enabled = true;

        // invertedTilemap.SetActive(false); -> �Ʒ��� ���� ����
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
        // [������ 3] SetActive ��� ������Ʈ�� enabled �Ӽ��� ���� (�ξ� ������ ����)
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
