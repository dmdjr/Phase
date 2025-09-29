using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRatio : MonoBehaviour
{
    public Vector2 targetResolution = new Vector2(1920, 1080);
    private Camera _camera;

    void Awake()
    {
        _camera = GetComponent<Camera>();
        UpdateCameraRect();
    }

    void Update()
    {
        UpdateCameraRect();
    }

    public void UpdateCameraRect()
    {
        float targetAspect = targetResolution.x / targetResolution.y;

        float windowAspect = (float)Screen.width / (float)Screen.height;

        float scaleHeight = windowAspect / targetAspect;

        Rect rect = new Rect(0, 0, 1, 1);

        if (scaleHeight < 1.0f) 
        {
            rect.width = 1.0f;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1.0f - scaleHeight) / 2.0f;
        }
        else 
        {
            float scaleWidth = 1.0f / scaleHeight;
            rect.width = scaleWidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scaleWidth) / 2.0f;
            rect.y = 0;
        }

        _camera.rect = rect;
    }
}
