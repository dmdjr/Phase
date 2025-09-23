using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EscapeTriggerController : MonoBehaviour
{
    static bool isSfxPlaying = false;

    public GameObject groundCheckObject; // �ر� �� ��Ȱ��ȭ �� ������Ʈ
    public GameObject[] fragmentPrefabs; // 파편 프리팹
    public float explosionForce = 300f; // 폭발 힘
    public float torqueAmount = 100f; // 회전 힘
    private bool hasShattered = false; // 한 번만 실행되도록 하는 상태 변수
    public AudioClip escapeClip;

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
            if (!isSfxPlaying)
            {
                SoundManager.Instance.PlaySfx(escapeClip);
                isSfxPlaying = !isSfxPlaying;
            }
            
            // 해금 조건을 만족했고, 아직 파편 효과가 실행되지 않았다면
            if (!hasShattered)
            {
                Shatter();
                hasShattered = true;
            }
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
            hasShattered = false;
        }
    }
    void Shatter()
    {
        if (fragmentPrefabs == null || fragmentPrefabs.Length == 0)
        {
            return;
        }

        foreach (GameObject fragmentPrefab in fragmentPrefabs)
        {
            GameObject frament = Instantiate(fragmentPrefab, transform.position, Quaternion.identity);
            Rigidbody2D rb = frament.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                Vector2 direction = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
                rb.AddForce(direction * explosionForce);
                float randomTorque = Random.Range(-torqueAmount, torqueAmount);
                rb.AddTorque(randomTorque);
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
