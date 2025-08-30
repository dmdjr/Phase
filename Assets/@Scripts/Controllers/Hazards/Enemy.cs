using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 0;
    public float shotInterval = 1.5f;
    [Header("missile mode settings")]
    public MissileHazard.ExplosionMode explosionMode
        = MissileHazard.ExplosionMode.None;
    public MissileHazard.GuidanceMode guidanceMode
        = MissileHazard.GuidanceMode.Guided;
    public Transform pointA; // 시작점
    public Transform pointB; // 끝점
    public float waitAtNode = 0f;
    public CooldownBar cooldownBar;
    Coroutine loop;
   
    Vector3 _a, _b;
    float _t;
    int _dir = 1;
    float _waitTimer;

    Rigidbody2D _rb;

    public bool isPlayerDetected = false;
    private bool isShooting = false;
    private GameObject missilePrefab;
    private Transform missilePos;
    Quaternion missileRot;
    private Transform missileParent;
    
    void Awake()
    {
        missilePrefab = Resources.Load<GameObject>("Prefabs/Objects/Missile");
        if (missilePrefab == null)
        {
            Debug.LogError("Enemy can't find Missile prefab");
        }
        missilePos = transform.Find("MissilePos"); // 미사일 나오는 위치
        if (missileParent == null)
        {
            GameObject mp = new GameObject("MissileContainer");
            mp.transform.SetParent(transform.parent);
            missileParent = mp.transform;
        }
        _rb = GetComponent<Rigidbody2D>();
        RecalcEndpoints();
        _t = 0f;
    }

    void OnEnable()
    {
        isPlayerDetected = false;
    }

    void OnDisable()
    {
        if (loop != null)
            StopCoroutine(loop);
    }

    IEnumerator SpawnLoop()
    {
        // 미사일이 생성된 후 바로 타겟을 바라보기 위함
        Transform target = GameObject.Find("Player").transform;
        while (true)
        {
            yield return new WaitForSeconds(shotInterval);
            Vector2 toTarget = target.position - missilePos.position;
            float ang = Vector2.SignedAngle(Vector2.right, toTarget);
            missileRot = Quaternion.Euler(0, 0, ang);
            GameObject missile = Instantiate(missilePrefab, missilePos.position, missileRot, missileParent);
            // Missile Mode Setting
            MissileHazard mh = missile.GetComponent<MissileHazard>();
            mh.Initialize(explosionMode, guidanceMode);

            cooldownBar?.ResetTimer();
        }
    }

    void RecalcEndpoints()
    {
        var here = transform.position;
        _a = pointA ? pointA.position : here;
        _b = pointB ? pointB.position : (here + Vector3.right * 3f);
    }

    void Update()
    {
        if (missilePrefab != null && isPlayerDetected && !isShooting)
        {
            loop = StartCoroutine(SpawnLoop());
            isShooting = true;
        }
        else if (!isPlayerDetected && isShooting)
        {
            StopCoroutine(loop);
            isShooting = false;
        }
    }

    void FixedUpdate()
    {
        TickMove(Time.fixedDeltaTime);
    }

    void TickMove(float dt)
    {
        if (_waitTimer > 0f)
        {
            _waitTimer -= dt;
            return;
        }

        float dist = Vector3.Distance(_a, _b);
        if (dist < 1e-4f) return;

        _t += (_dir * speed * dt) / dist;
        _t = Mathf.Clamp01(_t);

        Vector3 next = Vector3.Lerp(_a, _b, _t);
        _rb.MovePosition(next);

        if (_t >= 1f || _t <= 0f)
        {
            _dir *= -1;
            // 오브젝트 회전 추가하기   
            if (waitAtNode > 0f) _waitTimer = waitAtNode;
        }
    }
}
