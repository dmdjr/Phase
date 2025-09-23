using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillUnlockItem : MonoBehaviour
{
    public enum UpgradeType
    {
        UnLock, // 최초 스킬 해금
        Upgrade_1, // 첫 번째 업그레이드
        Upgrade_2 // 두 번째 업그레이드
    }

    [Tooltip("해당 아이템이 어떤 역할을 하는지 선택")]
    public UpgradeType itemType = UpgradeType.UnLock;
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
            if (skill == null)
            {
                return;
            }
            switch (itemType)
            {
                case UpgradeType.UnLock:
                    skill.enabled = true;
                    UIManager.Instance.UnlockSkill();
                    break;
                case UpgradeType.Upgrade_1:
                    skill.releasePointMoveSpeed = 15f;
                    skill.circleShrinkSpeed = 1f;
                    skill.circleGrowSpeed = 0.7f;
                    skill.finalDashForce = 20f;
                    skill.UpdateCircleSize(new Vector3(5f, 5f, 1f));
                    break;
                case UpgradeType.Upgrade_2:
                    skill.releasePointMoveSpeed = 30f;
                    skill.circleShrinkSpeed = 2f;
                    skill.circleGrowSpeed = 0.5f;
                    skill.finalDashForce = 30f;
                    skill.UpdateCircleSize(new Vector3(7f, 7f, 1f));
                    break;
            }

            // 아이템은 사라짐
            Destroy(gameObject);
        }
    }
}
