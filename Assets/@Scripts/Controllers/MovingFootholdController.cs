using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingFootholdController : MonoBehaviour
{
    public enum MovementType { Horizontal, Vertical }

    [Header("발판이 움직일 방향 세팅")]
    public MovementType direction = MovementType.Horizontal;

    [Header("발판의 이동속도")]
    public float speed = 2f;
    [Header("발판의 이동범위")]
    public float distance = 5f;

    private Vector3 startPoint;
    private Vector3 endPoint;
    private void Start()
    {
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
