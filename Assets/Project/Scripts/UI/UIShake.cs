using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class UIShake : MonoBehaviour
{
    [Tooltip("Will Apply Subtle Movement To The UI")]
    public bool handheldMovement = true;
    [Tooltip("Will Use Unscaled Time")]
    public bool useUnscaledTime = true;
    [Tooltip("Will Adjust The Position Of The UI Based On The Position Of The Mouse")]
    public float mouseSensitivity;
    [Tooltip("How Fast The Movement Of The UI Is")]
    public float movementSpeed = 10f;
    [Tooltip("The Strength Of The Handheld Motion Effect")]
    public float handheldStrength = 8f;
    [Tooltip("How Smooth The Movement Of The UI Is In Handheld Mode")]
    public float handheldSmoothing = .03f;
    [Tooltip("How Often The UI Finds A New Handheld Position")]
    public float handheldFrequency = .1f;

    private float _shakeStrength = .15f;
    private float _shakeDuration = .1f;
    private float _handheldTimer;
    private Vector2 _handheldPos;
    private Vector3 _axis;
    private Vector2 _mouseVector;
    private Coroutine _shakeCoroutine;
    private RectTransform _rect;

    private void Start()
    {
        _rect = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (_handheldTimer > 0) _handheldTimer -= Time.deltaTime;

        if (_handheldTimer <= 0 && handheldMovement)
        {
            if (handheldStrength > 0)
            {
                _handheldPos = Vector2.zero;
                _handheldPos.x += Random.value * handheldStrength * 2 - handheldStrength;
                _handheldPos.y += Random.value * handheldStrength * 2 - handheldStrength;
            }

            _handheldTimer = handheldFrequency;
        }

        if (!handheldMovement) _handheldTimer = 0f;
        _mouseVector = (Vector2) Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()) * mouseSensitivity;
        _axis = _mouseVector;

        _rect.position = useUnscaledTime
        ? Vector3.Lerp(_rect.position, _axis, Time.unscaledDeltaTime * movementSpeed)
        : Vector3.Lerp(_rect.position, _axis, Time.deltaTime * movementSpeed);

        _rect.position = useUnscaledTime
        ? Vector3.Lerp(_rect.position, _handheldPos, Time.unscaledDeltaTime * handheldSmoothing)
        : Vector3.Lerp(_rect.position, _handheldPos, Time.deltaTime * handheldSmoothing);
    }

    public void Shake(float strength, float duration)
    {
        _shakeStrength = strength;
        _shakeDuration = duration;
        _shakeCoroutine = StartCoroutine(InvokeRepeatingRealtime(nameof(BeginCamShake), 0f, .01f));
        StartCoroutine(InvokeRealtime(nameof(StopCamShake), _shakeDuration));
    }

    private void BeginCamShake()
    {
        if (_shakeStrength <= 0) return;
        var camPos = _rect.position;
        float shakeOffsetX = Random.value * _shakeStrength * 2 - _shakeStrength;
        float shakeOffsetY = Random.value * _shakeStrength * 2 - _shakeStrength;
        camPos.x += shakeOffsetX;
        camPos.y += shakeOffsetY;
        _rect.position = camPos;
    }

    private void StopCamShake() => StopCoroutine(_shakeCoroutine);

    private IEnumerator InvokeRealtime(string method, float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        SendMessage(method);
    }

    private IEnumerator InvokeRepeatingRealtime(string method, float initDelay, float repeatDelay)
    {
        int count = 0;
        
        while (true)
        {
            count++;
            yield return new WaitForSecondsRealtime(count == 1 ? initDelay : repeatDelay);
            SendMessage(method);
        }
    }
}
