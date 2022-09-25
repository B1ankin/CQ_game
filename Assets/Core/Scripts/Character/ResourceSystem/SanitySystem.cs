using System.Collections;
using UnityEngine;

public class SanitySystem
{
    public int maxSanity;
    public int sanity;

    public SanitySystem(int maxSanity)
    {
        this.maxSanity = maxSanity;
        HealFull();
    }

    public void HealFull()
    {
        this.sanity = maxSanity;

    }

    public void UpdateSanity(int value)
    {
        if(sanity + value > maxSanity)
        {
            sanity = maxSanity;
        } else if ( sanity + value <= 0) {
            sanity = 0;
        }
    }

    public float GetSanityPercent()
    {
        return 1.0f * sanity / maxSanity;
    }

}
