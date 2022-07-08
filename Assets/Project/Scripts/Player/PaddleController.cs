using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PaddleController : MonoBehaviour
{
    public float speed = 10;

    private void Start()
    {
        
    }
    
    private void Update()
    {
        var vertical = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()).y;
        transform.position = Vector2.Lerp(transform.position, new Vector2(transform.position.x, vertical), speed * Time.deltaTime);
    }
}
