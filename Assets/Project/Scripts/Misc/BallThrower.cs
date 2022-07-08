using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using Project.Scripts.Utils;
using UnityEngine;

public class BallThrower : MonoBehaviour
{
    public static BallThrower Instance;
    public PongBall ballPrefab;
    public Vector2 intervalRange = new(0.25f, 2.5f);
    public Vector2 spawnOffset;
    public Vector2 spawnSize;
    public Vector2 spawnVelocity;
    [ReadOnly] public float currentInterval;
    
    private float _spawnTimer;
    
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        _spawnTimer -= Time.deltaTime;
        
        var beatData = MusicManager.Instance.currentBeatData;
        var time = MusicManager.Instance.audioSource.time;
        currentInterval = MathUtils.Remap(beatData.difficultyCurve.Evaluate(time), 1f, 0f, intervalRange.x, intervalRange.y);

        if (_spawnTimer <= 0)
        {
            _spawnTimer = currentInterval;
            var pos = spawnOffset + new Vector2(Random.Range(-spawnSize.x / 2, spawnSize.x / 2), Random.Range(-spawnSize.y / 2, spawnSize.y / 2));
            var ball = Instantiate(ballPrefab, pos, Quaternion.identity);
            var vel = new Vector2(Random.Range(-1, 0), Random.Range(1, -1));
            ball.GetComponent<Rigidbody2D>().AddForce(vel * spawnVelocity, ForceMode2D.Impulse);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position + (Vector3)spawnOffset, spawnSize);
    }
}
