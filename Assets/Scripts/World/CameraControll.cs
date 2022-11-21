using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControll : MonoBehaviour
{
    [SerializeField] private float CamMoveSpeed = 0.1f;
    Camera cam;

    [SerializeField] private GameObject focusedTarget = null ;
    [SerializeField] private float camDelay = 0.2f;
    private Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        offset = cam.transform.position - focusedTarget.transform.position;
    }

    [SerializeField] private CameraControlMode camControlMode;
    private enum CameraControlMode
    {
        manuel = 0,
        auto = 1
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        if(camControlMode == CameraControlMode.manuel)
        {
            CamManuelCtrl();
        } else if (camControlMode == CameraControlMode.auto)
        {
            CamAutoCtrl();
        }
    }


    private void CamManuelCtrl()
    {
        if (Input.GetKey(KeyCode.W))
        {
            cam.transform.position += new Vector3(0, 0, 1) * CamMoveSpeed;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            cam.transform.position += new Vector3(0, 0, -1) * CamMoveSpeed;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            cam.transform.position += new Vector3(-1, 0, 0) * CamMoveSpeed;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            cam.transform.position += new Vector3(1, 0, 0) * CamMoveSpeed;
        }
    }

    public void CamAutoCtrl()
    {
        Vector3 targetPos = focusedTarget.transform.position + offset;
        Vector3 smoothFollow = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * camDelay);

        transform.position = smoothFollow;
        //transform.LookAt(targetPos);
    }
}
