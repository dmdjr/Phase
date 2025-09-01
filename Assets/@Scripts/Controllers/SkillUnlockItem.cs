using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillUnlockItem : MonoBehaviour
{
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
            if (skill != null)
            {
                skill.enabled = true;
            }

            // �������� �����
            Destroy(gameObject);
        }
    }
}
