using System.Collections.Generic;
using System.Runtime.InteropServices;
using GoogleARCore;
using UnityEngine;
using UnityEngine.UI;

public class Interface : MonoBehaviour
{

    public Canvas Canvas0;
    public Canvas Canvas1;
    public Canvas Canvas2;

    public static DetectedPlane default_plane;

    private int menu = 0;
    private int next = 1;

    private List<DetectedPlane> m_AllPlanes = new List<DetectedPlane>();

    // Use this for initialization
    void Awake()
    {
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
            Awake();
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

    public void lock_btn_tricked()
    {
        next = 2;
        default_plane = CurserRay.plane;
        
    }
}
