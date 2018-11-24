using System.Collections.Generic;
using System.Runtime.InteropServices;
using GoogleARCore;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controller for AugmentedImage.
/// </summary>
/// 

public class PrefabController : MonoBehaviour
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
                
                Pose p = set_proper_pose(image.CenterPose);

                // Create an anchor to ensure that ARCore keeps tracking this augmented image.
                //Anchor anchor = image.CreateAnchor(p);
                Anchor anchor = AppController.default_plane.CreateAnchor(p);
                //Anchor anchor = Session.CreateAnchor(p);
                visualizer = (EnomotoVisualizer)Instantiate(VisualizePrefab, anchor.transform);

                ////////////////////////////////////////////////////////////////////////////// <debug part>
                print("plane : " + AppController.default_plane.CenterPose);
                print(AppController.default_plane.CenterPose.rotation.eulerAngles);
                print("image : " + image.CenterPose);
                print(image.CenterPose.rotation.eulerAngles);
                print("pose : " + p);
                print(p.rotation.eulerAngles);
                ////////////////////////////////////////////////////////////////////////////// </debug part>

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

    private Pose set_proper_pose(Pose image)
    {
        Pose pose = new Pose(image.position, AppController.default_plane.CenterPose.rotation);
        pose.position.y = AppController.default_plane.CenterPose.position.y;
        pose.rotation.SetLookRotation(image.right);
        return pose;
    }
    
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
           // Enomoto.offsetY += 0.005f;
        }
        else if (mode == 0 && axis == 2)
        {
            Enomoto.offsetZ += 0.005f;
        }
        else if (mode == 1 && axis == 0)
        {
            //Enomoto.transform.Rotate(Vector3.right);
        }
        else if (mode == 1 && axis == 1)
        {
            Enomoto.transform.Rotate(Vector3.up);
        }
        else if (mode == 1 && axis == 2)
        {
           // Enomoto.transform.Rotate(Vector3.forward);
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
            //Enomoto.offsetY -= 0.005f;
        }
        else if (mode == 0 && axis == 2)
        {
            Enomoto.offsetZ -= 0.005f;
        }
        else if (mode == 1 && axis == 0)
        {
           // Enomoto.transform.Rotate(Vector3.left);
        }
        else if (mode == 1 && axis == 1)
        {
            Enomoto.transform.Rotate(Vector3.down);
        }
        else if (mode == 1 && axis == 2)
        {
           // Enomoto.transform.Rotate(Vector3.back);
        }
    }

}

