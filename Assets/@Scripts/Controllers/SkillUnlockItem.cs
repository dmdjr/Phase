using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillUnlockItem : MonoBehaviour
{
    public enum UpgradeType
    {
        UnLock, // ???? ??? ???
        Upgrade_1, // ? ??¡Æ ????????
        Upgrade_2 // ?? ??¡Æ ????????
    }

    [Tooltip("??? ???????? ?? ?????? ????? ????")]
    public UpgradeType itemType = UpgradeType.UnLock;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Player?? ?úô???
        if (collision.CompareTag("Player"))
        {
            // ???? ????? CameraController ????????? ???
            CameraController cameraController = Camera.main.GetComponent<CameraController>();
            if (cameraController != null)
            {
                cameraController.RestoreCameraPosz();
            }

            // ?¡À?????? SkillController ????????? ???? ????(enable)???
            SkillController skill = collision.GetComponent<SkillController>();
            if (skill == null)
            {
                return;
            }
            switch (itemType)
            {
                case UpgradeType.UnLock:
                    skill.enabled = true;
                    GameManager.Instance.skillGrade = 1;
                    UIManager.Instance.UnlockSkill();
                    break;
                case UpgradeType.Upgrade_1:
                    GameManager.Instance.skillGrade = 2;
                    skill.releasePointMoveSpeed = 10f;
                    skill.circleShrinkSpeed = 1f;   
                    skill.circleGrowSpeed = 0.7f;
                    skill.finalDashForce = 15f;
                    skill.UpdateCircleSize(new Vector3(3f, 3f, 2f));
                    break;
                case UpgradeType.Upgrade_2:
                    GameManager.Instance.skillGrade = 3;
                    skill.releasePointMoveSpeed = 15f;
                    skill.circleShrinkSpeed = 2f;
                    skill.circleGrowSpeed = 0.5f;
                    skill.finalDashForce = 20f;
                    skill.UpdateCircleSize(new Vector3(5f, 5f, 2f));
                    break;
            }

            // ???????? ?????
            Destroy(gameObject);
        }
    }
}
