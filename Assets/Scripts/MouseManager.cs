﻿using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Responsible for mouse movement including panning,zooming and clicking
/// </summary>
public class MouseManager : MonoBehaviour
{
    // the main camera of the scene
    [Tooltip("Just put main camera in 0_O")]
    public GameObject mainCamera;
    public CameraSettings cameraSettings;


    // need to be set from GameManager: Boundaries of terrain: min x (left), max x (right), min z (near), max z(far).
    // this are Demo data
    private float boundaryMinX = -40f;
    private float boundaryMaxX = 40f;
    private float boundaryMinZ = 5 - 45;
    private float boundaryMaxZ = 55 - 5;

    // need to be set from GameManager:
    // Boundaries of camera height: max: highest camera position (min zoom), min: lowest position (max zoom)
    private float boundaryMinY = 2.5f;
    private float boundaryMaxY = 75f;

    private Vector3 lastMousePosition;
    
    private Vector3 CalculatedCamPos;

    private Vector2 panMovement; 
    private float zoomYZMovement;



   
    
    // Update is called once per frame
    void Update()
    {
        // CalculatedCamPos = mainCamera.transform.position;

        // saves this as last mouse pposition if button is pressed in this frame
        if (Input.GetMouseButtonDown(1)) lastMousePosition = Input.mousePosition;        

        // returns name of clicked object in console
        if (Input.GetMouseButtonDown(0)) Debug.Log("Hello, I am " + getClickedObject());
        
        SetNewMainCamPos();
    }

    /* 
     * Calculates and sets new positon of main camera
     */
    private void SetNewMainCamPos()
    {
        // get calculated cam movements
        panMovement = Input.GetMouseButton(1) ? panning() : Vector2.zero;
        zoomYZMovement = Input.mouseScrollDelta.y != 0 ? zooming() : 0f;

        // calculate the theoretical camera position - not regarding boundaries/ max/ min zoom
        // x is included in zooming to avoid "loosing focus". 
        // Otherwise, the view screen would be moved while moving. (because of camera angle)
        CalculatedCamPos = mainCamera.transform.position + new Vector3(
            -panMovement.x,
            -zoomYZMovement,
            (zoomYZMovement - panMovement.y)
            );

        // Clamping the CamPos --> camera move within boundaries, has min/ max zoom. 
        // Math.clamp() --> clamping --> camera moves only in boundaries of map. 
        mainCamera.transform.position = new Vector3(
            Mathf.Clamp(CalculatedCamPos.x, boundaryMinX, boundaryMaxX),
            Mathf.Clamp(CalculatedCamPos.y, boundaryMinY, boundaryMaxY),
            Mathf.Clamp(CalculatedCamPos.z, boundaryMinZ, boundaryMaxZ)
            );
    }


    /*
     * calculates xz movement for the camera
     */
    private Vector2 panning(){
       // mouse movement since last frame
       // save current mouse position as last one for next frame
        Vector3 mouseDelta = Input.mousePosition - lastMousePosition;
        lastMousePosition = Input.mousePosition;

        float mainCameraMovementSpeed = cameraSettings.MoveSpeed;
        // return calculated movement on x & z axis
        return new Vector2(
                mouseDelta.x * mainCameraMovementSpeed,
                mouseDelta.y * mainCameraMovementSpeed);        
    }

    /// <summary>
    /// calculates zooming movement for camera positioning.
    /// </summary>
    private float zooming()
    {
        return Input.mouseScrollDelta.y * cameraSettings.ZoomSpeed * Time.deltaTime;
    }


    /* should be called by GameManager to set xz boundaries
     * Manupulations to z values are necessary because of the camera angle. 
     */ 
    public void DefinePanningBoundaries(float xMin, float xMax, float zMin, float zMax)
    {        
        (boundaryMinX, boundaryMaxX, boundaryMinZ, boundaryMaxZ) = (xMin, xMax, zMin - 45, zMax - 5);
    }


 /* should be called by GameManager to set xz boundaries
 * Set minimal and maximal height of camera. 
 */
    public void DefineZoomBoundaries(float yMin, float yMax)
    {
        (boundaryMinY, boundaryMaxY) = (yMin, yMax);
    }

    /*
     * returns name of clicked object or error text if that's not possible. 
     */ 
    private string getClickedObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100)) return hit.transform.gameObject.name;
        
        return "unknown und probably too far away or too small to be clicked. Sorry :/";
    }
}
