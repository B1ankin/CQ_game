using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class animationCtrl : MonoBehaviour
{
    NavMeshAgent agent;
    Animator animator;
    [SerializeField] private LayerMask layermask;

    [SerializeField] private bool Movable = true;


    Vector2 smoothDeltaPosition = Vector2.zero;
    Vector2 velocity = Vector2.zero;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        //Debug.Log(agent.velocity.magnitude);
        animator.SetFloat("vol", agent.velocity.magnitude);


        if (Input.GetMouseButtonUp(0))
        {
            Ray mousePos = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(mousePos, out hit, 200))
            {
                Debug.Log(hit.collider.name);
                if (hit.collider.gameObject.tag == "NPC" || hit.collider.gameObject.layer == 5 ) // UI layer check
                {
                    Debug.Log(hit.collider.name);
                } else
                {
                    Debug.Log(hit.collider.gameObject.layer);

                    if (Movable) agent.destination = hit.point;
                }
                //Debug.Log(hit.point);

            }
        }
    }

    public void faceTarget()
    {
        Ray mousePos = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(mousePos, out hit, 200, layermask))
        {
            transform.LookAt(hit.point);
        }
    }
}
