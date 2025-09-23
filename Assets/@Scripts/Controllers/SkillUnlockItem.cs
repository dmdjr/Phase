using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillUnlockItem : MonoBehaviour
{
    public enum UpgradeType
    {
        UnLock, // ���� ��ų �ر�
        Upgrade_1, // ù ��° ���׷��̵�
        Upgrade_2 // �� ��° ���׷��̵�
    }

    [Tooltip("�ش� �������� � ������ �ϴ��� ����")]
    public UpgradeType itemType = UpgradeType.UnLock;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Player�� �浹�ϸ�
        if (collision.CompareTag("Player"))
        {
            // ���� ī�޶󿡼� CameraController ������Ʈ�� ã��
            CameraController cameraController = Camera.main.GetComponent<CameraController>();
            if (cameraController != null)
            {
                cameraController.RestoreCameraPosz();
            }


            // �÷��̾��� SkillController ������Ʈ�� ã�Ƽ� Ȱ��ȭ(enable)��Ŵ
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

            // �������� �����
            Destroy(gameObject);
        }
    }
}
