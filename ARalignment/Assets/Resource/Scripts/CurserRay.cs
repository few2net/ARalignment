using System.Collections.Generic;
using System.Runtime.InteropServices;
using GoogleARCore;
using UnityEngine;
using UnityEngine.UI;

public class CurserRay : MonoBehaviour {

    public static DetectedPlane plane;
	
	// Update is called once per frame
	void Update () {

        // Raycast against the location the player touched to search for planes.
        TrackableHit hit;
        TrackableHitFlags raycastFilter = TrackableHitFlags.PlaneWithinPolygon |
            TrackableHitFlags.FeaturePointWithSurfaceNormal;

        if (Frame.Raycast(Screen.width/2, Screen.height/2, raycastFilter, out hit))
        {
            // Use hit pose and camera pose to check if hittest is from the
            // back of the plane, if it is, no need to create the anchor.
            if (hit.Trackable is DetectedPlane)
            {
                Debug.Log("Hit DetectedPlane");
                plane = (DetectedPlane)hit.Trackable;
                print(plane.CenterPose.position.y);
            } 
        }
        else
        {
            plane = null;
        }

        if (plane == null)
        {
            GameObject.Find("Canvas1/lock_btn").GetComponent<Button>().interactable = false;
        }
        else
        {
            GameObject.Find("Canvas1/lock_btn").GetComponent<Button>().interactable = true;
        }
    }

}
