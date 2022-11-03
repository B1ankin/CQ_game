using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationCtrl : MonoBehaviour
{
    public int animationState = 0;
    private int currentStat = 0;


    // Update is called once per frame
    private void FixedUpdate()
    {
        if(currentStat != animationState)
        {
            this.GetComponent<Animator>().SetInteger("animationStat", animationState);
            currentStat = animationState;
        }
    }

}
