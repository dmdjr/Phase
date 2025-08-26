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

    bool isGrounded; // �ٴ� ���� ���� ����
    private CameraController cameraController;
    private SkillController skillController; // SkillController�� ������ ����

    void Awake()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        cameraController = Camera.main.GetComponent<CameraController>();
        animator = GetComponent<Animator>();
        skillController = GetComponent<SkillController>(); // SkillController ������Ʈ�� ã�ƿ�
    }
    void Update()
    {
        // �ٴ� üũ
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer) != null;
        animator.SetBool("isGrounded", isGrounded);

        // ��ų�� ��� ���� �ƴ� ���� �÷��̾� �̵� ó��
        if (skillController != null && !skillController.IsSkillActive)
        {
            Movment();
        }
    }

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


    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �ε��� ������Ʈ�� �±װ� "Escape"���� Ȯ��
        if (collision.CompareTag("Escape"))
        {
            cameraController.MoveToNextStage();
        }
    }
}
