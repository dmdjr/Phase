using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingHazard : MonoBehaviour
{
    public float loopTime;
    public float gravityScale = 3.0f;
    public bool loop = false;
    private Vector3 _startPoint;
    private float _t;
    private Rigidbody2D _rb;
    private TimeAffected _timeAffected;

    void Awake()
    {
        _startPoint = transform.position;
        _rb = gameObject.GetComponent<Rigidbody2D>();
        _timeAffected = GetComponent<TimeAffected>();
    }

    void OnEnable()
    {
        Init();
    }

    void Init()
    {
        _rb.gravityScale = 0f;
        _rb.velocity = Vector2.zero;
        transform.position = _startPoint;
    }

    public void OnPlayerDetected() // 플레이어가 감지되면 추락, respawnTime 이후 제자리
    {
        _rb.gravityScale = gravityScale;
        _rb.velocity = Physics2D.gravity * Time.deltaTime * _timeAffected.currentTimeScale;

        if (loop)
        {
            StartCoroutine(LoopCoroutine());
        }     
    }

    private IEnumerator LoopCoroutine()
    {
        while (true)
        {
            float elapsed = 0f;
            while (elapsed < loopTime)
            {
                elapsed += Time.deltaTime * _timeAffected.currentTimeScale;
                yield return null;
            }
            Init();
            yield break;
        }
    }
}
