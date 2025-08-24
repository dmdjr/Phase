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
    public GameObject timeCircle; // 플레이어를 따라다니는 '기준 원' 
    public GameObject aimingCircle; // 스페이스바를 누르면 생기는 '조준 원'
    public float circleShrinkSpeed = 0.7f; // 조준 원이 닫히는 속도
    public float circleGrowSpeed = 1f; // 조준 원이 기준 원의 크기로 복구되는 속도

    [Header("[릴리즈 포인트 오브젝트")]
    public GameObject releasePoint; // 순간이동 목표 지점을 나타내는 '릴리즈 포인트'
    public Sprite invalidReleasePointSprite; // 타일맵에 닿았을 때 표기될 '릴리즈 포인트' 스프라이트
    public LayerMask tilemapLayer; // 충돌을 감지할 레이어(타일맵에 할당된 레이어 값)
    public float releasePointCollisionRadius = 0.1f; // 릴리즈 포인트의 충돌 판정 범위
    public float releasePointMoveSpeed = 7f; // 릴리즈 포인트의 이동 속도
    public float teleportRadius = 5f; // 릴리즈 포인트가 움직일 수 있는 최대 반경

    bool isTimeStopped = false; // 시간이 멈췄는지(스킬을 사용 중인지) 확인하는 상태 변수
    private float originalGravityScale; // 원래의 중력 값을 저장하기 위한 변수
    bool isGrounded; // 바닥 감지 상태 변수
    private CameraController cameraController;

    private SpriteRenderer timeCircleRenderer;
    private SpriteRenderer aimingCircleRenderer;
    private Vector3 originalCircleScale; // 기준원의 크기를 저장하는 변수
    private Color circleDefaultColor = new Color(1f, 1f, 1f, 10 / 255f); // 기준 원의 색상(투명도 10)
    private Color circleAimingColor = new Color(1f, 1f, 1f, 50 / 255f); // 조준 원의 색상(투명도 50)

    private Vector3 timeStopCenterPosition; // 스킬 사용 시 릴리즈 포인트를 기준으로 생기는 원의 중심점 위치
    private float maxReleaseDistance; // 릴리즈 포인트가 중심점에서 벗어날 수 있는 최대 거리(반지름)
    private bool isCircleGrowing = false; // 조준 원이 기준 원의 크기로 다시 커지는 중인지 확인하는 상태 변수

    private Sprite originalReleasePointSprite; // 원래의 릴리즈 포인트 스프라이트를 저장할 변수
    private SpriteRenderer releasePointRenderer; // 릴리즈 포인트의 스프라이트를 바꾸기 위한 변수
    private Vector3 lastValidReleasePosition; // 마지막으로 유효했던(타일맵에 닿지 않았던) 릴리즈 포인트 위치
    void Awake()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        cameraController = Camera.main.GetComponent<CameraController>();
        animator = GetComponent<Animator>();
        originalGravityScale = Rigidbody2D.gravityScale;


        if (timeCircle != null)
        {
            timeCircleRenderer = timeCircle.GetComponent<SpriteRenderer>();
            // 기준 원의 크기를 (2,2,1)로 고정하여 저장하려는 변수
            originalCircleScale = new Vector3(2f, 2f, 1f);
            // 기준 원의 크기를 originalCircleScale로 저장
            timeCircle.transform.localScale = originalCircleScale;
        }
        if (aimingCircle != null)
        {
            aimingCircleRenderer = aimingCircle.GetComponent<SpriteRenderer>();
        }

        if (releasePoint != null)
        {
            // 기존 릴리즈 포인트의 렌더러 컴포넌트 할당
            releasePointRenderer = releasePoint.GetComponent<SpriteRenderer>();
            // 기존 릴리즈 포인트의 스프라이트 저장 (나중에 되돌리기 위해)
            originalReleasePointSprite = releasePointRenderer.sprite;
        }
    }
    void Start()
    {
        if (timeCircle != null)
        {
            // 기준 원은 항상 보이도록 활성화
            timeCircle.SetActive(true);
            // 기준 원의 색상과 알파값을 설정
            timeCircleRenderer.color = circleDefaultColor;
        }
        if (aimingCircle != null)
        {
            // 조준 원은 게임 시작 시 보이지 않도록
            aimingCircle.SetActive(false);
        }
        if (releasePoint != null)
        {
            // 릴리즈 포인트도 게임 시작 시 보이지 않도록
            releasePoint.SetActive(false);
        }

    }
    void Update()
    {
        // 바닥 체크
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer) != null;
        animator.SetBool("isGrounded", isGrounded);

        if (timeCircle != null)
        {
            // 기준 원이 매 프레임 플레이어의 위치를 따라다니록 함
            timeCircle.transform.position = transform.position;
        }
        // isTimeStopped가 false일 경우에만 플레이어 이동 처리
        if (!isTimeStopped)
        {
            // 플레이어 이동 처리 함수
            Movment();
        }

        // 스페이스바 입력에 따른 스킬 함수
        MovingStop();
        // 조준 원의 크기 복구 관련 함수
        CircleGrowth();


        void Movment()
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

            float currentYVelocity = Rigidbody2D.velocity.y;
            Rigidbody2D.velocity = new Vector2(horizontalInput * moveSpeed, currentYVelocity);

            // 좌우 반전 및 Move 애니메이션
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
            if (Input.GetKey(KeyCode.UpArrow) && isGrounded)
            {
                Rigidbody2D.velocity = new Vector2(Rigidbody2D.velocity.x, jumpPower);
                animator.SetTrigger("isJumping");
            }
        }

        void MovingStop()
        {
            // 스페이스바를 누르는 순간 (최초 1회 실행)
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // 상태 변경: 시간을 멈추고 원이 커지는 것을 중단시킴
                isTimeStopped = true;
                isCircleGrowing = false;

                // 플레이어 정지: 이동 애니메이션을 멈추고, 속도와 중력을 0으로 만듦
                animator.SetBool("isWalking", false);
                Rigidbody2D.velocity = Vector2.zero;
                Rigidbody2D.gravityScale = 0f;

                // 릴리즈포인트 이동 범위 설정 
                timeStopCenterPosition = transform.position; // 현재 플레이어의 위치를 이동 범위의 중심점으로 설정
                maxReleaseDistance = teleportRadius; // 릴리즈 포인트의 최대 이동 반경을 설정

                // 릴리즈 포인트와 조준 원 활성화
                releasePoint.SetActive(true);
                releasePoint.transform.position = transform.position; // 릴리즈 포인트를 플레이어 위치에서 생성
                // 현재 플레이어의 위치를 마지막 위치로 초기화
                lastValidReleasePosition = transform.position;
                // 스프라이트를 원래대로 되돌림
                releasePointRenderer.sprite = originalReleasePointSprite;

                aimingCircle.SetActive(true);
                aimingCircleRenderer.color = circleAimingColor; // 조준 원의 색상 설정
                // 조준 원의 시작 크기를 현재 기준 원의 90%로 설정
                aimingCircle.transform.localScale = timeCircle.transform.localScale * 0.9f;
            }


            // 스페이스바를 누르고 있는 동안 (매 프레임 실행)
            if (Input.GetKey(KeyCode.Space))
            {
                // 조준 원 크기 줄이기 
                if (aimingCircle.transform.localScale.x > 0)
                {
                    // circleShrinkSpeed에 따라 매 프레임 크기를 조금 씩 줄임
                    aimingCircle.transform.localScale -= Vector3.one * circleShrinkSpeed * Time.deltaTime;
                }
                else
                {
                    // 조준원의 크기가 0보다 작아지면 크기를 0으로 고정(음수값 방지)
                    aimingCircle.transform.localScale = Vector3.zero;
                }

                // 릴리즈 포인트 이동 
                // 이동 방향을 계산할 변수
                float horizontalInput = 0f;
                float verticalInput = 0f;

                // 어떤 방향키를 누르고 있는지에 따라 값 저장
                if (Input.GetKey(KeyCode.LeftArrow)) horizontalInput = -1f;
                else if (Input.GetKey(KeyCode.RightArrow)) horizontalInput = 1f;
                if (Input.GetKey(KeyCode.DownArrow)) verticalInput = -1f;
                else if (Input.GetKey(KeyCode.UpArrow)) verticalInput = 1f;

                // 최종 이동 벡터 계산 
                Vector3 moveInput = new Vector2(horizontalInput, verticalInput).normalized * releasePointMoveSpeed * Time.deltaTime;

                // 다음 릴리즈 포인트의 위치를 저장하는 변수
                Vector3 nextPos = releasePoint.transform.position + moveInput;

                // 처음 릴리즈 포인트의 중심점에서 다음 릴리즈 포인트까지 거리를 저장하는 변수
                Vector3 offset = nextPos - timeStopCenterPosition;


                // 만약 offset의 길이가 내가 지정한 최대 거리보다 길다면
                if (offset.magnitude > maxReleaseDistance)
                {
                    // 방향은 그대로 두고 offset의 거리를 최대 거리만큼 제한함
                    offset = offset.normalized * maxReleaseDistance;
                    // 다음 릴리즈 포인트의 위치를 처음 릴리즈포인트의 중심점에서 offset만큼 더함
                    nextPos = timeStopCenterPosition + offset;
                }
                // 릴리즈 포인트를 움직이기 전에 타일맵인지 확인
                if (Physics2D.OverlapCircle(nextPos, releasePointCollisionRadius, tilemapLayer))
                {
                    // 타일맵에 닿았다면 릴리즈 포인트 스프라이트 갈아끼우기
                    releasePointRenderer.sprite = invalidReleasePointSprite;
                }
                else
                {
                    releasePointRenderer.sprite = originalReleasePointSprite;
                    lastValidReleasePosition = nextPos;
                }
                // 최종 위치 오브젝트에 적용
                releasePoint.transform.position = nextPos;
                aimingCircle.transform.position = releasePoint.transform.position;
            }


            // 스페이스바 키를 떼면 (최소 1회 실행)
            if (Input.GetKeyUp(KeyCode.Space))
            {
                // 시간 정지를 풀고, 원래 중력으로 복구
                isTimeStopped = false;
                Rigidbody2D.gravityScale = originalGravityScale;

                // 순간이동 실행(단, 조준 원이 남아 있을 경우만) -> 조준 원의 크기가 0이 되면 플레이어는 순간이동을 할 수 없음
                if (aimingCircle.transform.localScale.x > 0)
                {
                    // 플레이어의 위치를 릴리즈 포인트의 최종 위치로 변경
                    transform.position = lastValidReleasePosition;
                }

                // 기준 원의 크기를 마지막 순간의 조준 원의 크기로 변경
                timeCircle.transform.localScale = aimingCircle.transform.localScale;
                // 기준 원의 상태 복구 시작
                isCircleGrowing = true;

                releasePoint.SetActive(false);
                aimingCircle.SetActive(false);
            }
        }

        // 기준 원의 상태 복구 함수
        void CircleGrowth()
        {
            if (isCircleGrowing)
            {
                // 현재 크기와 원래 크기를 비교하여 거의 같아졌다면
                if (Vector3.Distance(timeCircle.transform.localScale, originalCircleScale) < 0.01f)
                {
                    timeCircle.transform.localScale = originalCircleScale; // 정확한 값으로 맞춰줌
                    isCircleGrowing = false; // 원 크기 키우기 종료
                }
                // 현재 크기와 원래 크기를 비교하여 현재 크기가 원래 크기보다 작다면
                else
                {
                    // Lerp를 사용하여 부드럽게 크기를 키움
                    timeCircle.transform.localScale = Vector3.Lerp(
                        timeCircle.transform.localScale,
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
