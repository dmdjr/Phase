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
    public GameObject timeCircle; // �÷��̾ ����ٴϴ� '���� ��' 
    public GameObject aimingCircle; // �����̽��ٸ� ������ ����� '���� ��'
    public float circleShrinkSpeed = 0.7f; // ���� ���� ������ �ӵ�
    public float circleGrowSpeed = 1f; // ���� ���� ���� ���� ũ��� �����Ǵ� �ӵ�

    [Header("[������ ����Ʈ ������Ʈ")]
    public GameObject releasePoint; // �����̵� ��ǥ ������ ��Ÿ���� '������ ����Ʈ'
    public Sprite invalidReleasePointSprite; // Ÿ�ϸʿ� ����� �� ǥ��� '������ ����Ʈ' ��������Ʈ
    public LayerMask tilemapLayer; // �浹�� ������ ���̾�(Ÿ�ϸʿ� �Ҵ�� ���̾� ��)
    public float releasePointCollisionRadius = 0.1f; // ������ ����Ʈ�� �浹 ���� ����
    public float releasePointMoveSpeed = 7f; // ������ ����Ʈ�� �̵� �ӵ�
    public float teleportRadius = 5f; // ������ ����Ʈ�� ������ �� �ִ� �ִ� �ݰ�

    bool isTimeStopped = false; // �ð��� �������(��ų�� ��� ������) Ȯ���ϴ� ���� ����
    private float originalGravityScale; // ������ �߷� ���� �����ϱ� ���� ����
    bool isGrounded; // �ٴ� ���� ���� ����
    private CameraController cameraController;

    private SpriteRenderer timeCircleRenderer;
    private SpriteRenderer aimingCircleRenderer;
    private Vector3 originalCircleScale; // ���ؿ��� ũ�⸦ �����ϴ� ����
    private Color circleDefaultColor = new Color(1f, 1f, 1f, 10 / 255f); // ���� ���� ����(���� 10)
    private Color circleAimingColor = new Color(1f, 1f, 1f, 50 / 255f); // ���� ���� ����(���� 50)

    private Vector3 timeStopCenterPosition; // ��ų ��� �� ������ ����Ʈ�� �������� ����� ���� �߽��� ��ġ
    private float maxReleaseDistance; // ������ ����Ʈ�� �߽������� ��� �� �ִ� �ִ� �Ÿ�(������)
    private bool isCircleGrowing = false; // ���� ���� ���� ���� ũ��� �ٽ� Ŀ���� ������ Ȯ���ϴ� ���� ����

    private Sprite originalReleasePointSprite; // ������ ������ ����Ʈ ��������Ʈ�� ������ ����
    private SpriteRenderer releasePointRenderer; // ������ ����Ʈ�� ��������Ʈ�� �ٲٱ� ���� ����
    private Vector3 lastValidReleasePosition; // ���������� ��ȿ�ߴ�(Ÿ�ϸʿ� ���� �ʾҴ�) ������ ����Ʈ ��ġ
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
            // ���� ���� ũ�⸦ (2,2,1)�� �����Ͽ� �����Ϸ��� ����
            originalCircleScale = new Vector3(2f, 2f, 1f);
            // ���� ���� ũ�⸦ originalCircleScale�� ����
            timeCircle.transform.localScale = originalCircleScale;
        }
        if (aimingCircle != null)
        {
            aimingCircleRenderer = aimingCircle.GetComponent<SpriteRenderer>();
        }

        if (releasePoint != null)
        {
            // ���� ������ ����Ʈ�� ������ ������Ʈ �Ҵ�
            releasePointRenderer = releasePoint.GetComponent<SpriteRenderer>();
            // ���� ������ ����Ʈ�� ��������Ʈ ���� (���߿� �ǵ����� ����)
            originalReleasePointSprite = releasePointRenderer.sprite;
        }
    }
    void Start()
    {
        if (timeCircle != null)
        {
            // ���� ���� �׻� ���̵��� Ȱ��ȭ
            timeCircle.SetActive(true);
            // ���� ���� ����� ���İ��� ����
            timeCircleRenderer.color = circleDefaultColor;
        }
        if (aimingCircle != null)
        {
            // ���� ���� ���� ���� �� ������ �ʵ���
            aimingCircle.SetActive(false);
        }
        if (releasePoint != null)
        {
            // ������ ����Ʈ�� ���� ���� �� ������ �ʵ���
            releasePoint.SetActive(false);
        }

    }
    void Update()
    {
        // �ٴ� üũ
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer) != null;
        animator.SetBool("isGrounded", isGrounded);

        if (timeCircle != null)
        {
            // ���� ���� �� ������ �÷��̾��� ��ġ�� ����ٴϷ� ��
            timeCircle.transform.position = transform.position;
        }
        // isTimeStopped�� false�� ��쿡�� �÷��̾� �̵� ó��
        if (!isTimeStopped)
        {
            // �÷��̾� �̵� ó�� �Լ�
            Movment();
        }

        // �����̽��� �Է¿� ���� ��ų �Լ�
        MovingStop();
        // ���� ���� ũ�� ���� ���� �Լ�
        CircleGrowth();


        void Movment()
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

            float currentYVelocity = Rigidbody2D.velocity.y;
            Rigidbody2D.velocity = new Vector2(horizontalInput * moveSpeed, currentYVelocity);

            // �¿� ���� �� Move �ִϸ��̼�
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
            if (Input.GetKey(KeyCode.UpArrow) && isGrounded)
            {
                Rigidbody2D.velocity = new Vector2(Rigidbody2D.velocity.x, jumpPower);
                animator.SetTrigger("isJumping");
            }
        }

        void MovingStop()
        {
            // �����̽��ٸ� ������ ���� (���� 1ȸ ����)
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // ���� ����: �ð��� ���߰� ���� Ŀ���� ���� �ߴܽ�Ŵ
                isTimeStopped = true;
                isCircleGrowing = false;

                // �÷��̾� ����: �̵� �ִϸ��̼��� ���߰�, �ӵ��� �߷��� 0���� ����
                animator.SetBool("isWalking", false);
                Rigidbody2D.velocity = Vector2.zero;
                Rigidbody2D.gravityScale = 0f;

                // ����������Ʈ �̵� ���� ���� 
                timeStopCenterPosition = transform.position; // ���� �÷��̾��� ��ġ�� �̵� ������ �߽������� ����
                maxReleaseDistance = teleportRadius; // ������ ����Ʈ�� �ִ� �̵� �ݰ��� ����

                // ������ ����Ʈ�� ���� �� Ȱ��ȭ
                releasePoint.SetActive(true);
                releasePoint.transform.position = transform.position; // ������ ����Ʈ�� �÷��̾� ��ġ���� ����
                // ���� �÷��̾��� ��ġ�� ������ ��ġ�� �ʱ�ȭ
                lastValidReleasePosition = transform.position;
                // ��������Ʈ�� ������� �ǵ���
                releasePointRenderer.sprite = originalReleasePointSprite;

                aimingCircle.SetActive(true);
                aimingCircleRenderer.color = circleAimingColor; // ���� ���� ���� ����
                // ���� ���� ���� ũ�⸦ ���� ���� ���� 90%�� ����
                aimingCircle.transform.localScale = timeCircle.transform.localScale * 0.9f;
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
                // ������ ����Ʈ�� �����̱� ���� Ÿ�ϸ����� Ȯ��
                if (Physics2D.OverlapCircle(nextPos, releasePointCollisionRadius, tilemapLayer))
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


            // �����̽��� Ű�� ���� (�ּ� 1ȸ ����)
            if (Input.GetKeyUp(KeyCode.Space))
            {
                // �ð� ������ Ǯ��, ���� �߷����� ����
                isTimeStopped = false;
                Rigidbody2D.gravityScale = originalGravityScale;

                // �����̵� ����(��, ���� ���� ���� ���� ��츸) -> ���� ���� ũ�Ⱑ 0�� �Ǹ� �÷��̾�� �����̵��� �� �� ����
                if (aimingCircle.transform.localScale.x > 0)
                {
                    // �÷��̾��� ��ġ�� ������ ����Ʈ�� ���� ��ġ�� ����
                    transform.position = lastValidReleasePosition;
                }

                // ���� ���� ũ�⸦ ������ ������ ���� ���� ũ��� ����
                timeCircle.transform.localScale = aimingCircle.transform.localScale;
                // ���� ���� ���� ���� ����
                isCircleGrowing = true;

                releasePoint.SetActive(false);
                aimingCircle.SetActive(false);
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
