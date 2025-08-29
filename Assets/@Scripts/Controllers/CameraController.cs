using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public float transitionCooldown = 0.5f; // 대기 시간
    private bool isTransitioning = false; // 현재 전환 중인지 확인하는 상태 변수(잠금 장치)

    // 카메라가 이동할 모든 스테이지를 순서대로 담아둘 리스트
    public List<Transform> cameraStages;

    // 현재 스테이지가 몇 번째인지 기억하는 변수 (0부터 시작)
    private int currentStageIndex = 0;

    // 다음 스테이지로 카메라를 이동시키는 함수
    public void MoveToNextStage()
    {
        if (isTransitioning)
        {
            return;
        }
        isTransitioning = true;
        currentStageIndex++;

        // 만약 마지막 스테이지를 넘어서려고 하면, 오류 방지를 위해 더 이상 진행 X
        if (currentStageIndex >= cameraStages.Count)
        {
            Debug.Log("마지막 스테이지임");
            // 인덱스가 리스트 범위를 벗어나지 않도록 마지막 인덱스로 고정
            currentStageIndex = cameraStages.Count - 1;
            StartCoroutine(TransitionCooldown());
            return;
        }

        // 리스트에서 다음 스테이지의 위치(Transform)를 가져옴
        Transform nextStage = cameraStages[currentStageIndex];

        // 해당 위치로 카메라를 이동
        // 카메라의 z축 위치는 -1 고정이니까 x, y 위치만 바꿔줌
        transform.position = new Vector3(nextStage.position.x, nextStage.position.y, transform.position.z);

        StartCoroutine(TransitionCooldown());
    }
    private IEnumerator TransitionCooldown()
    {
        // transitionCooldown에 설정된 시간만큼 기다림
        yield return new WaitForSeconds(transitionCooldown);

        // 기다린 후에 잠금을 해제하여 다음 이동이 가능하게 함
        isTransitioning = false;
    }
}
