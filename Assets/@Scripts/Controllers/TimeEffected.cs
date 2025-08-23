using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeEffected : MonoBehaviour
{
    public float currentTimeScale = 1.0f; // 기존의 시간 배율(정속도)
    Rigidbody2D rb; 
    Animator anim;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // CircleTimeSlip 스크립트에서 호출할 함수
    public void UpdateTimeScale(float newScale)
    {
        // newScale은 시간 배율 조정될 매개변수
        currentTimeScale = newScale; 

        if (anim != null)
        {
            anim.speed = currentTimeScale;
        }

        if (rb != null)
        {
            // (rb.velocity.magnitude > 0 ? rb.velocity.magnitude : 1) 이 부분은 속도가 0일 경우 발생할 오류 방지
            rb.velocity *= currentTimeScale / (rb.velocity.magnitude > 0 ? rb.velocity.magnitude : 1);
            rb.angularVelocity *= currentTimeScale;
            rb.gravityScale *= currentTimeScale;
        }
    }
}
