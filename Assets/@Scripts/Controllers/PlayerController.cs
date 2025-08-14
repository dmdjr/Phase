using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpPower = 10f;

    Rigidbody2D Rigidbody2D;
    SpriteRenderer spriteRenderer;
    [Header("[�ٴ� üũ]")]
    public Transform groundCheck;
    public float groundRadius = 0.15f;
    public LayerMask groundLayer;

    [Header("[��Ŭ ������Ʈ]")]
    public GameObject timeCircleObject;
    bool isTimeStopped = false;

    private float originalGravityScale;

    bool isGrounded;
    private CameraController cameraController;
    void Awake()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        cameraController = Camera.main.GetComponent<CameraController>();

        originalGravityScale = Rigidbody2D.gravityScale;
    }
    void Start()
    {
        if (timeCircleObject != null)
        {
            timeCircleObject.SetActive(false);
        }
    }
    void Update()
    {
        // �ٴ� üũ
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer) != null;
        // �̵� ó��
        if (!isTimeStopped)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) && isGrounded)
            {
                Rigidbody2D.velocity = new Vector2(Rigidbody2D.velocity.x, jumpPower);
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                transform.position += (Vector3)Vector2.left * moveSpeed * Time.deltaTime;
                spriteRenderer.flipX = false;
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                transform.position += (Vector3)Vector2.right * moveSpeed * Time.deltaTime;
                spriteRenderer.flipX = true;
            }

        }
        // �����̽��� ���� ���
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isTimeStopped = true;
            if (timeCircleObject != null)
            {
                timeCircleObject.SetActive(true);
                Rigidbody2D.velocity = Vector2.zero;
                Rigidbody2D.gravityScale = 0f;
            }
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            isTimeStopped = false;
            if (timeCircleObject != null)
            {
                timeCircleObject.SetActive(false);
                Rigidbody2D.gravityScale = originalGravityScale;
            }
        }


    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �ε��� ������Ʈ�� �±װ� "Escape"���� Ȯ��
        if (collision.CompareTag("Escape"))
        {
            // CameraController���� ���� ���������� �̵��϶�� ��ȣ�� ������.
            // ��� ��ġ�� �����ϴ����� CameraController�� �˾Ƽ� ó���Ѵ�.
            cameraController.MoveToNextStage();
        }
    }
}
