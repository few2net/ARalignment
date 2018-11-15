using System.Collections.Generic;
using System.Runtime.InteropServices;
using GoogleARCore;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controller for AugmentedImage.
/// </summary>
/// 

public class Controller : MonoBehaviour
{

    public EnomotoVisualizer VisualizePrefab;
    public GameObject Canvas0;
    public GameObject Canvas1;
    public GameObject FitToScanOverlay;
    
    private Dictionary<int, EnomotoVisualizer> m_Visualizers
            = new Dictionary<int, EnomotoVisualizer>();

    private List<DetectedPlane> m_AllPlanes = new List<DetectedPlane>();
    private List<AugmentedImage> m_TempAugmentedImages = new List<AugmentedImage>();

    // Update is called once per frame
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
        Session.GetTrackables<DetectedPlane>(m_AllPlanes);
        bool showSearchingUI = true;
        for (int i = 0; i < m_AllPlanes.Count; i++)
        {
            if (m_AllPlanes[i].TrackingState == TrackingState.Tracking)
            {
                showSearchingUI = false;
                break;
            }
        }
        Canvas0.SetActive(showSearchingUI);
        Canvas1.SetActive(!showSearchingUI);

        // Get updated augmented images for this frame.
        Session.GetTrackables<AugmentedImage>(m_TempAugmentedImages, TrackableQueryFilter.Updated);
        
        // Create visualizers and anchors for updated augmented images that are tracking and do not previously
        // have a visualizer. Remove visualizers for stopped images.
        foreach (var image in m_TempAugmentedImages)
        {
            EnomotoVisualizer visualizer = null;
            m_Visualizers.TryGetValue(image.DatabaseIndex, out visualizer);
            if (image.TrackingState == TrackingState.Tracking && visualizer == null)
            {
                // Create an anchor to ensure that ARCore keeps tracking this augmented image.
                Anchor anchor = image.CreateAnchor(image.CenterPose);
                visualizer = (EnomotoVisualizer)Instantiate(VisualizePrefab, anchor.transform);
                visualizer.Image = image;
                m_Visualizers.Add(image.DatabaseIndex, visualizer);
            }
            else if (image.TrackingState == TrackingState.Stopped && visualizer != null)
            {
                m_Visualizers.Remove(image.DatabaseIndex);
                GameObject.Destroy(visualizer.gameObject);
            }
        }

        // Show the fit-to-scan overlay if there are no images that are Tracking.
        foreach (var visualizer in m_Visualizers.Values)
        {
            if (visualizer.Image.TrackingState == TrackingState.Tracking)
            {
                FitToScanOverlay.SetActive(false);
                return;
            }
        }

        FitToScanOverlay.SetActive(true);
    }

    /// <summary>
    /// button code
    /// </summary>
    
    private int mode = 0;
    private int axis = 0;
    private string[] mode_text = {"Pos", "Rot"};
    private string[] axis_text = { "X", "Y", "Z" };

    public void changeMode()
    {
        if (mode == 1)
        {
            mode -= 2;
        }
        mode += 1;

        var mode_btn = GameObject.Find("Canvas/mode_btn/Text").GetComponent<Text>();
        mode_btn.text = mode_text[mode];
    }

    public void changeAxis()
    {
        if (axis == 2)
        {
            axis -= 3;
        }
        axis += 1;

        var axis_btn = GameObject.Find("Canvas/axis_btn/Text").GetComponent<Text>();
        axis_btn.text = axis_text[axis];
    }

    public void increase()
    {
        EnomotoVisualizer Enomoto = null;
        m_Visualizers.TryGetValue(0, out Enomoto);

        if (mode == 0 && axis == 0)
        {
            Enomoto.offsetX += 0.005f;
        }
        else if (mode == 0 && axis == 1)
        {
            Enomoto.offsetY += 0.005f;
        }
        else if (mode == 0 && axis == 2)
        {
            Enomoto.offsetZ += 0.005f;
        }
        else if (mode == 1 && axis == 0)
        {
            Enomoto.transform.Rotate(Vector3.right);
        }
        else if (mode == 1 && axis == 1)
        {
            Enomoto.transform.Rotate(Vector3.up);
        }
        else if (mode == 1 && axis == 2)
        {
            Enomoto.transform.Rotate(Vector3.forward);
        }
    }

    public void decrease()
    {
        EnomotoVisualizer Enomoto = null;
        m_Visualizers.TryGetValue(0, out Enomoto);

        if (mode == 0 && axis == 0)
        {
            Enomoto.offsetX -= 0.005f;
        }
        else if (mode == 0 && axis == 1)
        {
            Enomoto.offsetY -= 0.005f;
        }
        else if (mode == 0 && axis == 2)
        {
            Enomoto.offsetZ -= 0.005f;
        }
        else if (mode == 1 && axis == 0)
        {
            Enomoto.transform.Rotate(Vector3.left);
        }
        else if (mode == 1 && axis == 1)
        {
            Enomoto.transform.Rotate(Vector3.down);
        }
        else if (mode == 1 && axis == 2)
        {
            Enomoto.transform.Rotate(Vector3.back);
        }
    }

}

