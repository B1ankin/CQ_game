using System.Collections;
using UnityEngine;

public class HealthSystem
{
    public int maxhealth;
    public int health;

    public HealthSystem(int maxhealth)
    {
        this.maxhealth = maxhealth;
        HealFull();
    }

    public void HealFull()
    {
        this.health = maxhealth;

    }
}
