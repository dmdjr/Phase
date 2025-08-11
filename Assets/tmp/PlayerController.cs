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
    void Awake()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
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

}
