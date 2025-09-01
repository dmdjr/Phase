using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EscapeTriggerController : MonoBehaviour
{
    public GameObject groundCheckObject; // �ر� �� ��Ȱ��ȭ �� ������Ʈ
    void Update()
    {
        // if (CanEnableEscapeTrigger())
        // {
        //     Collider2D collider2D = GetComponent<Collider2D>();
        //     if (collider2D != null) collider2D.isTrigger = true;
        //     TilemapRenderer tilemapRenderer = GetComponent<TilemapRenderer>();
        //     if (tilemapRenderer != null) tilemapRenderer.enabled = false;
        // }
        Collider2D collider2D = GetComponent<Collider2D>();
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (CanEnableEscapeTrigger())
        {
            if (collider2D != null) collider2D.isTrigger = true;
            if (spriteRenderer != null) spriteRenderer.enabled = false;

            // GroungCheckObject ��Ȱ��ȭ�� ��
            if (groundCheckObject != null)
            {
                groundCheckObject.SetActive(false);
            }
        }
        else
        {
            if (collider2D != null) collider2D.isTrigger = false;
            if (spriteRenderer != null) spriteRenderer.enabled = true;
            if (groundCheckObject != null)
            {
                groundCheckObject.SetActive(true);
            }
        }
    }

    bool CanEnableEscapeTrigger()
    {
        LockCore[] lockCores = FindObjectsByType<LockCore>(FindObjectsSortMode.None);
        foreach (LockCore lockObject in lockCores)
        {
            if (lockObject.isBroken == false) return false;
        }

        Key[] keys = FindObjectsByType<Key>(FindObjectsSortMode.None);
        foreach (Key key in keys)
        {
            if (key.isAcquired == false) return false;
        }

        PushLockCore[] pushLockCores = FindObjectsByType<PushLockCore>(FindObjectsSortMode.None);
        foreach (PushLockCore pushLockCore in pushLockCores)
        {
            if (pushLockCore.isPushed == false) return false;
        }

        if (lockCores.Length == 0 && keys.Length == 0 && pushLockCores.Length == 0) return true;

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
