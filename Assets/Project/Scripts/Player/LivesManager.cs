using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LivesManager : MonoBehaviour
{
    public static LivesManager Instance;
    public int initialLives = 3;

    public UnityEvent onLivesIncreased;
    public UnityEvent onLivesDecreased;
    
    private int _lives;
    
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateLives(int addition)
    {
        _lives += addition;
        if (addition > 0) onLivesIncreased.Invoke();
        else onLivesDecreased.Invoke();
    }
}
