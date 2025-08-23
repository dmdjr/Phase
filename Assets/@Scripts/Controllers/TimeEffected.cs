using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeEffected : MonoBehaviour
{
    public float currentTimeScale = 1.0f; // ������ �ð� ����(���ӵ�)
    Rigidbody2D rb; 
    Animator anim;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // CircleTimeSlip ��ũ��Ʈ���� ȣ���� �Լ�
    public void UpdateTimeScale(float newScale)
    {
        // newScale�� �ð� ���� ������ �Ű�����
        currentTimeScale = newScale; 

        if (anim != null)
        {
            anim.speed = currentTimeScale;
        }

        if (rb != null)
        {
            // (rb.velocity.magnitude > 0 ? rb.velocity.magnitude : 1) �� �κ��� �ӵ��� 0�� ��� �߻��� ���� ����
            rb.velocity *= currentTimeScale / (rb.velocity.magnitude > 0 ? rb.velocity.magnitude : 1);
            rb.angularVelocity *= currentTimeScale;
            rb.gravityScale *= currentTimeScale;
        }
    }
}
