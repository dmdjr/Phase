using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;

public class CooldownBar : MonoBehaviour
{
    public Transform fillBar;
    public Enemy enemy;

    private float _t = 0; // active된 이후 흐른 시간
    private float minScaleX = 0.05f;
    private float maxScaleX = 1.45f;
    private float originScaleY = 0.15f;

    void Awake()
    {
        enemy = gameObject.transform.parent.GetComponent<Enemy>();
    }
    void OnEnable()
    {
        _t = 0f;      
    }

    public void ResetTimer()
    {
        _t = 0f;
    }

    void Update()
    {
        if (enemy.isPlayerDetected)
        {
            _t += Time.deltaTime;
            float progress = Mathf.Clamp01(_t / enemy.shotInterval);
            fillBar.localScale = new Vector3(progress * maxScaleX, originScaleY, 0f);
        }
        else
        {
            fillBar.localScale = new Vector3(minScaleX, originScaleY, 0f);
            ResetTimer();
        }
    }
}
