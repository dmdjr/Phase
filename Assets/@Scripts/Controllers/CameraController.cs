using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // ī�޶� �̵��� ��� ���������� ������� ��Ƶ� ����Ʈ
    public List<Transform> cameraStages;

    // ���� ���������� �� ��°���� ����ϴ� ���� (0���� ����)
    private int currentStageIndex = 0;

    // ���� ���������� ī�޶� �̵���Ű�� �Լ�
    public void MoveToNextStage()
    {
        // ���� ���������� �ε����� 1 ������ŵ�ϴ�.
        currentStageIndex++;

        // ���� ������ ���������� �Ѿ���� �ϸ�, ���� ������ ���� �� �̻� �������� �ʽ��ϴ�.
        if (currentStageIndex >= cameraStages.Count)
        {
            Debug.Log("������ ���������Դϴ�!");
            // �ε����� ����Ʈ ������ ����� �ʵ��� ������ �ε����� ����
            currentStageIndex = cameraStages.Count - 1;
            return;
        }

        // ����Ʈ���� ���� ���������� ��ġ(Transform)�� �����ɴϴ�.
        Transform nextStage = cameraStages[currentStageIndex];

        // �ش� ��ġ�� ī�޶� �̵�
        // ī�޶��� z�� ��ġ�� -1 �����̴ϱ� x, y ��ġ�� �ٲ���
        transform.position = new Vector3(nextStage.position.x, nextStage.position.y, transform.position.z);
    }
}
