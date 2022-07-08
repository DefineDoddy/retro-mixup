using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    public int value;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.GetComponent<Tag>() != null && col.GetComponent<Tag>().Is("snake head"))
        {
            SnakeController.Instance.AddBodySet();
            FoodSpawner.Instance.RemoveFood(this);
            ScoreManager.Instance.UpdateScore(value);
            AudioPlayer.Instance.Play(SFXHandler.Instance.GetSFX("player_collect"));
            Destroy(gameObject);
            
        }
    }
}
