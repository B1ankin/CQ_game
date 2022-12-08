using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class windowPopUp : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private bool windowOpen = false;

    private void Start()
    {
        if(windowOpen) this.transform.localScale = new Vector3(1f, 1f, 1);
        else
        {
            this.transform.localScale = new Vector3(0.1f, 1f, 1);
        }
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        windowOpen = !windowOpen;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        this.transform.localScale = new Vector3(1f, 1f, 1);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(!windowOpen)
        this.transform.localScale = new Vector3(0.1f, 1f, 1);
    }
}
