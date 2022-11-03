using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControll : MonoBehaviour
{
    [SerializeField] private float CamMoveSpeed = 0.1f;
    Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            cam.transform.position += new Vector3(0,0,1) * CamMoveSpeed;
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
}
