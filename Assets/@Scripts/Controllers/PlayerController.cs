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
    public bool isStop = false; // �ܺο��� �÷��̾� ���� ��� ���� ���� ����
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
    void FixedUpdate()
    {
        // �ٴ� üũ
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer) != null;
        animator.SetBool("isGrounded", isGrounded);
        if (isGrounded) // Ground와의 마찰력 표현
        {
            Rigidbody2D.velocity = Vector2.zero;
        }
        // ��ų�� ��� ���� �ƴ� ���� �÷��̾� �̵� ó�� + �ܺο��� ���� ��� ���� �ʴ� ���
        if (skillController != null && !skillController.IsSkillActive && !isStop)
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

        // new code ---
        Vector2 v = Rigidbody2D.velocity;
        if (Mathf.Abs(v.x) > Mathf.Abs(moveSpeed))
        {
            v.x = Mathf.MoveTowards(v.x, 0f, 20f * Time.fixedDeltaTime);
        }
        else
        {
            v.x = horizontalInput * moveSpeed;
        }
        Rigidbody2D.velocity = v;

        // origin ---
        // Rigidbody2D.velocity = new Vector2(horizontalInput * moveSpeed, currentYVelocity);

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

    public void Respawn(Transform respawnPoint)
    {
        // �ִϸ��̼� �߰��ϱ� 
        // ��� �ð� ������
        if (respawnPoint != null)
        {
            transform.position = respawnPoint.position;
            // �̵� �Ǵ� ���� ���� �ʱ�ȭ (�� �ϸ� �����̵� ������ �������� ���ӵ�)
        }
    }

    // player die animation�� ������ �����ӿ� �Ҵ�� �̺�Ʈ�� ȣ���ϴ� �Լ�
    /*public void OnDieAnimationEnd()
    {
        GameManager.Instance.PlayerDie(gameObject.GetComponent<PlayerController>());
        isStop = false;
        animator.SetBool("isDead", false);
    }*/

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
            /*animator.SetBool("isDead", true);
            isStop = true;*/
            /*gameObject.SetActive(false);
            GameManager.Instance.PlayerDie(gameObject.GetComponent<PlayerController>());*/
            GameManager.Instance.PlayerDie(this);
        }
    }
}
