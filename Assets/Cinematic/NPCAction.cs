using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAction : MonoBehaviour
{
    public GameObject dialogue1; // ù ��° ���
    public GameObject dialogue2; // �� ��° ���

    public float cameraMoveDuration = 1.5f; // ī�޶� �̵��� �ɸ��� �ð�

    private Vector3 zoomInPosition = new Vector3(55.16f, -84.4f, -1f); // ���� ��ǥ ��ġ
    private float zoomInSize = 5f; // ���� ��ǥ ������

    private Vector3 originalPosition = new Vector3(42.9f, -77.86f, -1f); // ���� ��ġ
    private float originalSize = 12f; // ���� ī�޶� ������


    private float jumpForce = 20; // NPC ������
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

        // z��ǥ�� ī�޶��� �⺻ z��ǥ(-10)�� �����ϵ��� ����
        targetPosition.z = startPosition.z;

        while (elapsedTime < cameraMoveDuration)
        {
            // Lerp(���� ����)�� ����Ͽ� ���۰��� ��ǥ�� ���̸� �ε巴�� �̵�
            mainCamera.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / cameraMoveDuration);
            mainCamera.orthographicSize = Mathf.Lerp(startSize, targetSize, elapsedTime / cameraMoveDuration);

            elapsedTime += Time.deltaTime;
            yield return null; // ���� �����ӱ��� ���
        }

        // ��Ȯ�� ��ǥ ��ġ�� ũ��� �����ֱ�
        mainCamera.transform.position = targetPosition;
        mainCamera.orthographicSize = targetSize;
    }
}
