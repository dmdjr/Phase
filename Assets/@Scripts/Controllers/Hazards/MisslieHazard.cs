using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MisslieHazard : HazardBase
{
    public float speed = 6f;
    public float rotateSpeed = 240f;
    public float lifeTime = 100f;
    public LayerMask groundLayer;

    Transform _target;
    Rigidbody2D _rb;
    float _timer;
    TimeAffected _timeAffected; // TimeAffected 컴포넌트 참조
    public Sprite brokenTile;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.gravityScale = 0f;
        _rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        _rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        _timeAffected = GetComponent<TimeAffected>(); // 컴포넌트 할당
        var player = FindFirstObjectByType<PlayerController>();
        if (player != null) _target = player.transform;
    }

    void OnEnable()
    {
        _timer = 0f;
    }

    void FixedUpdate()
    {
        _timer += Time.fixedDeltaTime;
        if (_timer >= lifeTime) { Destroy(gameObject); return; }


        // TimeAffected의 시간 배율을 적용한 현재 속도/회전속도를 계산
        float currentSpeed = speed * _timeAffected.currentTimeScale;
        float currentRotateSpeed = rotateSpeed * _timeAffected.currentTimeScale;

        if (_target == null)
        {
            _rb.velocity = transform.right * currentSpeed;
            return;
        }

        Vector2 toTarget = ((Vector2)_target.position - _rb.position).normalized;
        float rotateAmount = Vector3.Cross(toTarget, transform.right).z;

        _rb.angularVelocity = -rotateAmount * currentRotateSpeed;
        _rb.velocity = (Vector2)transform.right * currentSpeed;
    }

    public override void OnPlayerEnter(PlayerController player)
    {
        Debug.Log("Player Die");
        Destroy(gameObject);
    }

    private new void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("LockCore"))
        {
            Debug.Log("Lock core is broken");
            LockObject lockObject = other.GetComponent<LockObject>();
            lockObject.isBroken = true;
        }

        // Ground 충돌 처리
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Destroy(gameObject);
            return;
        }

        // 플레이어 충돌 처리 (부모 로직 사용)
        base.OnTriggerEnter2D(other);
    }
}


