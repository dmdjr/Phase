using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillUnlockItem : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Player와 충돌하면
        if (collision.CompareTag("Player"))
        {
            // 메인 카메라에서 CameraController 컴포넌트를 찾음
            CameraController cameraController = Camera.main.GetComponent<CameraController>();
            if (cameraController != null)
            {
                cameraController.RestoreCameraPosz();
            }


            // 플레이어의 SkillController 컴포넌트를 찾아서 활성화(enable)시킴
            SkillController skill = collision.GetComponent<SkillController>();
            if (skill != null)
            {
                skill.enabled = true;
            }

            // 아이템은 사라짐
            Destroy(gameObject);
        }
    }
}
