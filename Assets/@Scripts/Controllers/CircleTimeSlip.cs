using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleTimeSlip : MonoBehaviour
{

    public float timeScaleInCircle = 0.3f;
    private float normalTimeScale = 1.0f;


    // 기준 원으로 오브젝트가 들어온 순간 호출 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 들어온 오브젝트에서 TimeEffected 컴포넌트를 찾음
        TimeAffected affectedObject = collision.GetComponent<TimeAffected>();


        // 찾았다면 시간 느리게 만들기
        if (affectedObject != null)
        {
            affectedObject.UpdateTimeScale(timeScaleInCircle);
        }
    }


    // 들어온 오브젝트가 나간 순간 호출
    private void OnTriggerExit2D(Collider2D collision)
    {
        TimeAffected affectedObject = collision.GetComponent<TimeAffected>();

        // 시간 복구 
        if(affectedObject != null)
        {
            affectedObject.UpdateTimeScale(normalTimeScale);
        }
    }
}
