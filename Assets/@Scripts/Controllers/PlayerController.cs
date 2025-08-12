using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpPower = 10f;

    Rigidbody2D Rigidbody2D;
    SpriteRenderer spriteRenderer;
    [Header("[GroundCheck]")]
    public Transform groundCheck;
    public float groundRadius = 0.15f;
    public LayerMask groundLayer;

    bool isGrounded;

    CameraController cameraController;
    void Awake()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        cameraController = Camera.main.GetComponent<CameraController>();
    }

    void Update()
    {
        // 바닥 체크
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer) != null;
        // 이동 처리
        if (Input.GetKeyDown(KeyCode.UpArrow) && isGrounded)
        {
            Rigidbody2D.velocity = new Vector2(Rigidbody2D.velocity.x, jumpPower);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position += (Vector3)Vector2.left * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += (Vector3)Vector2.right * moveSpeed * Time.deltaTime;
        }


    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 부딪힌 오브젝트의 태그가 "Escape"인지 확인
        if (collision.CompareTag("Escape"))
        {
            // CameraController에게 다음 스테이지로 이동하라고 신호만 보냄
            // 어느 위치로 가야하는지는 CameraController가 알아서 처리
            cameraController.MoveToNextStage();
        }
    }
}
