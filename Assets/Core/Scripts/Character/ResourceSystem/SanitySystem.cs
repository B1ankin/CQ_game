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

}
