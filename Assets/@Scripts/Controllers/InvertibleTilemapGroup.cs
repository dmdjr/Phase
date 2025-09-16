using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvertibleTilemapGroup : InvertibleObject
{

    [Header("Ÿ�ϸ� �׷� ����")]
    [Tooltip("Normal World�� ���� ��� Ÿ�ϸʵ��� �θ� ������Ʈ")]
    public GameObject normalWorldTilemaps;

    [Tooltip("Inverted World�� ���� ��� Ÿ�ϸʵ��� �θ� ������Ʈ")]
    public GameObject invertedWorldTilemaps;

    public override void SetInvertedState(bool isInverted)
    {
        // Normal World Ÿ�ϸ� �׷��� �Ѱų� ��
        if (normalWorldTilemaps != null)
        {
            normalWorldTilemaps.SetActive(!isInverted);
        }

        // Inverted World Ÿ�ϸ� �׷��� �Ѱų� ��
        if (invertedWorldTilemaps != null)
        {
            invertedWorldTilemaps.SetActive(isInverted);
        }
    }

}
