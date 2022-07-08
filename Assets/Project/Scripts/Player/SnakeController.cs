using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Project.Scripts.Player;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class SnakeController : MonoBehaviour, IPlayer
{
    public static SnakeController Instance;
    public InputActionAsset controlMap;
    public SnakePiece bodyPiece;
    public int initialBodyCount = 2;
    public float speed = 280f;
    public float turnSpeed = 180f;
    public float maxDistance = .1f;
    public int setSize = 100;
    public float bodyMarkerSpeed = 20;
    public float headSizeMultiplier = 1.1f;
    
    private readonly List<SnakePiece> _body = new();
    private float _distCount;
    private int _requests;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        _requests = initialBodyCount;
        _distCount = maxDistance;
        var head = Instantiate(bodyPiece, transform.position, transform.rotation, transform); // instantiate head piece
        head.transform.localScale *= headSizeMultiplier; // make snake head bigger than rest of body pieces
        head.initialScale = transform.localScale;
        head.AddComponent<Tag>().value = "snake head"; // add identifier tag
        _body.Add(head);
    }

    private void OnEnable()
    {
        controlMap.Enable();
    }
    
    private void OnDisable()
    {
        controlMap.Disable();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // sets velocity so snake is moving in direction it is facing
        _body[0].transform.GetComponent<Rigidbody2D>().velocity = _body[0].transform.right * Time.deltaTime * speed;
        var vel = controlMap["Move"].ReadValue<Vector2>(); // gets the keyboard controls from the action map
        if (vel.x != 0) _body[0].transform.Rotate(new Vector3(0, 0, vel.x * Time.deltaTime * -turnSpeed)); // rotates the snake piece based on vel
    }

    private void Update()
    {
        if (Keyboard.current.jKey.wasPressedThisFrame) _requests++; // for debugging purposes only - adds another snake piece to player

        // lerps every snake piece in list to the previous one
        for (int i = 1; i < _body.Count; i++)
        {
            if (_body[i].Markers.Count == 0) return;
            var prevPiece = _body[i - 1];
            _body[i].transform.position = Vector3.Lerp(_body[i].transform.position, prevPiece.Markers[0].Position, Time.deltaTime * bodyMarkerSpeed);
            _body[i].transform.rotation = Quaternion.Lerp(_body[i].transform.rotation, prevPiece.Markers[0].Rotation, Time.deltaTime * bodyMarkerSpeed);
        }
        
        // adds a small offset to spawning snake pieces so they don't spawn on top of each other - controlled by maxDistance
        _distCount -= Time.deltaTime;

        if (_distCount <= 0)
        {
            if (_requests > 0)
            {
                AddBodySet();
                _requests--;
            }
            _distCount = maxDistance;
        }
    }

    public int DestroyBodyFrom(SnakePiece piece)
    {
        int pieceIndex = _body.IndexOf(piece);
        int count = 0;
        List<SnakePiece> piecesToDestroy = new(); // we need this list to stop a concurrent modification exception in the main list
        for (int i = pieceIndex; i < _body.Count; i++)
        {
            piecesToDestroy.Add(_body[i]);
            count++; // keep a count of how many pieces we need to destroy so we can remove them from the list much easier
        }
        piecesToDestroy.ForEach(p => Destroy(p.gameObject)); // lambda expression to destroy each gameobject the snake piece belongs to
        _body.RemoveRange(pieceIndex, count);
        //ScoreManager.Instance.UpdateScore(-count);
        return count;
    }
    
    public void AddBodySet()
    {
        // adds a body piece depending on how many pieces are in a set (how many we want to spawn at once)
        // this can be useful for different food sizes being worth more pieces
        for (int i = 0; i < setSize; i++)
        {
            CreateBodyPiece();
        }
    }

    private void CreateBodyPiece()
    {
        var lastPiece = _body[^1];
        lastPiece.ResetMarkers();
        var body = Instantiate(bodyPiece, lastPiece.Markers[0].Position, lastPiece.Markers[0].Rotation, transform);
        body.AddComponent<Tag>().value = "snake body"; // gives the gameobject an identifier using the custom tag system 
        Destroy(body.GetComponent<Rigidbody2D>());
        _body.Add(body);
        StartCoroutine(RippleAnimation());
    }

    // creates a ripple animation by looping through each snake piece and setting its scale with an offset of 0.02 seconds
    private IEnumerator RippleAnimation()
    {
        int index = 0;
        while (index < _body.Count)
        {
            _body[index].transform.localScale = _body[index].initialScale * 1.25f;
            yield return new WaitForSeconds(0.02f);
            index++;
        }      
    }

    public void OnDamage(GameObject cause)
    {
        throw new NotImplementedException();
    }
}
