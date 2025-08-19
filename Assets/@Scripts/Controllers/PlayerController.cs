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
    [Header("[�ٴ� üũ]")]
    public Transform groundCheck;
    public float groundRadius = 0.15f;
    public LayerMask groundLayer;

    [Header("[��Ŭ ������Ʈ]")]
    public GameObject timeCircleObject; // ���� ��
    public GameObject aimingCircleObject; // ���� ��(�Ŀ� �� ���� ������ ��)
    public GameObject releasePointObject; // �����̵��� ����(������ ����Ʈ��� ��)
    public float releasePointMoveSpeed = 5f; // ������ ����Ʈ�� �̵� �ӵ�
    public float circleShrinkSpeed = 1f; // ���� ���� ������ �ӵ�
    public float circleGrowSpeed = 1f; // ���� ���� ���� ���� �Ǵ� �ӵ�
    public float teleportRadius = 5f; // ������ ����Ʈ�� ����

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
        // �ٴ� üũ
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer) != null;
        animator.SetBool("isGrounded", isGrounded);

        if (timeCircleObject != null)
        {
            timeCircleObject.transform.position = transform.position;
        }
        // �̵� ó��
        if (!isTimeStopped)
        {
            HandleMovement();
        }

        // �����̽��� ��� ó��
        HandleTimeStop();
        HandleCircleGrowth();


        void HandleMovement()
        {
            // ���� �̵� ó��
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

            // �¿� ���� �� Walk �ִϸ��̼�
            if (horizontalInput == -1)
            {
                spriteRenderer.flipX = false;
            }
            else if (horizontalInput == 1)
            {
                spriteRenderer.flipX = true;
            }
            animator.SetBool("isWalking", horizontalInput != 0 && isGrounded);

            // ���� ó��
            if (Input.GetKeyDown(KeyCode.UpArrow) && isGrounded)
            {
                Rigidbody2D.velocity = new Vector2(Rigidbody2D.velocity.x, jumpPower);
                animator.SetTrigger("isJumping");
            }
        }
        void HandleTimeStop()
        {
            // �����̽��� ���� ���
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
                releasePointObject.transform.position = transform.position; // �÷��̾� ��ġ���� ����

                aimingCircleObject.SetActive(true);
                aimingCircleRenderer.color = circleAimingColor;
                aimingCircleObject.transform.localScale = timeCircleObject.transform.localScale * 0.9f;
            }

            if (Input.GetKey(KeyCode.Space))
            {
                // �� ��� �پ��� �ϱ�
                if (aimingCircleObject.transform.localScale.x > 0)
                {
                    aimingCircleObject.transform.localScale -= Vector3.one * circleShrinkSpeed * Time.deltaTime;
                }
                else
                {
                    aimingCircleObject.transform.localScale = Vector3.zero;
                }

                // ������ ����Ʈ �̵� (������ ����)
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
                // ���� ũ��� ���� ũ�⸦ ���Ͽ� ���� �������� ����
                if (Vector3.Distance(timeCircleObject.transform.localScale, originalCircleScale) < 0.01f)
                {
                    timeCircleObject.transform.localScale = originalCircleScale; // ��Ȯ�� ������ ������
                    isCircleGrowing = false;
                }
                else
                {
                    // Lerp�� ����Ͽ� �ε巴�� ũ�⸦ Ű��
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
        // �ε��� ������Ʈ�� �±װ� "Escape"���� Ȯ��
        if (collision.CompareTag("Escape"))
        {
            cameraController.MoveToNextStage();
        }
    }
}
