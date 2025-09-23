using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Button : MonoBehaviour, IPointerEnterHandler
{
    private RectTransform arrow;
    private TMP_Text buttonText;
    public TMP_Text onButtonText;
    public TMP_Text offButtonText;
    public Vector3 offset = new Vector3(-30f, 0f, 0f);

    void Awake()
    {
        arrow = GameObject.Find("Arrow").GetComponent<RectTransform>();
        if (buttonText == null)
        {
            buttonText = GetComponentInChildren<TMP_Text>();
        }
        if (onButtonText != null || offButtonText != null)
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

    public void OnClickOn()
    {
        onButtonText.color = Color.white;
        offButtonText.color = Color.gray;
    }

    public void OnClickOff()
    {
        onButtonText.color = Color.gray;
        offButtonText.color = Color.white;
    }
}
