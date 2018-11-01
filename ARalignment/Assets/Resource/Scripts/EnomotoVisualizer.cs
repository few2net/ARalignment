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


        Enomoto.transform.localPosition = new Vector3(offsetX, offsetY, offsetZ);
        Enomoto.SetActive(true);

        var posX = GameObject.Find("Canvas_menu2/posX").GetComponent<Text>();
        posX.text = "X : " + offsetX;
        var posY = GameObject.Find("Canvas_menu2/posY").GetComponent<Text>();
        posY.text = "Y : " + offsetY;
        var posZ = GameObject.Find("Canvas_menu2/posZ").GetComponent<Text>();
        posZ.text = "Z : " + offsetZ;

    }
}
