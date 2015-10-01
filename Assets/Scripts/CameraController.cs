// <copyright file="CameraController.cs" company="Wadii Bellamine">
// Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Wadii Bellamine</author>
// <date>2/25/2015 8:37 AM </date>

using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    public GameObject target;                           // Target to follow
    public float targetHeight = 1.7f;                         // Vertical offset adjustment
    public float distance = 12.0f;                            // Default Distance
    public float offsetFromWall = 2.0f;                       // Bring camera away from any colliding objects
    public float maxDistance = 30f;                       // Maximum zoom Distance
    public float minDistance = 2.0f;                      // Minimum zoom Distance
    public float xSpeed = 200.0f;                             // Orbit speed (Left/Right)
    public float ySpeed = 200.0f;                             // Orbit speed (Up/Down)
    public float yMinLimit = -80f;                            // Looking up limit
    public float yMaxLimit = 80f;                             // Looking down limit
    public float zoomRate = 40f;                          // Zoom Speed
    public float rotationDampening = 3.0f;                // Auto Rotation speed (higher = faster)
    public float zoomDampening = 3.0f;                    // Auto Zoom speed (Higher = faster)
    public LayerMask collisionLayers = -1;     // What the camera will collide with
    public bool lockToRearOfTarget = false;             // Lock camera to rear of target
    public bool allowMouseInputX = true;                // Allow player to control camera angle on the X axis (Left/Right)
    public bool allowMouseInputY = true;                // Allow player to control camera angle on the Y axis (Up/Down)

    private float xDeg = 0.0f;
    private float yDeg = 0.0f;
    private float currentDistance;
    private float desiredDistance;
    private float correctedDistance;
    private bool rotateBehind = false;
    private bool mouseSideButton = false;
    private float pbuffer = 0.0f;       //Cooldownpuffer for SideButtons
    private float coolDown = 0.5f;      //Cooldowntime for SideButtons 

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        xDeg = angles.x;
        yDeg = angles.y;
        currentDistance = distance;
        desiredDistance = distance;
        correctedDistance = distance;

        // Make the rigid body not change rotation
		Rigidbody rigidbody = GetComponent<Rigidbody>();
        if (rigidbody)
            rigidbody.freezeRotation = true;

        if (lockToRearOfTarget)
            rotateBehind = true;
    }
    void Update()
    {
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player") as GameObject;
            Debug.Log("Looking for Player");
        }

    }

    //Only Move camera after everything else has been updated
    void LateUpdate()
    {
        // Don't do anything if target is not defined
        if (target == null)
            return;
        //pushbuffer
        if (pbuffer > 0)
            pbuffer -= Time.deltaTime;
        if (pbuffer < 0) pbuffer = 0;

       
        Vector3 vTargetOffset;

		switch (Application.platform)
		{
			case RuntimePlatform.IPhonePlayer:
			case RuntimePlatform.Android:
			case RuntimePlatform.WP8Player:
				if( (Input.touchCount == 1) && (Input.GetTouch(0).phase == TouchPhase.Moved) )
				{
					xDeg += Input.touches[0].deltaPosition.x * xSpeed * 0.004f;
					yDeg -= Input.touches[0].deltaPosition.y * ySpeed * 0.004f;
				}
				break;
			default:
				if ( (GUIUtility.hotControl == 0) && (Input.GetMouseButton(0) || Input.GetMouseButton(1)))
				{
					//Check to see if mouse input is allowed on the axis
					if (allowMouseInputX)
						xDeg += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
					else
						RotateBehindTarget();
					if (allowMouseInputY)
						yDeg -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
					
					//Interrupt rotating behind if mouse wants to control rotation
					if (!lockToRearOfTarget)
						rotateBehind = false;
				}
				break;
		}
        
        
        yDeg = ClampAngle(yDeg, yMinLimit, yMaxLimit);

        // Set camera rotation
        Quaternion rotation = Quaternion.Euler(yDeg, xDeg, 0);

        // Calculate the desired distance
		switch (Application.platform)
		{
			case RuntimePlatform.IPhonePlayer:
			case RuntimePlatform.Android:
			case RuntimePlatform.WP8Player:
				if( (Input.touchCount == 2) && ( (Input.GetTouch(0).phase == TouchPhase.Moved) || (Input.GetTouch(1).phase == TouchPhase.Moved)))
				{
					// Store both touches.
					Touch touchZero = Input.GetTouch(0);
					Touch touchOne = Input.GetTouch(1);
					
					// Find the position in the previous frame of each touch.
					Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
					Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;
					
					// Find the magnitude of the vector (the distance) between the touches in each frame.
					float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
					float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;
					
					// Find the difference in the distances between each frame.
					float dist = prevTouchDeltaMag - touchDeltaMag;
					
					//float dist = Vector2.Distance( Input.touches[0].deltaPosition, Input.touches[1].deltaPosition);
					desiredDistance += dist * Time.deltaTime * (zoomRate/50.0f) * Mathf.Abs(desiredDistance);
				}
				break;
			default:
				desiredDistance -= Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * zoomRate * Mathf.Abs(desiredDistance);
				break;
		}

			
		
        desiredDistance = Mathf.Clamp(desiredDistance, minDistance, maxDistance);
        correctedDistance = desiredDistance;

        // Calculate desired camera position
        vTargetOffset = new Vector3(0, -targetHeight, 0);
        Vector3 position = target.transform.position - (rotation * Vector3.forward * desiredDistance + vTargetOffset);

        // Check for collision using the true target's desired registration point as set by user using height
        RaycastHit collisionHit;
        Vector3 trueTargetPosition = new Vector3(target.transform.position.x, target.transform.position.y + targetHeight, target.transform.position.z);

        // If there was a collision, correct the camera position and calculate the corrected distance
        var isCorrected = false;
        if (Physics.Linecast(trueTargetPosition, position, out collisionHit, collisionLayers))
        {
            // Calculate the distance from the original estimated position to the collision location,
            // subtracting out a safety "offset" distance from the object we hit.  The offset will help
            // keep the camera from being right on top of the surface we hit, which usually shows up as
            // the surface geometry getting partially clipped by the camera's front clipping plane.
            correctedDistance = Vector3.Distance(trueTargetPosition, collisionHit.point) - offsetFromWall;
            isCorrected = true;
        }

        // For smoothing, lerp distance only if either distance wasn't corrected, or correctedDistance is more than currentDistance
        currentDistance = !isCorrected || correctedDistance > currentDistance ? Mathf.Lerp(currentDistance, correctedDistance, Time.deltaTime * zoomDampening) : correctedDistance;

        // Keep within limits
        currentDistance = Mathf.Clamp(currentDistance, minDistance, maxDistance);

        // Recalculate position based on the new currentDistance
        position = target.transform.position - (rotation * Vector3.forward * currentDistance + vTargetOffset);

        //Finally Set rotation and position of camera
        transform.rotation = rotation;
        transform.position = position;
    }

    private void RotateBehindTarget()
    {
        float targetRotationAngle = target.transform.eulerAngles.y;
        float currentRotationAngle = transform.eulerAngles.y;
        xDeg = Mathf.LerpAngle(currentRotationAngle, targetRotationAngle, rotationDampening * Time.deltaTime);

        // Stop rotating behind if not completed
        if (targetRotationAngle == currentRotationAngle)
        {
            if (!lockToRearOfTarget)
                rotateBehind = false;
        }
        else
            rotateBehind = true;

    }


    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360f)
            angle += 360f;
        if (angle > 360f)
            angle -= 360f;
        return Mathf.Clamp(angle, min, max);
    }

}