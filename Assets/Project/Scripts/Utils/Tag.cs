using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tag : MonoBehaviour
{
    public string value;

    public bool Is(string value)
    {
        return this.value.Trim().ToLower().Equals(value.Trim().ToLower());
    }
    
    public bool Contains(string value)
    {
        return this.value.Trim().ToLower().Contains(value.Trim().ToLower());
    }
}
