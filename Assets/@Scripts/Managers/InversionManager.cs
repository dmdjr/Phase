using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InversionManager : MonoBehaviour
{
    [Header("ī�޶� ����")]
    private Camera mainCamera;

    [Header("Ÿ�ϸ� ������Ʈ ����")]
    public GameObject normalTilemap;
    public GameObject invertedTilemap;


    [Header("�Ϲ� ������Ʈ ����")]
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
