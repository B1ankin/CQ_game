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

    public bool HealthUpdate(int amount)
    {
        if( health + amount < 0)
        {
            health = health + amount;
            return false;  // 死了
        }
        else
        {
            health += amount;
            if ( health > maxhealth)
            {
                health = maxhealth;
            }
        }
        
        return true; // 活着
    }

    public bool IsDead()
    {
        if (health <= 0) return true;
        return false;
    }

    public float GetHealthPercent()
    {
        return 1.0f * health / maxhealth;
    }
}
