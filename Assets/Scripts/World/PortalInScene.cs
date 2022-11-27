using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class PortalInScene : MonoBehaviour
{
    public PortalInScene targetPos;
    // Start is called before the first frame update
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("123");
        collision.gameObject.transform.position = targetPos.transform.position;
    }

    // Update is called once per frame

}
