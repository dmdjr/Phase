using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAction : MonoBehaviour
{
    public GameObject dialogue1; // 첫 번째 대사
    public GameObject dialogue2; // 두 번째 대사

    public float cameraMoveDuration = 1.5f; // 카메라 이동에 걸리는 시간

    private Vector3 zoomInPosition = new Vector3(55.16f, -84.4f, -1f); // 줌인 목표 위치
    private float zoomInSize = 5f; // 줌인 목표 사이즈

    private Vector3 originalPosition = new Vector3(42.9f, -77.86f, -1f); // 원래 위치
    private float originalSize = 12f; // 원래 카메라 사이즈


    private float jumpForce = 20; // NPC 점프력
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer spriteRenderer;
    private Camera mainCamera;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        mainCamera = Camera.main;
    }

    public void StartAction(CinematicDirector director)
    {
        StartCoroutine(ActionCoroutine(director));
    }

    IEnumerator ActionCoroutine(CinematicDirector director)
    {
        yield return StartCoroutine(MoveCameraCoroutine(zoomInPosition, zoomInSize));

        yield return new WaitForSeconds(1.5f);
        spriteRenderer.flipX = false;
        yield return new WaitForSeconds(1.5f);
        dialogue1.SetActive(true);
        yield return new WaitForSeconds(2.5f);
        dialogue1.SetActive(false);
        dialogue2.SetActive(true);
        yield return new WaitForSeconds(2.5f);
        dialogue2.SetActive(false);
        if (director != null)
        {
            director.NpcActionFinished();
        }
        yield return StartCoroutine(MoveCameraCoroutine(originalPosition, originalSize));
        yield return new WaitForSeconds(0.5f);
        spriteRenderer.flipX = true;
        yield return new WaitForSeconds(0.3f);
        anim.SetTrigger("isJumping");
        rb.AddForce(new Vector2(1, 4).normalized * jumpForce, ForceMode2D.Impulse);


        yield return new WaitForSeconds(0.8f);
        gameObject.SetActive(false);

    }

    IEnumerator MoveCameraCoroutine(Vector3 targetPosition, float targetSize)
    {
        float elapsedTime = 0f;
        Vector3 startPosition = mainCamera.transform.position;
        float startSize = mainCamera.orthographicSize;

        // z좌표는 카메라의 기본 z좌표(-10)를 유지하도록 설정
        targetPosition.z = startPosition.z;

        while (elapsedTime < cameraMoveDuration)
        {
            // Lerp(선형 보간)를 사용하여 시작값과 목표값 사이를 부드럽게 이동
            mainCamera.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / cameraMoveDuration);
            mainCamera.orthographicSize = Mathf.Lerp(startSize, targetSize, elapsedTime / cameraMoveDuration);

            elapsedTime += Time.deltaTime;
            yield return null; // 다음 프레임까지 대기
        }

        // 정확한 목표 위치와 크기로 맞춰주기
        mainCamera.transform.position = targetPosition;
        mainCamera.orthographicSize = targetSize;
    }
}
