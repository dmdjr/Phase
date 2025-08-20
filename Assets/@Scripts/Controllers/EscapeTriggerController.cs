using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EscapeTriggerController : MonoBehaviour
{
    void Update()
    {
        if (CanEnableEscapeTrigger())
        {
            Collider2D collider2D = GetComponent<Collider2D>();
            collider2D.isTrigger = true;
            TilemapRenderer tilemapRenderer = GetComponent<TilemapRenderer>();
            tilemapRenderer.enabled = false;
        }
    }

    bool CanEnableEscapeTrigger()
    {
        LockObject[] lockObjects = FindObjectsByType<LockObject>(FindObjectsSortMode.None);
        foreach (LockObject lockObject in lockObjects)
        {
            if (lockObject.isBroken == false) return false;
        }
        return true;
    }
}
