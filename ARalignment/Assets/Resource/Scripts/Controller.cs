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
    public GameObject FitToScanOverlay;

    private Dictionary<int, EnomotoVisualizer> m_Visualizers
            = new Dictionary<int, EnomotoVisualizer>();

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

    private int mode = 88;

    public void changeMode()
    {
        if (mode == 90)
        {
            mode -= 3;
        }
        mode += 1;

        var mode_btn = GameObject.Find("Canvas_menu2/mode_btn/Text").GetComponent<Text>();
        mode_btn.text = "" + (char)mode;
    }

    public void increase()
    {
        EnomotoVisualizer Enomoto = null;
        m_Visualizers.TryGetValue(0, out Enomoto);

        if (mode == 88)
        {
            Enomoto.offsetX += 0.005f;
        }

        else if (mode == 89)
        {
            Enomoto.offsetY += 0.005f;
        }

        else if (mode == 90)
        {
            Enomoto.offsetZ += 0.005f;
        }
    }

    public void decrease()
    {
        EnomotoVisualizer Enomoto = null;
        m_Visualizers.TryGetValue(0, out Enomoto);

        if (mode == 88)
        {
            Enomoto.offsetX -= 0.005f;
        }

        else if (mode == 89)
        {
            Enomoto.offsetY -= 0.005f;
        }

        else if (mode == 90)
        {
            Enomoto.offsetZ -= 0.005f;
        }
    }

}

