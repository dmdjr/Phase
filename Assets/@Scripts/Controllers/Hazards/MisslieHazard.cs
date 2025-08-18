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

    public Sprite brokenTile;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.gravityScale = 0f;
        _rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        _rb.interpolation = RigidbodyInterpolation2D.Interpolate;

        var player = FindFirstObjectByType<PlayerController>();
        if (player != null) _target = player.transform;
    }

    void OnEnable()
    {
        _timer = 0f;
        if (_target != null)
        {
            Vector2 toTarget = ((Vector2)_target.position - _rb.position);
            if (toTarget.sqrMagnitude > 1e-6f)
            {
                float ang = Vector2.SignedAngle(Vector2.right, toTarget);
                _rb.SetRotation(ang);  // 초기 방향을 플레이어를 향하도록 설정
            }
        }
    }

    void FixedUpdate()
    {
        _timer += Time.fixedDeltaTime;
        if (_timer >= lifeTime) { Destroy(gameObject); return; }

        if (_target == null)
        {
            _rb.velocity = transform.right * speed;
            return;
        }

        Vector2 toTarget = ((Vector2)_target.position - _rb.position).normalized;
        float rotateAmount = Vector3.Cross(toTarget, transform.right).z;

        _rb.angularVelocity = -rotateAmount * rotateSpeed;
        _rb.velocity = (Vector2)transform.right * speed;
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


