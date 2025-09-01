using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicDirector : MonoBehaviour
{
    public PlayerController player; // �������� ������ ��Ʈ�ѷ�
    public NPCAction npc; // �������� ������ ��Ʈ�ѷ�
    private bool hasCinematicPlayed = false; // ������ ���� �� ����Ǵ� �� ���� ���� ���� ����

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

    // NPC�� ������ ������ �� ����
    public void NpcActionFinished()
    {
        // ��ų �������� Ȱ��ȭ
        if (skillUnlockItem != null)
        {
            skillUnlockItem.SetActive(true);
        }

        // �÷��̾� ���� ���¸� ����
        if (player != null)
        {
            player.isStop = false;
        }
    }
}
