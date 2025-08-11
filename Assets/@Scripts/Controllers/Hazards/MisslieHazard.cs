using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MisslieHazard : HazardBase
{
    public float speed = 6f;
    public float rotateSpeed = 240f;
    public float lifeTime = 100f;
    public LayerMask groundLayer;

    Transform _target;
    Rigidbody2D _rb;
    float _timer;

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
