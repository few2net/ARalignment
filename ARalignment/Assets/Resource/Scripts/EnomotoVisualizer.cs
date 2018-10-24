using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using GoogleARCore;
using GoogleARCoreInternal;
using UnityEngine;
using UnityEngine.UI;

public class EnomotoVisualizer : MonoBehaviour
{

    /// <summary>
    /// The AugmentedImage to visualize.
    /// </summary>
    public AugmentedImage Image;

    public GameObject Enomoto;


    // refer to center point of an image
    public float offsetX = 0; // width (+right -left)
    public float offsetZ = 0; // height (+up -down)
    public float offsetY = 0; // deep (+in -out)

    public void Update()
    {
        if (Image == null || Image.TrackingState != TrackingState.Tracking)
        {
            Enomoto.SetActive(false);
            return;
        }


        if (offsetX != 0 || offsetZ != 0 || offsetY != 0)
        {
            Enomoto.transform.localPosition = (offsetX * Vector3.left) + (offsetZ * Vector3.back) + (offsetY * Vector3.up);
        }

        // Enomoto.transform.localPosition = (halfWidth * Vector3.left)+ (halfHeight * Vector3.back) * 21 +
        //      (Vector3.up);
        Enomoto.SetActive(true);

        var sizeX = GameObject.Find("Canvas/sizeX").GetComponent<Text>();
        sizeX.text = "X : " + Image.ExtentX.ToString();
        var sizeZ = GameObject.Find("Canvas/sizeZ").GetComponent<Text>();
        sizeZ.text = "Z : " + Image.ExtentZ.ToString();
        
        //center.text = "C : " + Image.CenterPose.ToString();
    }
}
