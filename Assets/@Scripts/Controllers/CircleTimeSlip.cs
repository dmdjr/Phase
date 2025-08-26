using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleTimeSlip : MonoBehaviour
{

    public float timeScaleInCircle = 0.1f;
    private float normalTimeScale = 1.0f;


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


    private void OnTriggerStay2D(Collider2D collision)
    {
        TimeAffected affectedObject = collision.GetComponent<TimeAffected>();

        // TimeAffected ������Ʈ ������ ����
        if (affectedObject == null)
        {
            return; 
        }

        // ��ų Ȱ��ȭ �Ǿ����� ���� Ÿ�ӽ���
        if (SkillController.isTimeSkillActive)
        {
            affectedObject.UpdateTimeScale(timeScaleInCircle);
        }
        else
        {
            affectedObject.UpdateTimeScale(normalTimeScale);
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
