using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class TokenQueueUI : MonoBehaviour, IDropHandler
{
    public List<Token> tokenQueue;
    [SerializeField] private GameObject TokenSlotPrefab;


    private void Awake()
    {
        tokenQueue = new List<Token>();
    }



    private void Start()
    {
    }

    // initial
    public void UpdateTokenQueue(int queue_size)
    {
        for (int i = 0; i < queue_size; i++)
        {
            var temp = Instantiate(TokenSlotPrefab);
            temp.name = "TokenSlot " + i.ToString();
            temp.transform.SetParent(transform);
            temp.GetComponent<RectTransform>().localScale = Vector3.one;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log($"{gameObject.name}On Drop");
        if (eventData.pointerDrag != null)
        {
            //check type
            bool typecheck = false;
            foreach(var token in tokenQueue)
            {
                if (token.GetType() == typeof (ActionToken) || token.GetType() == typeof(SpecialToken))
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


            for (int i = 0 ; i < transform.childCount; i ++)
            {
                if (transform.GetChild(i).childCount != 0) continue;

                eventData.pointerDrag.transform.SetParent(transform.GetChild(i));
                eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = new Vector3(10, -10, 0);
            }


            //GetComponent<RectTransform>().anchoredPosition;
            tokenQueue.Add(eventData.pointerDrag.GetComponent<TokenUI>().GetToken());
            eventData.pointerDrag.GetComponent<TokenUI>().slotIndex = tokenQueue.Count - 1;
            foreach (var a in tokenQueue)
            {
                Debug.Log(a.tokenName);
            }
            //BattleController.Instance.AddTokenToQueue(eventData.pointerDrag.GetComponent<TokenUI>().GetToken());

        }
    }


    public void ResortQueueDisplay()
    {
        Debug.Log("Passed");
        for(int i= transform.childCount -1 ; i >= 0; i--)
        {
            Debug.Log($"{transform.GetChild(i).name} + {i}");
            if (transform.GetChild(i).childCount == 0)
            {
                if (i == 0) break; // 到尾端
                transform.GetChild(i - 1).GetChild(0).GetComponent<TokenUI>().slotIndex -= 1;
                transform.GetChild(i - 1).GetChild(0).SetParent(transform.GetChild(i));

            }
        }
    }

    public void RemoveTokenFromQueue(int slotIndex)
    {
        tokenQueue.RemoveAt(slotIndex);
        ResortQueueDisplay();

    }


    public void ResetTokenQueue()
    {
        tokenQueue = new List<Token>();

        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }



    public List<Token> GetTokenQueue()
    {
        return this.tokenQueue;
    }
}
