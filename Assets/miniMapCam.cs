using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class miniMapCam : MonoBehaviour
{
    [SerializeField] private float CamMoveSpeed = 0.1f;
    [SerializeField] private GameObject focusedTarget = null;
    [SerializeField] private float camDelay = 0.2f;
    private Vector3 offset;
    Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = this.GetComponent<Camera>();
        offset = cam.transform.position - focusedTarget.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPos = focusedTarget.transform.position + offset;
        Vector3 smoothFollow = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * camDelay);

        transform.position = smoothFollow;
        //transform.LookAt(targetPos);
    }   
}
