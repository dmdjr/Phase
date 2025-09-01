using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAction : MonoBehaviour
{
    public GameObject dialogue1; // 첫 번째 대사
    public GameObject dialogue2; // 두 번째 대사

    private float jumpForce = 20; // NPC 점프력
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void StartAction(CinematicDirector director)
    {
        StartCoroutine(ActionCoroutine(director));
    }

    IEnumerator ActionCoroutine(CinematicDirector director)
    {
        yield return new WaitForSeconds(1f);
        spriteRenderer.flipX = false;
        yield return new WaitForSeconds(1f);
        dialogue1.SetActive(true);
        yield return new WaitForSeconds(3f);
        dialogue1.SetActive(false);
        dialogue2.SetActive(true);
        yield return new WaitForSeconds(3f);
        dialogue2.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        spriteRenderer.flipX = true;
        yield return new WaitForSeconds(0.3f);
        anim.SetTrigger("isJumping");
        rb.AddForce(new Vector2(1, 4).normalized * jumpForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(0.7f);
        gameObject.SetActive(false);

        if (director != null)
        {
            director.NpcActionFinished();
        }

    }
}
