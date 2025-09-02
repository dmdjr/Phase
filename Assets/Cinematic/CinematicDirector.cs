using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicDirector : MonoBehaviour
{
    public PlayerController player; // 움직임을 제어할 컨트롤러
    public NPCAction npc; // 움직임을 제어할 컨트롤러
    private bool hasCinematicPlayed = false; // 연출이 여러 번 재생되는 걸 막기 위한 상태 변수

    public GameObject skillUnlockItem;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !hasCinematicPlayed)
        {
            hasCinematicPlayed = true;
            if (player != null)
            {
                player.isStop = true;
                player.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                player.GetComponent<Animator>().SetBool("isWalking", false);
            }
            if (npc != null)
            {
                npc.StartAction(this);
            }

        }
    }

    // NPC의 연출이 끝났을 때 실행
    public void NpcActionFinished()
    {
        // 스킬 아이템을 활성화
        if (skillUnlockItem != null)
        {
            skillUnlockItem.SetActive(true);
        }

        // 플레이어 정지 상태를 해제
        if (player != null)
        {
            player.isStop = false;
        }
    }
}
