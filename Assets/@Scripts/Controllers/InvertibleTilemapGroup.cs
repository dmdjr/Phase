using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvertibleTilemapGroup : InvertibleObject
{

    [Header("타일맵 그룹 설정")]
    [Tooltip("Normal World에 속한 모든 타일맵들의 부모 오브젝트")]
    public GameObject normalWorldTilemaps;

    [Tooltip("Inverted World에 속한 모든 타일맵들의 부모 오브젝트")]
    public GameObject invertedWorldTilemaps;

    public override void SetInvertedState(bool isInverted)
    {
        // Normal World 타일맵 그룹을 켜거나 끔
        if (normalWorldTilemaps != null)
        {
            normalWorldTilemaps.SetActive(!isInverted);
        }

        // Inverted World 타일맵 그룹을 켜거나 끔
        if (invertedWorldTilemaps != null)
        {
            invertedWorldTilemaps.SetActive(isInverted);
        }
    }

}
