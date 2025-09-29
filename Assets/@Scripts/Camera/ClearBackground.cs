using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class ClearBackground : MonoBehaviour
{
    void OnPreCull()
    {
        GL.Clear(true, true, Color.black);
    }
}
