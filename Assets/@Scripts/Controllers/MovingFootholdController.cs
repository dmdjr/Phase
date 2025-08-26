using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingFootholdController : MonoBehaviour
{
    // 발판의 움직임 모드 선택(좌우이동, 수직이동)
    public enum MovementType { Horizontal, Vertical }

    [Header("발판이 움직일 방향 세팅")]
    public MovementType direction = MovementType.Horizontal;

    [Header("발판의 이동속도")]
    public float speed = 2f;
    [Header("발판의 이동범위")]
    public float distance = 5f;

    private Vector3 startPoint; // 발판의 시작 지점
    private Vector3 endPoint; // 발판의 끝 지점
    private void Start()
    {
        // 시작 지점을 오브젝트의 현재 위치로 세팅
        startPoint = transform.position;
        if (direction == MovementType.Horizontal)
        {
            endPoint = startPoint + Vector3.right * distance;
        }
        else
        {
            endPoint = startPoint + Vector3.up * distance;
        }
    }
    private void Update()
    {
        float t = Mathf.PingPong(Time.time * speed, 1f);
        transform.position = Vector3.Lerp(startPoint, endPoint, t);
    }

    // 아래 두 개의 이벤트 메서드는 플레이어와 발판이 닿으면 하나로 움직이게 하는 기능
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(this.transform);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }

    // 씬 뷰에서 발판의 움직임 범위를 시각적으로 표현하기 위함
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green; 

        Vector3 start = transform.position;
        Vector3 end;

        if (direction == MovementType.Horizontal)
        {
            end = start + Vector3.right * distance;
        }
        else
        {
            end = start + Vector3.up * distance;
        }

        Gizmos.DrawLine(start, end);
    }
}
