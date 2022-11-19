using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCharacterAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    private SpriteRenderer spriteRenderer;

    // animtor and 
    


    void Start()
    {
        this.transform.rotation = Camera.main.transform.rotation;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// Left and right direction flip 
    /// </summary>
    public void Flip(bool dir)
    {
        if (dir)
        {
            spriteRenderer.flipX = true;
        } else
        {
            spriteRenderer.flipX = false;
        }
    }

}
