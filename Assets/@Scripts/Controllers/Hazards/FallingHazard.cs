using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingHazard : MonoBehaviour
{
    public float loopTime;
    public float gravityScale = 3.0f;
    public bool loop = false;
    public bool isFalling = false;
    private Vector3 _startPoint;
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

    void FixedUpdate()
    {
        if (isFalling)
        {
            Vector2 gravity = Physics2D.gravity * gravityScale * _timeAffected.currentTimeScale;
            _rb.velocity += gravity * Time.fixedDeltaTime;
        }
    }

    void Init()
    {
        _rb.gravityScale = 0f;
        _rb.velocity = Vector2.zero;
        transform.position = _startPoint;
        isFalling = false;
    }

    public void OnPlayerDetected() // detect -> fall, loop afeter loopTime
    {
        _rb.gravityScale = gravityScale;
        isFalling = true;

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
