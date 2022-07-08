using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakePiece : MonoBehaviour
{
    public readonly List<Marker> Markers = new();
    public float updateRate = .2f;

    private float _timer;
    [HideInInspector] public Vector3 initialScale;

    // Start is called before the first frame update
    void Start()
    {
        initialScale = transform.localScale;
        transform.localScale = initialScale * 0.8f;
        Markers.Add(new Marker(transform.position, transform.rotation));
    }

    // Update is called once per frame
    void Update()
    {
        if (_timer == 0)
        {
            _timer = updateRate;
            if (Markers.Count > 0) Markers.RemoveAt(0);
            Markers.Add(new Marker(transform.position, transform.rotation));
        }
        else _timer -= Time.deltaTime;
        
        transform.localScale = Vector3.Lerp(transform.localScale, initialScale, Time.deltaTime * 5);
    }

    public void ResetMarkers()
    {
        Markers.Clear();
        Markers.Add(new Marker(transform.position, transform.rotation));
    }

    public class Marker
    {
        public Vector3 Position;
        public Quaternion Rotation;

        public Marker(Vector3 position, Quaternion rotation)
        {
            Position = position;
            Rotation = rotation;
        }
    }
}
