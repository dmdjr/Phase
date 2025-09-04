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
    public bool isStop = false; // 외부에서 플레이어 상태 제어를 위한 상태 변수
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

        // 스킬이 사용 중이 아닐 때만 플레이어 이동 처리 + 외부에서 동작 제어를 하지 않는 경우
        if (skillController != null && !skillController.IsSkillActive && !isStop)
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

    public void Respawn(Transform respawnPoint)
    {
        // 애니메이션 추가하기 
        // 대기 시간 가지기
        if (respawnPoint != null)
        {
            transform.position = respawnPoint.position;
            // 이동 또는 점프 상태 초기화 (안 하면 순간이동 직전의 움직임이 지속됨)
        }
    }

    // player die animation의 마지막 프레임에 할당된 이벤트로 호출하는 함수
    public void OnDieAnimationEnd()
    {
        GameManager.Instance.PlayerDie(gameObject.GetComponent<PlayerController>());
        isStop = false;
        animator.SetBool("isDead", false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Escape"))
        {
            cameraController.MoveToNextStage();
        }

        if (collision.CompareTag("PushLockCore"))
        {
            PushLockCore pushLockCore = collision.GetComponent<PushLockCore>();
            pushLockCore.isPushed = true;
        }

        if (collision.CompareTag("Hazard"))
        {
            Debug.Log($"Player collide with {collision.name}");
            animator.SetBool("isDead", true);
            isStop = true;
            // GameObject effectPrefab = Resources.Load<GameObject>("Prefabs/VFX_Player_Die");
            // GameObject dieEffect = Instantiate(effectPrefab, transform.position + new Vector3(0f, 5f, 0f), transform.rotation, GetComponent<Transform>());
            // Destroy(dieEffect, 2f);
        }
    }
}
