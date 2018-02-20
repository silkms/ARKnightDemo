using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFacingBillboard : MonoBehaviour
{
	
	// Update is called once per frame
	void Update ()
    {
        if (Camera.main)
        {
            // Aim billboard at camera
            transform.LookAt(Camera.main.transform.position);
            transform.Rotate(0, 180, 0);
        }
	}
}
