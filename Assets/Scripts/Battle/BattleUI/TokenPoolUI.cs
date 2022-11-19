using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TokenPoolUI : MonoBehaviour, IDropHandler
{

    public List<ActionToken> actionTokens;
    public List<SupportToken> supportTokens;
    public List<SpecialToken> specialTokens;

    [SerializeField]
    private GameObject tokenPrefab;
    [SerializeField]
    private GameObject tokenSlotPrefab;


    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log($"{gameObject.name}On Drop");
        if (eventData.pointerDrag != null)
        {
            //check type
            var temptype = eventData.pointerDrag.GetComponent<TokenUI>().GetToken().GetType();
            Debug.Log(temptype);

            if (eventData.pointerDrag.transform.IsChildOf(GameObject.Find("TokenQueueUI").transform))
            {
                GameObject.Find("TokenQueueUI").GetComponent<TokenQueueUI>().
                    RemoveTokenFromQueue(eventData.pointerDrag.GetComponent<TokenUI>().slotIndex);
            }





            if (temptype == typeof(ActionToken))
            {
                Transform actionT = transform.GetChild(0);

                for (int i = actionT.childCount - 1; i >= 0; i--)
                {
                    if (actionT.GetChild(i).childCount != 0) continue;

                    eventData.pointerDrag.transform.SetParent(actionT.GetChild(i));
                    eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = new Vector3(0,0, 0);
                }
            } else if (temptype == typeof(SupportToken))
            {
                Transform supportT = transform.GetChild(1);

                for (int i = supportT.childCount - 1; i >= 0; i--)
                {
                    if (supportT.GetChild(i).childCount != 0) continue;

                    eventData.pointerDrag.transform.SetParent(supportT.GetChild(i));
                    eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
                }
            } else if (temptype == typeof(SpecialToken))
            {
                Transform actionT = transform.GetChild(2);

                for (int i = actionT.childCount - 1; i >= 0; i--)
                {
                    if (actionT.GetChild(i).childCount != 0) continue;

                    eventData.pointerDrag.transform.SetParent(actionT.GetChild(i));
                    eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
                }
            }


            
        }
    }

    public void LoadData(BattleCharacter bc)
    {
        actionTokens = bc.GetActionTokens();
        supportTokens = bc.GetSupportTokens();
        specialTokens = bc.GetSpecialTokens();

        UpdateTokenPoolUI();


    }

    public void UpdateTokenPoolUI()
    {
        // destroy current UIinfo
        var action = transform.Find("ActionTokenUI");
        for(int i = 0; i < action.childCount; i++)
        {
            Destroy(action.GetChild(i).gameObject);
        }
        // Update new UI info 
        for (int i = 0; i < actionTokens.Count; i++)
        {
            var tempSlot = Instantiate(tokenSlotPrefab);
            tempSlot.transform.SetParent(action, false);
            var tempToken = Instantiate(tokenPrefab);
            tempToken.transform.SetParent(tempSlot.transform, false);
            tempToken.GetComponent<RectTransform>().pivot = new Vector2(0, 0);
            tempToken.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);

            tempToken.GetComponent<TokenUI>().UpdateToken(actionTokens[i]);
        }

        var support = transform.Find("SupportTokenUI");
        for (int i = 0; i < support.childCount; i++)
        {
            Destroy(support.GetChild(i).gameObject);
        }
        // Update new UI info 
        for (int i = 0; i < supportTokens.Count; i++)
        {
            var tempSlot = Instantiate(tokenSlotPrefab);
            tempSlot.transform.SetParent(support, false);
            var tempToken = Instantiate(tokenPrefab);
            tempToken.transform.SetParent(tempSlot.transform, false);
            tempToken.GetComponent<RectTransform>().pivot = new Vector2(0, 0);
            tempToken.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);

            tempToken.GetComponent<TokenUI>().UpdateToken(supportTokens[i]);
        }

        var special = transform.Find("SpecialTokenUI");
        for (int i = 0; i < special.childCount; i++)
        {
            Destroy(special.GetChild(i).gameObject);
        }
        // Update new UI info 
        for (int i = 0; i < specialTokens.Count; i++)
        {
            var tempSlot = Instantiate(tokenSlotPrefab);
            tempSlot.transform.SetParent(special, false);
            var tempToken = Instantiate(tokenPrefab);
            tempToken.transform.SetParent(tempSlot.transform, false);
            tempToken.GetComponent<RectTransform>().pivot = new Vector2(0, 0);
            tempToken.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);

            tempToken.GetComponent<TokenUI>().UpdateToken(specialTokens[i]);
        }


    }




}
