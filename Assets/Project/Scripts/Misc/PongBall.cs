using System;
using System.Collections;
using System.Collections.Generic;
using Project.Scripts.Utils;
using UnityEngine;

public class PongBall : MonoBehaviour
{
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
        if (col.GetComponent<Tag>() != null)
        {
            if (col.HasTagThatContains("snake")) // use extension method to check whether we collided with snake
            {
                if (col.HasTag("snake body")) // check whether is was a body piece or not
                {
                    int count = SnakeController.Instance.DestroyBodyFrom(col.GetComponent<SnakePiece>()); // destroy snake pieces
                    CameraScript.Instance.Shake(0.01f * count, 0.1f); // shake camera with strength based on how many pieces were destroyed
                    AudioPlayer.Instance.Play(SFXHandler.Instance.GetSFX("player_hit")); // play hit sound effect
                } else AudioPlayer.Instance.Play(SFXHandler.Instance.GetSFX("player_hit_head"));
                Destroy(gameObject);
            }
            else if (col.HasTag("side")) // check whether ball has hit side of screen
            {
                Destroy(gameObject);
                LivesManager.Instance.UpdateLives(-1); // remove a life
                CameraScript.Instance.Shake(0.1f, 0.1f);
                AudioPlayer.Instance.Play(SFXHandler.Instance.GetSFX("wall_hit"));
            }
        }
    }
}
