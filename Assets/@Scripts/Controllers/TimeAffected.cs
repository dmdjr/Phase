using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeAffected : MonoBehaviour
{
    public float currentTimeScale = 1.0f; // 기존의 시간 배율(정속도)
    Rigidbody2D rb;
    Animator anim;
    private float originalGravityScale;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        if (rb != null)
        {
            originalGravityScale = rb.gravityScale;
        }
    }
    /*private void Start()
    {
        if (SkillController.isTimeSkillActive)
        {
            CircleTimeSlip circle = FindAnyObjectByType<CircleTimeSlip>();
            if (circle != null)
            {
                Collider2D circleCollider = circle.GetComponent<Collider2D>();
                Collider2D ourCollider = GetComponent<Collider2D>();

                if (circleCollider != null && ourCollider != null && circleCollider.IsTouching(ourCollider))
                {
                    UpdateTimeScale(circle.timeScaleInCircle);
                }
            }
        }
    }*/
    // CircleTimeSlip 스크립트에서 호출할 함수
    public void UpdateTimeScale(float newScale)
    {
        {
            if (Mathf.Approximately(currentTimeScale, newScale)) return;

            if (rb != null && currentTimeScale != 0)
            {
                rb.velocity /= currentTimeScale;
                rb.angularVelocity /= currentTimeScale;
            }
            currentTimeScale = newScale;
            if (anim != null)
            {
                anim.speed = currentTimeScale;
            }
            if (rb != null)
            {
                rb.gravityScale = originalGravityScale * currentTimeScale;
                rb.velocity *= currentTimeScale;
                rb.angularVelocity *= currentTimeScale;
            }
        }
    }
}


