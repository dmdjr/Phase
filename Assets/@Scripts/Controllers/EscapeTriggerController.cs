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
        if (lockObjects.Length == 0)
        {
            Debug.Log("Can't fine LockCore");
        }
        foreach (LockObject lockObject in lockObjects)
        {
            if (lockObject.isBroken == false) return false;
        }
        return true;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponent<PlayerController>();
        if (player != null)
        {
            GameManager.Instance.IncreaseStage();
        }
    }
}
