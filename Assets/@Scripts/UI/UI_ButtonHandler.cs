using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Button : MonoBehaviour, IPointerEnterHandler
{
    private RectTransform arrow;
    public Vector3 offset = new Vector3(-30f, 0f, 0f);
    void Awake()
    {
        arrow = GameObject.Find("Arrow").GetComponent<RectTransform>(); 
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
}
