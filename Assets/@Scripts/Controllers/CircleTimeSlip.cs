using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleTimeSlip : MonoBehaviour
{

    public float timeScaleInCircle = 0.3f;
    private float normalTimeScale = 1.0f;


    // ���� ������ ������Ʈ�� ���� ���� ȣ�� 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ���� ������Ʈ���� TimeEffected ������Ʈ�� ã��
        TimeAffected affectedObject = collision.GetComponent<TimeAffected>();


        // ã�Ҵٸ� �ð� ������ �����
        if (affectedObject != null)
        {
            affectedObject.UpdateTimeScale(timeScaleInCircle);
        }
    }


    // ���� ������Ʈ�� ���� ���� ȣ��
    private void OnTriggerExit2D(Collider2D collision)
    {
        TimeAffected affectedObject = collision.GetComponent<TimeAffected>();

        // �ð� ���� 
        if(affectedObject != null)
        {
            affectedObject.UpdateTimeScale(normalTimeScale);
        }
    }
}
