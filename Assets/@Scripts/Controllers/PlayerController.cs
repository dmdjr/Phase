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
    public GameObject timeCircleObject;
    public GameObject aimingCircleObject;
    public GameObject releasePointObject;
    public float releasePointMoveSpeed = 7f;
    public float circleShrinkSpeed = 0.5f;

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
            originalCircleScale = timeCircleObject.transform.localScale;
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

        // �̵� ó��
        if (!isTimeStopped)
        {
            HandleMovement();
        }

        // �����̽��� ��� ó��
        HandleTimeStop();



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
                animator.SetBool("isWalking", false);
                Rigidbody2D.velocity = Vector2.zero;
                Rigidbody2D.gravityScale = 0f;

                timeStopCenterPosition = transform.position;
                maxReleaseDistance = originalCircleScale.x / 2.0f;

                releasePointObject.SetActive(true);
                releasePointObject.transform.position = transform.position; // �÷��̾� ��ġ���� ����

                aimingCircleObject.SetActive(true);
                aimingCircleRenderer.color = circleAimingColor;
                aimingCircleObject.transform.localScale = originalCircleScale * 0.3f;
            }

            if (Input.GetKey(KeyCode.Space))
            {
                // �� ��� �پ��� �ϱ�
                if (aimingCircleObject.transform.localScale.x > 0)
                {
                    aimingCircleObject.transform.localScale -= Vector3.one * circleShrinkSpeed * Time.deltaTime;
                }
                // ������ ����Ʈ �̵�
                float h = Input.GetAxisRaw("Horizontal"); // A,D �Ǵ� �¿� ȭ��ǥ Ű
                float v = Input.GetAxisRaw("Vertical"); // W,S �Ǵ� ���Ʒ� ȭ��ǥ Ű
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
                // �÷��̾ ������ ����Ʈ ��ġ�� �����̵�
                if (aimingCircleObject.transform.localScale.x > 0)
                {
                    transform.position = releasePointObject.transform.position;
                }
                releasePointObject.SetActive(false);
                aimingCircleObject.SetActive(false);
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
