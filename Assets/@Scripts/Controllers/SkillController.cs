using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillController : MonoBehaviour
{

    public InversionManager inversionManager; // InversionManager�� ������ ����

    public static bool isTimeSkillActive = false;

    [Header("[��Ŭ ������Ʈ]")]
    public GameObject timeCircle;
    public CircleTimeSlip circleTimeSlip;
    public GameObject aimingCircle;
    public float circleShrinkSpeed = 0.7f;
    public float circleGrowSpeed = 1f;

    [Header("[������ ����Ʈ ������Ʈ]")]
    public GameObject releasePoint;
    public Sprite invalidReleasePointSprite;
    public LayerMask tilemapLayer;
    public float releasePointCollisionRadius = 0.1f;
    public float releasePointMoveSpeed = 10f;

    // isTimeStopped�� �ܺο��� ���� �� �ֵ��� public ������Ƽ�� ����
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

    // �÷��̾��� �ٸ� ������Ʈ���� ������ ����
    private Rigidbody2D rb;
    private Animator anim;
    private Camera mainCamera;


    // �����̽��ٸ� ���� �� �߰��� ���ư� �Ÿ�
    public float finalDashForce = 15f;
    void Awake()
    {
        // �� ��ũ��Ʈ�� �پ��ִ� ���� ������Ʈ���� �ʿ��� ������Ʈ���� ã�ƿ�
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

        // �ʱ�ȭ ���� (PlayerController�� Awake/Start���� ������)
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
            IsSkillActive = true; // isTimeStopped ��� ���
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
        // �����̽��ٸ� ������ �ִ� ���� (�� ������ ����)
        if (Input.GetKey(KeyCode.Space))
        {
            // ���� �� ũ�� ���̱� 
            if (aimingCircle.transform.localScale.x > 0)
            {
                // circleShrinkSpeed�� ���� �� ������ ũ�⸦ ���� �� ����
                aimingCircle.transform.localScale -= Vector3.one * circleShrinkSpeed * Time.deltaTime;
            }
            else
            {
                // ���ؿ��� ũ�Ⱑ 0���� �۾����� ũ�⸦ 0���� ����(������ ����)
                aimingCircle.transform.localScale = Vector3.zero;
            }

            // ī�޶��� �þ� ��� ���
            float cameraHeight = mainCamera.orthographicSize; // ���� ī�޶� ���� ������ ����
            float cameraWidth = cameraHeight * mainCamera.aspect; // ���� ī�޶� ���� ������ ����
            Vector3 cameraPosition = Camera.main.transform.position; // ���� ī�޶��� ��ġ

            float minX = cameraPosition.x - cameraWidth;
            float maxX = cameraPosition.x + cameraWidth;
            float minY = cameraPosition.y - cameraHeight;
            float maxY = cameraPosition.y + cameraHeight;

            // ������ ����Ʈ �̵� 
            // �̵� ������ ����� ����
            float horizontalInput = 0f;
            float verticalInput = 0f;

            // � ����Ű�� ������ �ִ����� ���� �� ����
            if (Input.GetKey(KeyCode.LeftArrow)) horizontalInput = -1f;
            else if (Input.GetKey(KeyCode.RightArrow)) horizontalInput = 1f;
            if (Input.GetKey(KeyCode.DownArrow)) verticalInput = -1f;
            else if (Input.GetKey(KeyCode.UpArrow)) verticalInput = 1f;

            // ���� �̵� ���� ��� 
            Vector3 moveInput = new Vector2(horizontalInput, verticalInput).normalized * releasePointMoveSpeed * Time.deltaTime;

            // ���� ������ ����Ʈ�� ��ġ�� �����ϴ� ����
            Vector3 nextPos = releasePoint.transform.position + moveInput;

            // ó�� ������ ����Ʈ�� �߽������� ���� ������ ����Ʈ���� �Ÿ��� �����ϴ� ����
            Vector3 offset = nextPos - timeStopCenterPosition;


            // ���� offset�� ���̰� ���� ������ �ִ� �Ÿ����� ��ٸ�
            if (offset.magnitude > maxReleaseDistance)
            {
                // ������ �״�� �ΰ� offset�� �Ÿ��� �ִ� �Ÿ���ŭ ������
                offset = offset.normalized * maxReleaseDistance;
                // ���� ������ ����Ʈ�� ��ġ�� ó�� ����������Ʈ�� �߽������� offset��ŭ ����
                nextPos = timeStopCenterPosition + offset;
            }

            // ������ ����Ʈ�� ī�޶� �þ� ���� �ִ��� Ȯ���ϴ� ���� ����
            bool isInsideCameraView = (nextPos.x > minX && nextPos.x < maxX && nextPos.y > minY && nextPos.y < maxY);

            // ������ ����Ʈ�� �����̱� ���� Ÿ�ϸ����� Ȯ�� + ī�޶� �þ� �ȿ� �ִ��� 
            if (Physics2D.OverlapCircle(nextPos, releasePointCollisionRadius, tilemapLayer) || !isInsideCameraView)
            {
                // Ÿ�ϸʿ� ��Ҵٸ� ������ ����Ʈ ��������Ʈ ���Ƴ����
                releasePointRenderer.sprite = invalidReleasePointSprite;
            }
            else
            {
                releasePointRenderer.sprite = originalReleasePointSprite;
                lastValidReleasePosition = nextPos;
            }
            // ���� ��ġ ������Ʈ�� ����
            releasePoint.transform.position = nextPos;
            aimingCircle.transform.position = releasePoint.transform.position;
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            isTimeSkillActive = false;
            IsSkillActive = false; // isTimeStopped ��� ���
            if (circleTimeSlip != null)
            {
                circleTimeSlip.DeactivateTimeSlip();
            }
            rb.gravityScale = originalGravityScale;
            if (aimingCircle.transform.localScale.x > 0)
            {
                // �÷��̾��� ���� ��ġ�� ����������Ʈ�� ��ȿ�� ������ ��ġ ������ �Ÿ� ���
                Vector2 dashDirection = (lastValidReleasePosition - timeStopCenterPosition).normalized;
                // Debug.Log("dashDirection" + dashDirection);
                // �������� �ʾҴٸ� ������ 0
                if (dashDirection == Vector2.zero)
                {
                    dashDirection = Vector2.zero;
                }
                transform.position = lastValidReleasePosition;
                // �ش� �������� �߰� �� �ֱ�
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
    // ���� ���� ���� ���� �Լ�
    void CircleGrowth()
    {
        if (isCircleGrowing)
        {
            // ���� ũ��� ���� ũ�⸦ ���Ͽ� ���� �������ٸ�
            if (Vector3.Distance(timeCircle.transform.localScale, originalCircleScale) < 0.01f)
            {
                timeCircle.transform.localScale = originalCircleScale; // ��Ȯ�� ������ ������
                isCircleGrowing = false; // �� ũ�� Ű��� ����
            }
            // ���� ũ��� ���� ũ�⸦ ���Ͽ� ���� ũ�Ⱑ ���� ũ�⺸�� �۴ٸ�
            else
            {
                // Lerp�� ����Ͽ� �ε巴�� ũ�⸦ Ű��
                timeCircle.transform.localScale = Vector3.Lerp(
                    timeCircle.transform.localScale,
                    originalCircleScale,
                    circleGrowSpeed * Time.deltaTime
                );
            }
        }
    }

    // �ܺο��� ���� ũ�⸦ ��� �����ϰ� ���ִ� �Լ�
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
