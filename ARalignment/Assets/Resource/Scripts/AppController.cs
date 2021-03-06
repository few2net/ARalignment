﻿using System.Collections.Generic;
using System.Runtime.InteropServices;
using GoogleARCore;
using UnityEngine;
using UnityEngine.UI;

public class AppController : MonoBehaviour
{
    public GameObject point_cloud;
    public Canvas Canvas0;
    public Canvas Canvas1;
    public Canvas Canvas2;

    public static DetectedPlane default_plane;
    public static bool others_tgl_state = true;
    public static bool default_tgl_state = true;

    private int menu = 0;
    private int next = 1;

    private List<DetectedPlane> m_AllPlanes = new List<DetectedPlane>();

    // Use this for initialization
    void Awake()
    {
        point_cloud.gameObject.SetActive(true);
        Canvas0.gameObject.SetActive(true);
        Canvas1.gameObject.SetActive(false);
        Canvas2.gameObject.SetActive(false);
        next = 1;
        default_plane = null;
    }

    void Update()
    {
        // Exit the app when the 'back' button is pressed.
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }

        // Check that motion tracking is tracking.
        if (Session.Status != SessionStatus.Tracking)
        {
            return;
        }

        // Hide snackbar when currently tracking at least one plane.
        menu = 0;
        Session.GetTrackables<DetectedPlane>(m_AllPlanes);
        for (int i = 0; i < m_AllPlanes.Count; i++)
        {
            if (m_AllPlanes[i].TrackingState == TrackingState.Tracking)
            {
                menu = next;
                break;
            }
        }
        
        if (menu == 0)
        {
            recall();
        }
        else if (menu == 1)
        {
            Canvas0.gameObject.SetActive(false);
            Canvas1.gameObject.SetActive(true);
            Canvas2.gameObject.SetActive(false);
        }
        else if (menu == 2)
        {
            Canvas0.gameObject.SetActive(false);
            Canvas1.gameObject.SetActive(false);
            Canvas2.gameObject.SetActive(true);
        }
    }

    public void recall()
    {
        point_cloud.gameObject.SetActive(true);
        Canvas0.gameObject.SetActive(true);
        Canvas1.gameObject.SetActive(false);
        Canvas2.gameObject.SetActive(false);
        next = 1;
        default_plane = null;
    }

    public void lock_btn_tricked()
    {
        next = 2;
        default_plane = CurserRay.plane;
        
    }

    public void others_tgl_tricked(bool val)
    {
        others_tgl_state = val;
    }

    public void default_tgl_tricked(bool val)
    {
        default_tgl_state = val;
    }
}
