using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //=================�� �κ��� Stage5, 10, 15���� ��ų ������ ���� ������ ���� ����=========================
    [Header("����� ������Ʈ")]
    public Transform trans_Player;
    public Transform trans_NPC;


    public float transitionCooldown = 0.5f; // ��� �ð�
    private bool isTransitioning = false; // ���� ��ȯ ������ Ȯ���ϴ� ���� ����(��� ��ġ)

    // ī�޶� �̵��� ��� ���������� ������� ��Ƶ� ����Ʈ
    public List<Transform> cameraStages;

    // ���� ���������� �� ��°���� ����ϴ� ���� (0���� ����)
    private int currentStageIndex = 0;

    // ���� ���������� ī�޶� �̵���Ű�� �Լ�
    public void MoveToNextStage()
    {
        if (isTransitioning)
        {
            return;
        }
        isTransitioning = true;
        currentStageIndex++;

        // ���� ������ ���������� �Ѿ���� �ϸ�, ���� ������ ���� �� �̻� ���� X
        if (currentStageIndex >= cameraStages.Count)
        {
            Debug.Log("������ ����������");
            // �ε����� ����Ʈ ������ ����� �ʵ��� ������ �ε����� ����
            currentStageIndex = cameraStages.Count - 1;
            StartCoroutine(TransitionCooldown());
            return;
        }

        // ����Ʈ���� ���� ���������� ��ġ(Transform)�� ������
        Transform nextStage = cameraStages[currentStageIndex];

        // �ش� ��ġ�� ī�޶� �̵�
        // ī�޶��� z�� ��ġ�� -1 �����̴ϱ� x, y ��ġ�� �ٲ���
        transform.position = new Vector3(nextStage.position.x, nextStage.position.y, transform.position.z);

        MoveCameraPosZ();


        StartCoroutine(TransitionCooldown());
    }

    void MoveCameraPosZ()
    {
        // 5��° ���������� �̵��� �� ������ ���� �ڵ�
        if (currentStageIndex == 4)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 0f);

            if (trans_Player != null)
            {
                trans_Player.position = new Vector3(trans_Player.position.x, trans_Player.position.y, 1f);
            }
            if (trans_NPC != null)
            {
                trans_NPC.position = new Vector3(trans_NPC.position.x, trans_NPC.position.y, 1f);
            }
        }

        // 10��° ���������� �̵��� �� ����
        if (currentStageIndex == 9)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 0f);

            if (trans_Player != null)
            {
                trans_Player.position = new Vector3(trans_Player.position.x, trans_Player.position.y, 1f);
            }

        }

        // 15��° ���������� �̵��� �� ����
        if (currentStageIndex == 14)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 0f);

            if (trans_Player != null)
            {
                trans_Player.position = new Vector3(trans_Player.position.x, trans_Player.position.y, 1f);
            }
        }

    }
    public void RestoreCameraPosz()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, -1f);
        if (trans_Player != null)
        {
            trans_Player.position = new Vector3(trans_Player.position.x, trans_Player.position.y, 0f);
        }

    }
    private IEnumerator TransitionCooldown()
    {
        // transitionCooldown�� ������ �ð���ŭ ��ٸ�
        yield return new WaitForSeconds(transitionCooldown);

        // ��ٸ� �Ŀ� ����� �����Ͽ� ���� �̵��� �����ϰ� ��
        isTransitioning = false;
    }
}
