using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine;

public class TokenQueueUI : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log($"{gameObject.name}On Drop");
        if (eventData.pointerDrag != null)
        {
            //check type

            //check pos


            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                if (transform.GetChild(i).childCount != 0) continue;

                eventData.pointerDrag.transform.SetParent(transform.GetChild(i));
                eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = new Vector3(10, -10, 0);
            }


            //GetComponent<RectTransform>().anchoredPosition;

            // call tokenQueue update
        }
    }
}
