using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MissileHazard : MonoBehaviour
{
    public enum ExplosionMode { None, Explosive }
    public enum GuidanceMode { Straight, Guided }
    public ExplosionMode explosionMode = ExplosionMode.None;
    public GuidanceMode guidanceMode = GuidanceMode.Guided;
    public float explosionRadius = 10f;
    public float speed = 6f;
    public float rotateSpeed = 240f;
    public float lifeTime = 100f;
    public LayerMask groundLayer;
    public Transform _target;
    Rigidbody2D _rb;
    float _timer;
    TimeAffected _timeAffected; // TimeAffected 컴포넌트 참조
    public Sprite brokenTile;

    public void Initialize(ExplosionMode mode1, GuidanceMode mode2)
    {
        explosionMode = mode1;
        guidanceMode = mode2;
    }

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


        if (guidanceMode == GuidanceMode.Straight)
        {
            _rb.angularVelocity = 0f;
            _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            _rb.velocity = (Vector2)transform.right * currentSpeed;
        }
        else if (guidanceMode == GuidanceMode.Guided)
        {
            Vector2 toTarget = ((Vector2)_target.position - _rb.position).normalized;
            float rotateAmount = Vector3.Cross(toTarget, transform.right).z;
            _rb.angularVelocity = -rotateAmount * currentRotateSpeed;
            _rb.velocity = (Vector2)transform.right * currentSpeed;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        

        if (other.CompareTag("LockCore"))
        {
            Debug.Log("Lock core is broken");
            LockCore lockObject = other.GetComponent<LockCore>();
            lockObject.isBroken = true;
        }

        // Ground 충돌 처리
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            if (explosionMode == ExplosionMode.Explosive)
            {
                Transform explosion = transform.Find("Explosion");
                if (explosion != null) explosion.gameObject.SetActive(true);
                _rb.velocity = Vector2.zero;
                _rb.angularVelocity = 0f;
                _rb.isKinematic = true;
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
                Destroy(gameObject, 0.5f); // 애니메이션 추가하면 0.5f 지연 없애기
            }
            else
            {
                Destroy(gameObject);
            }
            return;
        }

        if (other.CompareTag("Player"))
        {
            Destroy(gameObject);
            return;
        }
    }
}
