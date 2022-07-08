using System.Collections;
using System.Collections.Generic;
using Doozy.Runtime.Reactor.Animators;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class BeatManager : MonoBehaviour
{
    public TMP_Text countdownText;
    public float zoomMultiplier;
    [ReadOnly] public float lastBeat;

    private float _startZoom;
    private int _currentCount;
    private float[] _samples;
    private float _prevAudioValue;
    private float _audioValue;
    private float _beatTimer;

    // Start is called before the first frame update
    void Start()
    {
        _startZoom = Camera.main.orthographicSize;
        _samples = new float[128];
    }

    // Update is called once per frame
    void Update()
    {
        var beatData = MusicManager.Instance.currentBeatData;
        if (Keyboard.current.upArrowKey.wasPressedThisFrame) Pulse();
        MusicManager.Instance.audioSource.GetSpectrumData(_samples, 0, FFTWindow.BlackmanHarris);
        
        if (_samples is { Length: > 0 })
        {
            _prevAudioValue = _audioValue;
            _audioValue = _samples[0] * 100;
            lastBeat = _audioValue;
            if (_prevAudioValue > beatData.beatBias && _audioValue <= beatData.beatBias && _beatTimer > beatData.beatStep) Pulse();
            if (_prevAudioValue <= beatData.beatBias && _audioValue > beatData.beatBias && _beatTimer > beatData.beatStep) Pulse();
            _beatTimer += Time.deltaTime;
        }
        
    }

    public void Pulse()
    {
        _beatTimer = 0;
        Camera.main.orthographicSize = _startZoom * zoomMultiplier;
        _currentCount = _currentCount == 4 ? 1 : _currentCount + 1;
        countdownText.GetComponent<UIAnimator>().Play();
        countdownText.text = _currentCount.ToString();
    }
}
