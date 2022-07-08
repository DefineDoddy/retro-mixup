using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "New BeatData", menuName = "Custom/BeatData")]
public class BeatData : ScriptableObject
{
    public AudioClip music;
    public float beatBias = 20;
    public float beatStep = 0.35f;
    public AnimationCurve difficultyCurve = new();

    public void OnEnable()
    {
        if (difficultyCurve.length > 0 && difficultyCurve.keys[0].time != 0) 
            difficultyCurve.AddKey(0, difficultyCurve.keys[0].value);
        
        if (difficultyCurve.length > 1 && Math.Abs(difficultyCurve.keys[^1].time - music.length) > 0)
            difficultyCurve.AddKey(music.length, difficultyCurve.keys[^1].value);
    }
    
    [Button("Reset Curve")]
    public void ResetCurve()
    {
        difficultyCurve = new AnimationCurve();
        difficultyCurve.AddKey(0, 0);
        difficultyCurve.AddKey(music.length, 1);
    }
}
