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
        if (eventData.pointerDrag == null)
        {
            return;
        }

        // in queue 
        if (eventData.pointerDrag.transform.IsChildOf(GameObject.Find("TokenQueueUI").transform))
        {
            Debug.Log("from queue");
            //check type
            var temptype = eventData.pointerDrag.GetComponent<TokenUI>().GetToken().GetType();
            Debug.Log(temptype);
            GameObject tokenPool = GameObject.Find("TokenPoolUI");

            GameObject.Find("TokenQueueUI").GetComponent<TokenQueueUI>().
            RemoveTokenFromQueue(eventData.pointerDrag.GetComponent<TokenUI>().slotIndex);

            if (temptype == typeof(ActionToken))
            {
                Transform actionT = tokenPool.transform.GetChild(0);
                for (int i = actionT.childCount - 1; i >= 0; i--)
                {
                    if (actionT.GetChild(i).childCount != 0) continue;

                    eventData.pointerDrag.transform.SetParent(actionT.GetChild(i));
                    eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
                }
            }
            else if (temptype == typeof(SupportToken))
            {
                Transform supportT = tokenPool.transform.GetChild(1);

                for (int i = supportT.childCount - 1; i >= 0; i--)
                {
                    if (supportT.GetChild(i).childCount != 0) continue;

                    eventData.pointerDrag.transform.SetParent(supportT.GetChild(i));
                    eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
                }
            }
            else if (temptype == typeof(SpecialToken))
            {
                Transform specialT = tokenPool.transform.GetChild(2);

                for (int i = specialT.childCount - 1; i >= 0; i--)
                {
                    if (specialT.GetChild(i).childCount != 0) continue;

                    eventData.pointerDrag.transform.SetParent(specialT.GetChild(i));
                    eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
                }
            }




        } else
        {
            Debug.Log("from pool");
            TokenQueueUI tokenQueueUI = GameObject.Find("TokenQueueUI").GetComponent<TokenQueueUI>();

            bool typecheck = false;
            foreach (var token in tokenQueueUI.tokenQueue)
            {
                if (token.GetType() == typeof(ActionToken) || token.GetType() == typeof(SpecialToken))
                {
                    typecheck = true;
                    break;
                }
            }
            if (typecheck)
            {
                if (eventData.pointerDrag.GetComponent<TokenUI>().GetToken().GetType() != typeof(SupportToken)) return;
            }
            //check pos


            for (int i = 0; i < tokenQueueUI.transform.childCount; i++)
            {
                if (tokenQueueUI.transform.GetChild(i).childCount != 0) continue;
                eventData.pointerDrag.transform.SetParent(tokenQueueUI.transform.GetChild(i));
                eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
            }

            tokenQueueUI.tokenQueue.Add(eventData.pointerDrag.GetComponent<TokenUI>().GetToken());
            eventData.pointerDrag.GetComponent<TokenUI>().slotIndex = tokenQueueUI.tokenQueue.Count - 1;
                    




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
