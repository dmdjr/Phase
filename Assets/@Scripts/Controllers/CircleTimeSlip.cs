using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleTimeSlip : MonoBehaviour
{

    public float timeScaleInCircle = 0.1f;
    private float normalTimeScale = 1.0f;
    private Collider2D circleCollider;
    private void Awake()
    {
        circleCollider = GetComponent<Collider2D>();
    }
    public void ActivateTimeSlip()
    {
        // ���� ��ġ�� ũ��(������)�� �������� ��ġ�� ��� �ݶ��̴��� ã�� �迭�� ��ȯ�մϴ�.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, circleCollider.bounds.extents.x);

        foreach (Collider2D hit in colliders)
        {
            TimeAffected affectedObject = hit.GetComponent<TimeAffected>();
            if (affectedObject != null)
            {
                affectedObject.UpdateTimeScale(timeScaleInCircle);
            }
        }
    }
    public void DeactivateTimeSlip()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, circleCollider.bounds.extents.x);

        foreach (Collider2D hit in colliders)
        {
            TimeAffected affectedObject = hit.GetComponent<TimeAffected>();
            if (affectedObject != null)
            {
                affectedObject.UpdateTimeScale(normalTimeScale);
            }
        }
    }
    // ���� ������ ������Ʈ�� ���� ���� ȣ�� 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // PlayerController�� ��ų Ȱ��ȭ ����ġ�� ���ִٸ�
        if (SkillController.isTimeSkillActive)
        {
            // ���� ������Ʈ���� TimeEffected ������Ʈ�� ã��
            TimeAffected affectedObject = collision.GetComponent<TimeAffected>();

            // ã�Ҵٸ� �ð� ������ �����
            if (affectedObject != null)
            {
                affectedObject.UpdateTimeScale(timeScaleInCircle);
            }
        }

    }



    // ���� ������Ʈ�� ���� ���� ȣ��
    private void OnTriggerExit2D(Collider2D collision)
    {
        TimeAffected affectedObject = collision.GetComponent<TimeAffected>();

        // �ð� ���� 
        if (affectedObject != null)
        {
            // ������ �������� ��ų Ȱ��ȭ ����ġ ���¿� ������� ������ �ð��� �ǵ����� ��
            affectedObject.UpdateTimeScale(normalTimeScale);
        }
    }
}
