using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InversionManager : MonoBehaviour
{
    [Header("타일맵 오브젝트 설정")]
    public GameObject normalTilemap;
    public GameObject invertedTilemap;

    private void Start()
    {
        normalTilemap.SetActive(true);
        invertedTilemap.SetActive(false);
    }

    [Header("일반 오브젝트 설정")]
    private List<InvertibleObject> allInvertibleObjects;

    private void Awake()
    {
        allInvertibleObjects = new List<InvertibleObject>(FindObjectsByType<InvertibleObject>(FindObjectsSortMode.None));
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            SetAllObjectsInverted(true);

        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            SetAllObjectsInverted(false);
        }
    }
    private void SetAllObjectsInverted(bool isInverted)
    {
        normalTilemap.SetActive(!isInverted);
        invertedTilemap.SetActive(isInverted);
        foreach (InvertibleObject obj in allInvertibleObjects)
        {
            obj.SetInvertedState(isInverted);
        }
    }
}
