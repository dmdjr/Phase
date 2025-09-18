using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeAffected : MonoBehaviour
{
    public float currentTimeScale = 1.0f; // ������ �ð� ����(���ӵ�)
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
    // CircleTimeSlip ��ũ��Ʈ���� ȣ���� �Լ�
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


