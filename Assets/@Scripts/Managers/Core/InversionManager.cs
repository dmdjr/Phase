using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InversionManager : MonoBehaviour
{

    [Header("타일맵 오브젝트 설정")]
    public GameObject normalTilemap;
    public GameObject invertedTilemap;

    [Header("일반 오브젝트 설정")]
    private List<InvertibleObject> allInvertibleObjects;

    private void Awake()
    {

        allInvertibleObjects = new List<InvertibleObject>(FindObjectsByType<InvertibleObject>(FindObjectsSortMode.None));
    }
    private void Start()
    {

        normalTilemap.SetActive(true);
        invertedTilemap.SetActive(false);
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
