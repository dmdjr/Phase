using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleTimeSlip : MonoBehaviour
{

    public float timeScaleInCircle = 0.1f;
    private float normalTimeScale = 1.0f;


    // 기준 원으로 오브젝트가 들어온 순간 호출 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // PlayerController의 스킬 활성화 스위치가 켜있다면
        if (SkillController.isTimeSkillActive)
        {
            // 들어온 오브젝트에서 TimeEffected 컴포넌트를 찾음
            TimeAffected affectedObject = collision.GetComponent<TimeAffected>();

            // 찾았다면 시간 느리게 만들기
            if (affectedObject != null)
            {
                affectedObject.UpdateTimeScale(timeScaleInCircle);
            }
        }

    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        TimeAffected affectedObject = collision.GetComponent<TimeAffected>();

        // TimeAffected 컴포넌트 없으면 종료
        if (affectedObject == null)
        {
            return; 
        }

        // 스킬 활성화 되어있을 때만 타임슬립
        if (SkillController.isTimeSkillActive)
        {
            affectedObject.UpdateTimeScale(timeScaleInCircle);
        }
        else
        {
            affectedObject.UpdateTimeScale(normalTimeScale);
        }
    }

    // 들어온 오브젝트가 나간 순간 호출
    private void OnTriggerExit2D(Collider2D collision)
    {
        TimeAffected affectedObject = collision.GetComponent<TimeAffected>();

        // 시간 복구 
        if (affectedObject != null)
        {
            // 나가는 순간에는 스킬 활성화 스위치 상태에 관계없이 무조건 시간을 되돌려야 함
            affectedObject.UpdateTimeScale(normalTimeScale);
        }
    }
}
