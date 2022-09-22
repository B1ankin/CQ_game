using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// Token slot that store a single token
/// 
/// </summary>
public class TokenUI : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    // Start is called before the first frame update
    [SerializeField] private Canvas canvas;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] Token token;
    public Text tokenName;
    public Text tokenEffectsContainer;

    private RectTransform rectTransform;
    private Vector3 oldPos;

    public void UpdateToken(Token token )
    {
        GetComponent<Image>().color = Color.red;
        this.token = token;
        tokenName.text = token.tokenName;
        tokenEffectsContainer.text = token.ToString();
    }

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        oldPos = new Vector3();
    }

    public void InitialToken(int tokenId)
    {
       // update token by id
    }




    public void OnPointerDown(PointerEventData eventData)
    {
        //if in queue, back to inventory
        // else if in inventory, and queue is not empty -- > add to pool at end
        Debug.Log("");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.GetChild(0).gameObject.SetActive(true); // display token discription
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
        oldPos = rectTransform.anchoredPosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        // reset pos
        rectTransform.anchoredPosition = oldPos;

        Debug.Log("On End Drag");
    }

    /// <summary>
    /// loop handling ondrag events (/frame)
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta/ canvas.scaleFactor;
    }


}
