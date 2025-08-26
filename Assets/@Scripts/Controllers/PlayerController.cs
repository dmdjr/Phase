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

    bool isGrounded; // 바닥 감지 상태 변수
    private CameraController cameraController;
    private SkillController skillController; // SkillController를 제어할 변수

    void Awake()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        cameraController = Camera.main.GetComponent<CameraController>();
        animator = GetComponent<Animator>();
        skillController = GetComponent<SkillController>(); // SkillController 컴포넌트를 찾아옴
    }
    void Update()
    {
        // 바닥 체크
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer) != null;
        animator.SetBool("isGrounded", isGrounded);

        // 스킬이 사용 중이 아닐 때만 플레이어 이동 처리
        if (skillController != null && !skillController.IsSkillActive)
        {
            Movment();
        }
    }

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


    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 부딪힌 오브젝트의 태그가 "Escape"인지 확인
        if (collision.CompareTag("Escape"))
        {
            cameraController.MoveToNextStage();
        }
    }
}
