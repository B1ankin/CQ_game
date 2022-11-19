using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// Token slot that store a single token
/// 
/// </summary>
public class TokenUI : MonoBehaviour, IPointerDownHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    // Start is called before the first frame update
    private Canvas canvas;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] Token token;
    public Text tokenName;
    public Text tokenEffectsContainer;

    private RectTransform rectTransform;
    private Vector3 oldPos;

    // 用于判断token在queue位置
    public int slotIndex = -1;


    public void UpdateToken(Token token )
    {
        if (token.GetType() == typeof(ActionToken))
        {
            //GetComponent<Image>().color = Color.red;
            var ttoken = (ActionToken)token;
            string tokentext = "";
            tokentext += $"命中: {ttoken.acc}\n";
            tokentext += $"伤害: {ttoken.dmgMulti}\n";
            tokentext += $"范围类型: {ttoken.shape}\n";
            tokenEffectsContainer.text = tokentext;

        }
        else if (token.GetType() == typeof(SupportToken))
        {
            //GetComponent<Image>().color = Color.yellow;
            var ttoken = (SupportToken)token;
            tokenEffectsContainer.text = "";
            foreach (var i in ttoken.tokenEffects)
            {
                if (i.x  == 17)
                {
                    tokenEffectsContainer.text += $"前移 {i.y}\n";
                }
                else if (i.x == 9)
                {
                    tokenEffectsContainer.text += $"命中 {i.y}\n";

                }
                else if (i.x == 18)
                {
                    tokenEffectsContainer.text += $"后退 {i.y}\n";

                }
                else if (i.x == 23)
                {
                    tokenEffectsContainer.text += $"精神伤害 {i.y}\n";
                }
                else if (i.x == 48)
                {
                    tokenEffectsContainer.text += $"伤害倍率 {i.y}\n";
                }
                else if (i.x == 12)
                {
                    tokenEffectsContainer.text += $"消耗 {i.y}\n";

                }

            }

        }
        else if (token.GetType() == typeof(SpecialToken))
        {
            //GetComponent<Image>().color = Color.green;
            var ttoken = (SpecialToken)token;
            string tokentext = "";
            tokentext += $"类型: 闪避\n";
            tokentext += $"效果: 对自身释放一次闪避\n";

            tokenEffectsContainer.text = tokentext;

        }
        GetComponent<Image>().sprite = token.tokenSprite;
        this.token = token;
        tokenName.text = token.tokenName;
    }

    private void Start()
    {
        canvas = GameObject.Find("mainCanvas").GetComponent<Canvas>();
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        oldPos = new Vector3();
    }

    public void InitialToken(int tokenId)
    {
       // update token by id
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        // in pool, move to queue

        // in queue 
        if (eventData.pointerDrag.transform.IsChildOf(GameObject.Find("TokenQueueUI").transform))
        {
            Debug.Log("from queue");
        }

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //if in queue, back to inventory
        // else if in inventory, and queue is not empty -- > add to pool at end
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

    public Token GetToken()
    {
        return token;
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
