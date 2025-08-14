using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpPower = 10f;

    Rigidbody2D Rigidbody2D;
    SpriteRenderer spriteRenderer;
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
        // 스페이스바 동작 기능
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
        // 부딪힌 오브젝트의 태그가 "Escape"인지 확인
        if (collision.CompareTag("Escape"))
        {
            // CameraController에게 다음 스테이지로 이동하라고 신호만 보낸다.
            // 어느 위치로 가야하는지는 CameraController가 알아서 처리한다.
            cameraController.MoveToNextStage();
        }
    }
}
