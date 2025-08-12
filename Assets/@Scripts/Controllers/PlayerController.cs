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
        // �ٴ� üũ
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer) != null;
        // �̵� ó��
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
        // �ε��� ������Ʈ�� �±װ� "Escape"���� Ȯ��
        if (collision.CompareTag("Escape"))
        {
            // CameraController���� ���� ���������� �̵��϶�� ��ȣ�� ����
            // ��� ��ġ�� �����ϴ����� CameraController�� �˾Ƽ� ó��
            cameraController.MoveToNextStage();
        }
    }
}
