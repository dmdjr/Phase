using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeHazard : HazardBase
{
    public override void OnPlayerEnter(PlayerController player)
    {
        // 사망 처리
        Debug.Log("Player Die");
    }
}
