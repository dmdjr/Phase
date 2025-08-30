using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerCore : MonoBehaviour
{
    public enum LaserDirection { Horizontal, Vertical }

    [Header("������ ����")]
    [Tooltip("�������� �߻�� ������ ����")]
    public LaserDirection direction = LaserDirection.Vertical;

    [Tooltip("�������� �ε��� ����� �ν��� ���̾�")]
    public LayerMask obstacleLayer;

    [Tooltip("�������� �ִ�� ����� �Ÿ�")]
    public float maxDistance = 100f;

    [Header("���� ������ ����")]
    public LineRenderer beamUp;
    public LineRenderer beamDown;
    public LineRenderer beamLeft;
    public LineRenderer beamRight;

    void Update()
    {
        UpdateBeams();
    }

    void UpdateBeams()
    {
        beamUp.gameObject.SetActive(false);
        beamDown.gameObject.SetActive(false);
        beamLeft.gameObject.SetActive(false);
        beamRight.gameObject.SetActive(false);

        if (direction == LaserDirection.Vertical)
        {
            FireBeam(beamUp, Vector2.up);
            FireBeam(beamDown, Vector2.down);
        }
        else // Horizontal
        {
            FireBeam(beamLeft, Vector2.left);
            FireBeam(beamRight, Vector2.right);
        }
    }

    void FireBeam(LineRenderer beam, Vector2 fireDirection)
    {
        beam.gameObject.SetActive(true);

        Vector2 startPoint = transform.position;

        RaycastHit2D hit = Physics2D.Raycast(startPoint, fireDirection, maxDistance, obstacleLayer);

        Vector2 endPoint;

        if (hit.collider != null)
        {
            endPoint = hit.point;
        }
        else
        {
            endPoint = startPoint + fireDirection * maxDistance;
        }

        beam.SetPosition(0, startPoint);
        beam.SetPosition(1, endPoint);
    }
}
