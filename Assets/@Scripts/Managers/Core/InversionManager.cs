using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InversionManager : MonoBehaviour
{
    public static InversionManager Instance { get; private set; }

    [Header("타일맵 오브젝트 설정")]
    public GameObject normalTilemap;
    public GameObject invertedTilemap;

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

        normalTilemap.SetActive(true);
        invertedTilemap.SetActive(false);
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

        normalTilemap.SetActive(!isInverted);
        invertedTilemap.SetActive(isInverted);
        foreach (InvertibleObject obj in allInvertibleObjects)
        {
            if (obj != null)
            {
                obj.SetInvertedState(isInverted);
            }
        }
    }
}
