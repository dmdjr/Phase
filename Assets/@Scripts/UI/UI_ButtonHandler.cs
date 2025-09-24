using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ButtonHandler : MonoBehaviour, IPointerEnterHandler, IPointerUpHandler
{
    private RectTransform arrow;
    public Image onButtonTextImg;
    public Image offButtonTextImg;
    public Vector3 offset = new Vector3(-30f, 0f, 0f);

    void Awake()
    {
        arrow = GameObject.Find("Arrow").GetComponent<RectTransform>();
    
        if (onButtonTextImg != null || offButtonTextImg != null)
        {
            OnClickOn();
        }
    }

    private void Start()
    {
        arrow.gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        arrow.gameObject.SetActive(true);
        RectTransform buttonRect = eventData.pointerEnter.GetComponent<RectTransform>();
        arrow.position = buttonRect.position + offset;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        arrow.gameObject.SetActive(false);
    }

    public void OnClickOn()
    {
        onButtonTextImg.color = Color.white;
        offButtonTextImg.color = Color.gray;
    }

    public void OnClickOff()
    {
        onButtonTextImg.color = Color.gray;
        offButtonTextImg.color = Color.white;
    }
}
