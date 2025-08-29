using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

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

        StartCoroutine(TransitionCooldown());
    }
    private IEnumerator TransitionCooldown()
    {
        // transitionCooldown�� ������ �ð���ŭ ��ٸ�
        yield return new WaitForSeconds(transitionCooldown);

        // ��ٸ� �Ŀ� ����� �����Ͽ� ���� �̵��� �����ϰ� ��
        isTransitioning = false;
    }
}
