using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;

public class CooldownBar : MonoBehaviour
{
    public Transform fillBar;
    public Transform backgroundImg;
    public Enemy enemy;

    private float _t = 0; // active된 이후 흐른 시간
    private float minScaleX = 0.05f;
    private float maxScaleX = 1.45f;
    private float originScaleY = 0.15f;
    private float progress = 0f;

    private SpriteRenderer fillSpriteRenderer;
    private SpriteRenderer backgroundSpriteRenderer;

    void Awake()
    {
        enemy = gameObject.transform.parent.GetComponent<Enemy>();
        backgroundSpriteRenderer = backgroundImg.GetComponent<SpriteRenderer>();
        fillSpriteRenderer = fillBar.GetComponent<SpriteRenderer>();
    }
    void OnEnable()
    {
        _t = 0f;
        backgroundSpriteRenderer.enabled = false;
        fillSpriteRenderer.enabled = false;
    }

    public void ResetTimer()
    {
        _t = 0f;
    }

    void Update()
    {
        if ((enemy.isPlayerDetected && enemy.isShooting) || (progress > 0f && progress < 1f))
        {
            backgroundSpriteRenderer.enabled = true;
            fillSpriteRenderer.enabled = true;
            _t += Time.deltaTime;
            progress = Mathf.Clamp01(_t / enemy.shotInterval);
            fillBar.localScale = new Vector3(progress * maxScaleX, originScaleY, 0f);
        }
        else {
            backgroundSpriteRenderer.enabled = false;
            fillSpriteRenderer.enabled = false;
            fillBar.localScale = new Vector3(minScaleX, originScaleY, 0f);
            ResetTimer();
        }
    }
}
