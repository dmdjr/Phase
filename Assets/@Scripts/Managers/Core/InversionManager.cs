using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class InversionManager : MonoBehaviour
{
    public static InversionManager Instance { get; private set; }
    public bool IsInvertedState { get; private set; }


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
       
        foreach (InvertibleObject obj in allInvertibleObjects)
        {
            if (obj != null)
            {
                obj.SetInvertedState(isInverted);
            }
        }
    }
   
    
}
