using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Camera))]
public class CameraScript : MonoBehaviour
{
    public static CameraScript Instance;
    
    [Space]
    [Header("General Settings")]

    [Tooltip("Will Lock The Movement On The Y Axis")]
    public bool lockYAxis = false;
    [Tooltip("Will Lock The Movement On The X Axis")]
    public bool lockXAxis = false;
    [Tooltip("Will Ignore Any Locked Values")]
    public bool axisInfinity = true;
    [Tooltip("Will Apply Subtle Movement To The Camera")]
    public bool handheldCamera = false;
    [Tooltip("Will Adjust The Offset Based On The Camera's Orthographic Size (Zoom)")]
    public bool dynamicOffset = true;
    [Tooltip("Will Adjust The Position Of The Camera Based On The Position Of The Mouse")]
    public bool affectedByMouse = false;
    [Tooltip("Will Use Unscaled Time")]
    public bool useUnscaledTime = false;

    [Space]
    [Tooltip("How Smooth The Movement Of The Camera Is")]
    public float movementSpeed = 10f;
    [Tooltip("The Value In Which The Offset Is Adjusted By - REQUIRES DYNAMIC OFFSET")]
    [Range(0, 1)] public float offsetMultiplier = 0.4f;

    [Space]
    [Tooltip("The Strength Of The Handheld Camera Effect")]
    public float handheldStrength = 10f;
    [Tooltip("How Smooth The Movement Of The Camera Is In Handheld Mode")]
    public float handheldSmoothing = .1f;
    [Tooltip("How Often The Camera Finds A New Handheld Position")]
    public float handheldFrequency = .1f;

    [Space]
    [Tooltip("The Offset Of The Position")]
    public Vector2 offset;
    [Tooltip("The Minimum Possible Position The In Which Camera Can Travel To")]
    public Vector2 minimumPosition;
    [Tooltip("The Maximum Possible Position In Which The Camera Can Travel To")]
    public Vector2 maximumPosition;
    [Tooltip("The Locked Position The Camera Will Stay At When 'LockX/Y' Axis Is Enabled")]
    public Vector2 lockedPosition;

    [Space]
    [Header("Follow Settings")]

    [Tooltip("Will Lock The Orthographic Zoom")]
    public bool lockZoom = false;
    [Tooltip("Will Look Ahead Of The Player")]
    public bool lookahead = true;
    [Tooltip("The Player That The Camera Will Look Ahead Of (Follow Target Element)")]
    public int lookAheadTarget = 0;
    [Tooltip("The Amount The Camera Will Use To Look Ahead")]
    public float lookAheadAmount = 0.5f;
    [Tooltip("How Fast The Camera Will Zoom In")]
    [Range(1, 10)] public float zoomSpeed = 1.5f;
    [Tooltip("The Minimum Amount The Camera Can Zoom Out To")]
    public float minimumZoom;
    [Tooltip("The Maximum Amount The Camera Can Zoom In To")]
    public float maximumZoom;
    [Tooltip("The Locked Zoom The Camera Will Stay At When 'LockZoom' Is Enabled")]
    public float lockedZoom;
    [Tooltip("The Limitation On How Far The Camera Can Zoom In And Out")]
    public float zoomDivider;
    [Tooltip("The Targets That The Camera Will Follow")]
    public List<GameObject> followTargets;

    [Space]
    [Header("Cinematography")]

    [Tooltip("The Camera Will Wait For The Current Cine Point To Finish Before Starting The Next One")]
    public bool waitForCompletion = false;
    [Tooltip("The Cine Points The Camera Will Travel To - Used For Cutscenes And Fluent Camera Movement")]
    public Transform[] cinePoints;

    private Vector3 Velocity = Vector3.zero;
    private Vector3 axis;
    private float shakeStrength = 0.1f;
    private float shakeDuration = 0.1f;
    [HideInInspector] public bool cineMode;
    private Camera mainCam;
    private Vector2 handheldPos;
    private float camZoom = 8;
    private Vector2 appliedOffset;
    [HideInInspector] public bool interestMode = false;
    private Vector2 mouseVector;
    private float handheldTimer;
    private bool currentlyShaking = false;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        mainCam = GetComponent<Camera>(); //Getting the camera component
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (handheldTimer > 0)
            handheldTimer -= Time.deltaTime;

        if (handheldTimer <= 0 && handheldCamera)
        {
            if (handheldStrength > 0)
            {
                handheldPos = Vector2.zero;
                handheldPos.x += Random.value * handheldStrength * 2 - handheldStrength; //Getting random values and applying it to 'offsetX'
                handheldPos.y += Random.value * handheldStrength * 2 - handheldStrength; //Getting random values and applying it to 'offsetY'
            }

            handheldTimer = handheldFrequency;
        }

        if (!handheldCamera)
            handheldTimer = 0f;

        if (affectedByMouse)
        {
            Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -10);
            mouseVector = Camera.main.ScreenToWorldPoint(mousePos) * 0.04f;
        }

        else mouseVector = Vector2.zero;

        if (cineMode == false && interestMode == false)
        {
            if (dynamicOffset)
            {
                if (offset.x > 0) appliedOffset.x = offset.x - GetComponent<Camera>().orthographicSize * offsetMultiplier;
                if (offset.y > 0) appliedOffset.y = offset.y - GetComponent<Camera>().orthographicSize * offsetMultiplier;
            }

            else appliedOffset = offset;

            if (lookahead)
            {
                Vector3 point = followTargets[lookAheadTarget].transform.position + new Vector3(followTargets[lookAheadTarget].GetComponent<Rigidbody2D>().velocity.x, 0, 0);
                Vector3 lookahead = (point - transform.position) * lookAheadAmount;
                axis = transform.position + lookahead + new Vector3(appliedOffset.x, appliedOffset.y, 0) + new Vector3(mouseVector.x, mouseVector.y, 0);
            }

            else axis = GetCenterPoint() + appliedOffset + mouseVector; //Setting the camera's position to "centerPoint" + "offset"
        }

        if (!lockZoom)
        {
            if (cineMode || interestMode)
            {
                camZoom = Mathf.Clamp(camZoom, maximumZoom, minimumZoom); //Clamping the zoom
                mainCam.orthographicSize = useUnscaledTime ? Mathf.Lerp(mainCam.orthographicSize, camZoom, Time.unscaledDeltaTime * zoomSpeed * 1.25f)
                    : Mathf.Lerp(mainCam.orthographicSize, camZoom, Time.deltaTime * zoomSpeed * 1.25f); //Setting the camera's orthographic size to 'zoomValue'
            }

            else
            {
                camZoom = Mathf.Clamp(GetZoomValue(), maximumZoom, minimumZoom); //Clamping the zoom
                mainCam.orthographicSize = useUnscaledTime ? Mathf.Lerp(mainCam.orthographicSize, camZoom, Time.unscaledDeltaTime * zoomSpeed)
                    : Mathf.Lerp(mainCam.orthographicSize, camZoom, Time.deltaTime * zoomSpeed); //Setting the camera's orthographic size to 'zoomValue'
            }
        }

        else mainCam.orthographicSize = lockedZoom;

        if (axisInfinity == false) //Adding bounds to the camera based on the 'minimumAxis' and 'maximumAxis' Vectors if 'axisInfinity' is false
        {
            if (axis.x < minimumPosition.x && cineMode == false) axis.x = minimumPosition.x;
            if (axis.x > maximumPosition.x && cineMode == false) axis.x = maximumPosition.x;
            if (axis.y < minimumPosition.y && cineMode == false) axis.y = minimumPosition.y;
            if (axis.y > maximumPosition.y && cineMode == false) axis.y = maximumPosition.y;
        }

        if (lockXAxis && cineMode == false) axis.x = lockedPosition.x; //Locking the x axis if 'lockXAxis' is true
        if (lockYAxis && cineMode == false) axis.y = lockedPosition.y; //Locking the y axis if 'lockYAxis' is true

        Vector3 targetVelocity = new Vector3(axis.x, axis.y, -10); //Getting the position of the axis and mouse position then setting that Vector3 as 'targetVelocity'

        transform.position = useUnscaledTime
            ? Vector3.Lerp(transform.position, targetVelocity, Time.unscaledDeltaTime * movementSpeed)
            : Vector3.Lerp(transform.position, targetVelocity, Time.deltaTime * movementSpeed);

        transform.position = useUnscaledTime
            ? Vector3.Lerp(transform.position, handheldPos, Time.unscaledDeltaTime * handheldSmoothing)
            : Vector3.Lerp(transform.position, handheldPos, Time.deltaTime * handheldSmoothing);
    }

    public void Shake(float strength, float duration)
    {
        if (!currentlyShaking || duration > shakeDuration)
        {
            currentlyShaking = true;
            shakeStrength = strength; //Setting 'shakeAmount' to the value of the element from the number found in the string
            shakeDuration = duration; //Setting 'shakeDuration' to the value of the element from the number found in the string
            InvokeRepeating(nameof(BeginCamShake), 0, .01f); //Invoking the shake effect
            Invoke(nameof(StopCamShake), shakeDuration); //Stopping the shake effect after: 'shakeDuration' is up
        }
    }

    private void BeginCamShake()
    {
        if (shakeStrength > 0)
        {
            Vector3 camPos = transform.position;
            float shakeOffsetX = Random.value * shakeStrength * 2 - shakeStrength; //Getting random values and applying it to 'shakeOffsetX'
            float shakeOffsetY = Random.value * shakeStrength * 2 - shakeStrength; //Getting random values and applying it to 'shakeOffsetY'
            camPos.x += shakeOffsetX;
            camPos.y += shakeOffsetY;
            transform.position = camPos;
        }
    }

    public void StopCamShake()
    {
        CancelInvoke(nameof(BeginCamShake)); //Canceling original Invoke of: 'BeginCamShake'
        currentlyShaking = false;
    }

    public void CinePoint(int point, float zoom, float duration)
    {
        if (waitForCompletion && !cineMode) StartCoroutine(CinePointTravel(point, zoom, duration));
        else StartCoroutine(CinePointTravel(point, zoom, duration));
    }

    IEnumerator CinePointTravel(int point, float zoom, float duration)
    {
        if (duration > 0)
        {
            cineMode = true;
            camZoom = zoom;
            axis = cinePoints[point].position;
            yield return new WaitForSeconds(duration);
            cineMode = false;
        }
    }

    private Vector2 GetCenterPoint()
    {
        if (followTargets.Count == 0) return Vector2.zero;

        if (followTargets.Count == 1 && followTargets[0] != null && followTargets[0].activeInHierarchy)
            return followTargets[0].transform.position;

        Bounds bounds = new Bounds();

        if (followTargets[0] != null && followTargets[0].activeInHierarchy)
            bounds = new Bounds(followTargets[0].transform.position, Vector2.zero);

        foreach (var target in followTargets)
        {
            if (target != null && target.activeInHierarchy)
                bounds.Encapsulate(target.transform.position);
        }

        return bounds.center;
    }

    private float GetZoomValue()
    {
        Bounds bounds = new Bounds(followTargets[0].transform.position, Vector2.zero);
        if (followTargets.Count == 1 || followTargets.Count == 0) return (maximumZoom + minimumZoom) / 2;

        foreach (var target in followTargets) if (target.activeInHierarchy) bounds.Encapsulate(target.transform.position);
        if (bounds.size.y > bounds.size.x && bounds.size.y > maximumZoom * 1.005f) return bounds.size.y * 1.5f;
        return bounds.size.x * 1.5f;
    }

    public void InterestPointEnter(GameObject interestObject)
    {
        if (!cineMode && !interestMode)
        {
            interestMode = true;
            axis = interestObject.transform.position + new Vector3(offset.x, offset.y, 0);
            camZoom--;
        }
    }

    public void InterestPointExit()
    {
        interestMode = false;
        FindObjectOfType<IPointsDefiner>().latestPoint = null;
    }
}
