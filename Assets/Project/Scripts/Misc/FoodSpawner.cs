using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class FoodSpawner : MonoBehaviour
{
    public static FoodSpawner Instance;
    public Food foodPrefab;
    public Vector2 spawnRateTo;
    public float spawnRateProgress;
    public float startDelay;
    public Vector2 spawnSize;
    public int maxFood;
    
    private float _spawnRate;
    private float _spawnRateTimer;
    private readonly List<Food> _foods = new();
    
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        StartCoroutine(LerpSpawnRate());
    }

    // Update is called once per frame
    void Update()
    {
        _spawnRateTimer -= Time.deltaTime;

        if (_spawnRateTimer <= 0 && _foods.Count < maxFood)
        {
            _spawnRateTimer = _spawnRate;
            var pos = new Vector2(Random.Range(-spawnSize.x / 2, spawnSize.x / 2), Random.Range(-spawnSize.y / 2, spawnSize.y / 2));
           _foods.Add(Instantiate(foodPrefab, pos, Quaternion.identity));
        }
    }

    public void RemoveFood(Food food)
    {
        _foods.Remove(food);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, spawnSize);
    }
    
    private IEnumerator LerpSpawnRate()
    {
        _spawnRate = 0;
        yield return new WaitForSeconds(startDelay);
        _spawnRate = spawnRateTo.x;
        
        for (float f = 0; f <= spawnRateProgress; f += Time.deltaTime) {
            _spawnRate = Mathf.Lerp(spawnRateTo.x, spawnRateTo.y, f / spawnRateProgress);
            yield return null;
        }
    }
}
