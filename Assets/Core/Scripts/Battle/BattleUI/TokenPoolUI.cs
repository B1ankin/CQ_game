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

            //check pos
            Transform actionT = transform.GetChild(0);

            for(int i = actionT.childCount - 1; i >= 0 ; i--)
            {
                if (actionT.GetChild(i).childCount != 0) continue;

                eventData.pointerDrag.transform.SetParent(actionT.GetChild(i));
                eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = new Vector3(10, -10, 0);
            }


            //GetComponent<RectTransform>().anchoredPosition;

            // call tokenQueue update
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
            tempToken.GetComponent<RectTransform>().anchoredPosition = new Vector3(10, 10, 0);

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
            tempToken.GetComponent<RectTransform>().anchoredPosition = new Vector3(10, 10, 0);

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
            tempToken.GetComponent<RectTransform>().anchoredPosition = new Vector3(10, 10, 0);

            tempToken.GetComponent<TokenUI>().UpdateToken(specialTokens[i]);
        }


    }




}
