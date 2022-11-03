using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageFaceCamera : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        FaceCam();
    }

    public void FaceCam()
    {
        transform.rotation = Camera.main.transform.rotation;
    }

    
}
