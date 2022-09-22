using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TokenSlotUI : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log($"{gameObject.name}On Drop");
        if( eventData.pointerDrag != null ){
            eventData.pointerDrag.transform.SetParent(transform);
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = new Vector3(10, -10, 0);
                //GetComponent<RectTransform>().anchoredPosition;

            
        }
    }


}
