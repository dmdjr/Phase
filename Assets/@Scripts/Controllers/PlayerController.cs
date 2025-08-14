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
        animator = GetComponent<Animator>();
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
        // 바닥 체크
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer) != null;
        // 이동 처리
        if (!isTimeStopped)
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
            transform.position += new Vector3(horizontalInput, 0, 0) * moveSpeed * Time.deltaTime;

            // 좌우 반전 및 Walk 애니메이션
            if (horizontalInput == -1)
            {
                spriteRenderer.flipX = false;
            }
            else if (horizontalInput == 1)
            {
                spriteRenderer.flipX = true; 
            }
            animator.SetBool("isWalking", horizontalInput != 0);

            // 점프 처리
            if (Input.GetKeyDown(KeyCode.UpArrow) && isGrounded)
            {
                Rigidbody2D.velocity = new Vector2(Rigidbody2D.velocity.x, jumpPower);
            }
        }
        // 스페이스바 동작 기능
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isTimeStopped = true;

            // 스페이스바 누를 시 Walk 애니메이션 강제 종료
            animator.SetBool("isWalking", false);
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
        // 부딪힌 오브젝트의 태그가 "Escape"인지 확인
        if (collision.CompareTag("Escape"))
        {
            cameraController.MoveToNextStage();
        }
    }
}
