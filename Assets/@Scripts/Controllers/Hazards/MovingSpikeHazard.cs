using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingSpikeHazard : HazardBase
{
    public enum LoopMode { PingPong, Restart }

    public LoopMode loopMode = LoopMode.PingPong;
    [Tooltip("시작점을 EmptyObject로 지정")]
    public Transform pointA; // 시작점(비워두면 현재 위치)
    [Tooltip("끝점을 EmptyObject로 지정")]
    public Transform pointB; // 끝점
    public float speed = 2f;
    public float waitAtNode = 0f; // 끝점에서 잠시 대기(초)

    Vector3 _a, _b;
    float _t; // 0: _a ~ 1: _b, _t = 0.5이면 _a와 _b의 중간
    int _dir = 1; // 1: A->B, -1: B->A
    float _waitTimer;

    Rigidbody2D _rb;

    void Awake()
    {
        _a = pointA ? pointA.position : transform.position; // pointA가 null이면 → pointA.position을 쓰지 않고 here를 사용
        _b = pointB ? pointB.position : transform.position + Vector3.right * 3f; // 임시값
        RecalcEndpoints();
        _t = 0f;
    }

    void RecalcEndpoints()
    {
        var here = transform.position;
        _a = pointA ? pointA.position : here;
        _b = pointB ? pointB.position : (here + Vector3.right * 3f);
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

        if (loopMode == LoopMode.PingPong)
        {
            float dist = Vector3.Distance(_a, _b);
            if (dist < 1e-4f) return;

            _t += (_dir * speed * dt) / dist;
            _t = Mathf.Clamp01(_t);

            Vector3 next = Vector3.Lerp(_a, _b, _t);
            ApplyPosition(next);

            if (_t >= 1f || _t <= 0f)
            {
                _dir *= -1;
                if (waitAtNode > 0f) _waitTimer = waitAtNode;
            }
        }
        else if (loopMode == LoopMode.Restart)
        {
            Vector3 cur = transform.position;
            Vector3 next = Vector3.MoveTowards(cur, _b, speed * dt);
            ApplyPosition(next);

            if (next == _b)
            {
                if (waitAtNode > 0f) _waitTimer = waitAtNode;
                // 시작점으로 즉시 워프 후 다음 프레임부터 다시 B로 향함
                if (_rb != null) _rb.position = _a;
                else transform.position = _a;
            }
        }
    }

    void ApplyPosition(Vector3 pos)
    {
        if (_rb != null) _rb.MovePosition(pos);
        else transform.position = pos;
    }

    public override void OnPlayerEnter(PlayerController player)
    {
        // 사망 처리
        Debug.Log("Player Die");
    }
}
