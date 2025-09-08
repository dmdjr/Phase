using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillController : MonoBehaviour
{

    public InversionManager inversionManager; // InversionManager를 연결할 변수

    public static bool isTimeSkillActive = false;

    [Header("[써클 오브젝트]")]
    public GameObject timeCircle;
    public CircleTimeSlip circleTimeSlip;
    public GameObject aimingCircle;
    public float circleShrinkSpeed = 0.7f;
    public float circleGrowSpeed = 1f;

    [Header("[릴리즈 포인트 오브젝트]")]
    public GameObject releasePoint;
    public Sprite invalidReleasePointSprite;
    public LayerMask tilemapLayer;
    public float releasePointCollisionRadius = 0.1f;
    public float releasePointMoveSpeed = 10f;

    // isTimeStopped를 외부에서 읽을 수 있도록 public 프로퍼티로 변경
    public bool IsSkillActive { get; private set; } = false;

    private float originalGravityScale;

    private SpriteRenderer timeCircleRenderer;
    private SpriteRenderer aimingCircleRenderer;
    public Vector3 originalCircleScale;
    //private Color circleDefaultColor = new Color(1f, 1f, 1f, 10 / 255f);
    //private Color circleAimingColor = new Color(1f, 1f, 1f, 50 / 255f);

    private Vector3 timeStopCenterPosition;
    private float maxReleaseDistance;
    private bool isCircleGrowing = false;

    private Sprite originalReleasePointSprite;
    private SpriteRenderer releasePointRenderer;
    private Vector3 lastValidReleasePosition;

    // 플레이어의 다른 컴포넌트들을 저장할 변수
    private Rigidbody2D rb;
    private Animator anim;
    private Camera mainCamera;


    // 스페이스바를 뗐을 때 추가로 나아갈 거리
    public float finalDashForce = 15f;
    void Awake()
    {
        // 이 스크립트가 붙어있는 게임 오브젝트에서 필요한 컴포넌트들을 찾아옴
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            return;
        }
        anim = GetComponent<Animator>();
        if (anim == null)
        {
            return;
        }
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            return;
        }
        originalGravityScale = rb.gravityScale;

        // 초기화 로직 (PlayerController의 Awake/Start에서 가져옴)
        if (timeCircle != null)
        {
            timeCircleRenderer = timeCircle.GetComponent<SpriteRenderer>();
            originalCircleScale = new Vector3(2.5f, 2.5f, 1f);
            timeCircle.transform.localScale = originalCircleScale;
            //timeCircleRenderer.color = circleDefaultColor;
        }
        if (aimingCircle != null)
        {
            aimingCircleRenderer = aimingCircle.GetComponent<SpriteRenderer>();
            aimingCircle.SetActive(false);
        }
        if (releasePoint != null)
        {
            releasePointRenderer = releasePoint.GetComponent<SpriteRenderer>();
            originalReleasePointSprite = releasePointRenderer.sprite;
            releasePoint.SetActive(false);
        }
    }
    private void OnEnable()
    {
        if (timeCircle != null)
        {
            timeCircle.SetActive(true);
        }
    }
    private void OnDisable()
    {
        if (timeCircle != null)
        {
            timeCircle.SetActive(false);
        }
    }
    void Update()
    {
        if (timeCircle != null)
        {
            timeCircle.transform.position = transform.position;
        }
        MovingStop();
        CircleGrowth();
    }
    void MovingStop()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isTimeSkillActive = true;
            IsSkillActive = true; // isTimeStopped 대신 사용
            isCircleGrowing = false;
            anim.SetBool("isWalking", false);
            rb.velocity = Vector2.zero;
            rb.gravityScale = 0f;
            timeStopCenterPosition = transform.position;

            maxReleaseDistance = timeCircle.transform.localScale.x * 2f;
            //float baseCircleRadius = originalCircleScale.x * 1.5f;
            //maxReleaseDistance = baseCircleRadius * timeCircle.transform.localScale.x;
            // maxReleaseDistance = baseCircleRadius * timeCircle.transform.localScale.x;


            releasePoint.SetActive(true);
            releasePoint.transform.position = transform.position;
            lastValidReleasePosition = transform.position;
            releasePointRenderer.sprite = originalReleasePointSprite;
            aimingCircle.SetActive(true);
            //aimingCircleRenderer.color = circleAimingColor;
            aimingCircle.transform.localScale = timeCircle.transform.localScale * 0.9f;

            if (inversionManager != null)
            {
                inversionManager.ToggleInversionState(true);
            }
            if (circleTimeSlip != null)
            {
                circleTimeSlip.ActivateTimeSlip();
            }
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

            // 카메라의 시야 경계 계산
            float cameraHeight = mainCamera.orthographicSize; // 메인 카메라 세로 높이의 절반
            float cameraWidth = cameraHeight * mainCamera.aspect; // 메인 카메라 가로 높이의 절반
            Vector3 cameraPosition = Camera.main.transform.position; // 현재 카메라의 위치

            float minX = cameraPosition.x - cameraWidth;
            float maxX = cameraPosition.x + cameraWidth;
            float minY = cameraPosition.y - cameraHeight;
            float maxY = cameraPosition.y + cameraHeight;

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

            // 릴리즈 포인트가 카메라 시야 내에 있는지 확인하는 상태 변수
            bool isInsideCameraView = (nextPos.x > minX && nextPos.x < maxX && nextPos.y > minY && nextPos.y < maxY);

            // 릴리즈 포인트를 움직이기 전에 타일맵인지 확인 + 카메라 시야 안에 있는지 
            if (Physics2D.OverlapCircle(nextPos, releasePointCollisionRadius, tilemapLayer) || !isInsideCameraView)
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
        if (Input.GetKeyUp(KeyCode.Space))
        {
            isTimeSkillActive = false;
            IsSkillActive = false; // isTimeStopped 대신 사용
            if (circleTimeSlip != null)
            {
                circleTimeSlip.DeactivateTimeSlip();
            }
            rb.gravityScale = originalGravityScale;
            if (aimingCircle.transform.localScale.x > 0)
            {
                // 플레이어의 현재 위치와 릴리즈포인트의 유효한 마지막 위치 사이의 거리 계산
                Vector2 dashDirection = (lastValidReleasePosition - timeStopCenterPosition).normalized;

                // 움직이지 않았다면 방향은 0
                if (dashDirection == Vector2.zero)
                {
                    dashDirection = Vector2.zero;
                }
                transform.position = lastValidReleasePosition;
                // 해당 방향으로 추가 힘 주기
                rb.velocity = dashDirection * finalDashForce;
            }
            timeCircle.transform.localScale = aimingCircle.transform.localScale;
            isCircleGrowing = true;
            releasePoint.SetActive(false);
            aimingCircle.SetActive(false);

            if (inversionManager != null)
            {
                inversionManager.ToggleInversionState(false);
            }
            if (circleTimeSlip != null)
            {
                circleTimeSlip.DeactivateTimeSlip();
            }
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

    // 외부에서 원의 크기를 즉시 변경하게 해주는 함수
    public void UpdateCircleSize(Vector3 newScale)
    {
        originalCircleScale = newScale;
        if (timeCircle != null)
        {
            timeCircle.transform.localScale = originalCircleScale;
        }
    }

    public void RevertInversion()
    {
        if (inversionManager != null)
        {
            inversionManager.ToggleInversionState(false);
        }
    }
    public void ResetSkillState()
    {
        isTimeSkillActive = false;
        IsSkillActive = false;
        if (rb != null)
        {
            rb.gravityScale = originalGravityScale;
        }
        if (aimingCircle != null)
        {
            aimingCircle.SetActive(false);
        }
        if (releasePoint != null)
        {
            releasePoint.SetActive(false);
        }
        if (timeCircle != null)
        {
            timeCircle.transform.localScale = originalCircleScale;
        }
        if (inversionManager != null)
        {
            inversionManager.ToggleInversionState(false);
        }
        if (circleTimeSlip != null)
        {
            circleTimeSlip.DeactivateTimeSlip();
        }

    }
}
