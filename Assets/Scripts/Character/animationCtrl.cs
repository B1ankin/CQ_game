using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class animationCtrl : MonoBehaviour
{
    NavMeshAgent agent;
    Animator animator;
    [SerializeField] private LayerMask layermask;


    Vector2 smoothDeltaPosition = Vector2.zero;
    Vector2 velocity = Vector2.zero;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Debug.Log(agent.velocity.magnitude);
        animator.SetFloat("vol", agent.velocity.magnitude);


        if (Input.GetMouseButtonUp(0))
        {
            Ray mousePos = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(mousePos, out hit, 200, layermask))
            {
                agent.destination = hit.point;
                Debug.Log(hit.point);

            }
        }
    }

}
