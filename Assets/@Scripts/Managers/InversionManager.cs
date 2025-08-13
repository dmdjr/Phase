using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InversionManager : MonoBehaviour
{
    [Header("카메라 설정")]
    private Camera mainCamera;

    [Header("타일맵 오브젝트 설정")]
    public GameObject normalTilemap;
    public GameObject invertedTilemap;


    [Header("일반 오브젝트 설정")]
    private List<InvertibleObject> allInvertibleObjects;

    private void Awake()
    {
        mainCamera = Camera.main;
        allInvertibleObjects = new List<InvertibleObject>(FindObjectsByType<InvertibleObject>(FindObjectsSortMode.None));
    }
    private void Start()
    {
        mainCamera.backgroundColor = Color.black;

        normalTilemap.SetActive(true);
        invertedTilemap.SetActive(false);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
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

        mainCamera.backgroundColor = isInverted ? Color.white : Color.black;
        normalTilemap.SetActive(!isInverted);
        invertedTilemap.SetActive(isInverted);
        foreach (InvertibleObject obj in allInvertibleObjects)
        {
            obj.SetInvertedState(isInverted);
        }
    }
}
