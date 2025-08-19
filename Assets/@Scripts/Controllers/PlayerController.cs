using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpPower = 10f;

    Rigidbody2D Rigidbody2D;
    SpriteRenderer spriteRenderer;
    Animator animator;
    [Header("[바닥 체크]")]
    public Transform groundCheck;
    public float groundRadius = 0.15f;
    public LayerMask groundLayer;

    [Header("[써클 오브젝트]")]
    public GameObject timeCircleObject; // 기준 원
    public GameObject aimingCircleObject; // 다음 원(후에 이 원이 기준이 됨)
    public GameObject releasePointObject; // 순간이동할 지점(릴리즈 포인트라고 함)
    public float releasePointMoveSpeed = 5f; // 릴리즈 포인트의 이동 속도
    public float circleShrinkSpeed = 1f; // 다음 원이 닫히는 속도
    public float circleGrowSpeed = 1f; // 다음 원이 기준 원이 되는 속도
    public float teleportRadius = 5f; // 릴리즈 포인트의 범위

    bool isTimeStopped = false;
    private float originalGravityScale;
    bool isGrounded;
    private CameraController cameraController;

    private SpriteRenderer timeCircleRenderer;
    private SpriteRenderer aimingCircleRenderer;
    private Vector3 originalCircleScale;
    private Color circleDefaultColor = new Color(1f, 1f, 1f, 10 / 255f);
    private Color circleAimingColor = new Color(1f, 1f, 1f, 50 / 255f);

    private Vector3 timeStopCenterPosition;
    private float maxReleaseDistance;
    private bool isCircleGrowing = false;
    void Awake()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        cameraController = Camera.main.GetComponent<CameraController>();
        animator = GetComponent<Animator>();
        originalGravityScale = Rigidbody2D.gravityScale;


        if (timeCircleObject != null)
        {
            timeCircleRenderer = timeCircleObject.GetComponent<SpriteRenderer>();
            originalCircleScale = new Vector3(2f, 2f, 1f);
            timeCircleObject.transform.localScale = originalCircleScale;
        }
        if (aimingCircleObject != null)
        {
            aimingCircleRenderer = aimingCircleObject.GetComponent<SpriteRenderer>();
        }
    }
    void Start()
    {
        if (timeCircleObject != null)
        {
            timeCircleObject.SetActive(true);
            timeCircleRenderer.color = circleDefaultColor;
        }
        if (aimingCircleObject != null)
        {
            aimingCircleObject.SetActive(false);
        }
        if (releasePointObject != null)
        {
            releasePointObject.SetActive(false);
        }

    }
    void Update()
    {
        // 바닥 체크
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer) != null;
        animator.SetBool("isGrounded", isGrounded);

        if (timeCircleObject != null)
        {
            timeCircleObject.transform.position = transform.position;
        }
        // 이동 처리
        if (!isTimeStopped)
        {
            HandleMovement();
        }

        // 스페이스바 기능 처리
        HandleTimeStop();
        HandleCircleGrowth();


        void HandleMovement()
        {
            // 수평 이동 처리
            float horizontalInput = 0;

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                horizontalInput = -1f;
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                horizontalInput = 1f;
            }
            transform.position += new Vector3(horizontalInput, 0, 0) * moveSpeed * Time.deltaTime;

            // 좌우 반전 및 Walk 애니메이션
            if (horizontalInput == -1)
            {
                spriteRenderer.flipX = false;
            }
            else if (horizontalInput == 1)
            {
                spriteRenderer.flipX = true;
            }
            animator.SetBool("isWalking", horizontalInput != 0 && isGrounded);

            // 점프 처리
            if (Input.GetKeyDown(KeyCode.UpArrow) && isGrounded)
            {
                Rigidbody2D.velocity = new Vector2(Rigidbody2D.velocity.x, jumpPower);
                animator.SetTrigger("isJumping");
            }
        }
        void HandleTimeStop()
        {
            // 스페이스바 동작 기능
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isTimeStopped = true;
                isCircleGrowing = false;
                animator.SetBool("isWalking", false);
                Rigidbody2D.velocity = Vector2.zero;
                Rigidbody2D.gravityScale = 0f;

                timeStopCenterPosition = transform.position;
                maxReleaseDistance = teleportRadius;
                releasePointObject.SetActive(true);
                releasePointObject.transform.position = transform.position; // 플레이어 위치에서 생성

                aimingCircleObject.SetActive(true);
                aimingCircleRenderer.color = circleAimingColor;
                aimingCircleObject.transform.localScale = timeCircleObject.transform.localScale * 0.9f;
            }

            if (Input.GetKey(KeyCode.Space))
            {
                // 원 계속 줄어들게 하기
                if (aimingCircleObject.transform.localScale.x > 0)
                {
                    aimingCircleObject.transform.localScale -= Vector3.one * circleShrinkSpeed * Time.deltaTime;
                }
                else
                {
                    aimingCircleObject.transform.localScale = Vector3.zero;
                }

                // 릴리스 포인트 이동 (기존과 동일)
                float h = Input.GetAxisRaw("Horizontal");
                float v = Input.GetAxisRaw("Vertical");
                Vector3 moveInput = new Vector2(h, v) * releasePointMoveSpeed * Time.deltaTime;

                Vector3 nextPos = releasePointObject.transform.position + moveInput;
                Vector3 offset = nextPos - timeStopCenterPosition;

                if (offset.magnitude > maxReleaseDistance)
                {
                    offset = offset.normalized * maxReleaseDistance;
                    nextPos = timeStopCenterPosition + offset;
                }
                releasePointObject.transform.position = nextPos;
                aimingCircleObject.transform.position = releasePointObject.transform.position;
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                isTimeStopped = false;
                Rigidbody2D.gravityScale = originalGravityScale;

                if (aimingCircleObject.transform.localScale.x > 0)
                {
                    transform.position = releasePointObject.transform.position;
                }

                timeCircleObject.transform.localScale = aimingCircleObject.transform.localScale;
                isCircleGrowing = true;

                releasePointObject.SetActive(false);
                aimingCircleObject.SetActive(false);
            }
        }
        void HandleCircleGrowth()
        {
            if (isCircleGrowing)
            {
                // 현재 크기와 원래 크기를 비교하여 거의 같아지면 멈춤
                if (Vector3.Distance(timeCircleObject.transform.localScale, originalCircleScale) < 0.01f)
                {
                    timeCircleObject.transform.localScale = originalCircleScale; // 정확한 값으로 맞춰줌
                    isCircleGrowing = false;
                }
                else
                {
                    // Lerp를 사용하여 부드럽게 크기를 키움
                    timeCircleObject.transform.localScale = Vector3.Lerp(
                        timeCircleObject.transform.localScale,
                        originalCircleScale,
                        circleGrowSpeed * Time.deltaTime
                    );
                }
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 부딪힌 오브젝트의 태그가 "Escape"인지 확인
        if (collision.CompareTag("Escape"))
        {
            cameraController.MoveToNextStage();
        }
    }
}
