using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum SpawnModes
{
    Fixed,
    Random
}

public class Spawner : MonoBehaviour
{
    public static Action OnWaveCompleted;

    [Header("Settings")]
    [SerializeField] private SpawnModes spawnMode = SpawnModes.Fixed;
    [SerializeField] private int enemyCount = 10;
    [SerializeField] private float delayBtwWaves = 1f;

    [Header("Fixed Delay")]
    [SerializeField] private float delayBtwSpawns;

    [Header("Random Delay")]
    [SerializeField] private float minRandomDelay;
    [SerializeField] private float maxRandomDelay;

    [Header("Poolers")]
    [SerializeField] private ObjectPooler enemyWave2;
    [SerializeField] private ObjectPooler enemyWave4;
    [SerializeField] private ObjectPooler enemyWave6;
    [SerializeField] private ObjectPooler enemyWave8;
    [SerializeField] private ObjectPooler enemyWave10;

    private float _spawnTimer;
    private int _enemiesSpawned;
    private int _enemiesRamaining;

    private Waypoint _waypoint;
    private List<ObjectPooler> poolers;
    private void Start()
    {
        _waypoint = GetComponent<Waypoint>();

        _enemiesRamaining = enemyCount;
        poolers = new List<ObjectPooler> { enemyWave2, enemyWave4, enemyWave6, enemyWave8, enemyWave10 };
    }

    private void Update()
    {
        _spawnTimer -= Time.deltaTime;
        if (_spawnTimer < 0)
        {
            _spawnTimer = GetSpawnDelay();
            if (_enemiesSpawned < enemyCount)
            {
                _enemiesSpawned++;
                SpawnEnemy();
            }
        }
    }

    private void SpawnEnemy()
    {
        GameObject newInstance = GetRandomPooler().GetInstanceFromPool(); // Sử dụng pooler ngẫu nhiên
        Enemy enemy = newInstance.GetComponent<Enemy>();
        enemy.Waypoint = _waypoint;
        enemy.ResetEnemy();

        enemy.transform.localPosition = transform.position;
        newInstance.SetActive(true);
    }


    private float GetSpawnDelay()
    {
        float delay = 0f;
        if (spawnMode == SpawnModes.Fixed)
        {
            delay = delayBtwSpawns;
        }
        else
        {
            delay = GetRandomDelay();
        }

        return delay;
    }

    private float GetRandomDelay()
    {
        float randomTimer = Random.Range(minRandomDelay, maxRandomDelay);
        return randomTimer;
    }

    private ObjectPooler GetRandomPooler()
    {
        int currentWave = LevelManager.Instance.CurrentWave;

        // Nếu bạn muốn spawn ngẫu nhiên một loại quái trong phạm vi wave, hãy chọn ngẫu nhiên trong các pooler
        if (currentWave <= 2) // 1- 10
        {
            return poolers[Random.Range(0,0)]; // Chọn ngẫu nhiên giữa enemyWave2 hoặc enemyWave4
        }
        if (currentWave > 2 && currentWave <= 4) // 11- 20
        {
            return poolers[Random.Range(0, 3)]; // Chọn ngẫu nhiên giữa enemyWave4 và enemyWave6
        }
        if (currentWave > 4 && currentWave <= 6) // 21- 30
        {
            return poolers[Random.Range(1, 4)]; // Chọn ngẫu nhiên giữa enemyWave6 và enemyWave8
        }
        if (currentWave > 6 && currentWave <= 8) // 31- 40
        {
            return poolers[Random.Range(2, 5)]; // Chọn ngẫu nhiên giữa enemyWave8 và enemyWave10
        }
        if (currentWave > 8 && currentWave <= 10) // 41- 50
        {
            return poolers[Random.Range(3, poolers.Count)]; // Chọn ngẫu nhiên bất kỳ pooler nào
        }

        return null;
    }
    private void UpdateEnemyCount()
    {
        // Tăng số lượng quái tùy theo wave
        _enemiesRamaining = enemyCount + (LevelManager.Instance.CurrentWave * 2);  // Ví dụ tăng 2 quái cho mỗi wave
    }

    private IEnumerator NextWave()
    {
        yield return new WaitForSeconds(delayBtwWaves);
        UpdateEnemyCount();  // Cập nhật số lượng quái cho wave tiếp theo
        _enemiesRamaining = enemyCount;
        _spawnTimer = 0f;
        _enemiesSpawned = 0;
    }

    private void RecordEnemy(Enemy enemy)
    {
        _enemiesRamaining--;
        if (_enemiesRamaining <= 0)
        {
            OnWaveCompleted?.Invoke();
            StartCoroutine(NextWave());
        }
    }

    private void OnEnable()
    {
        Enemy.OnEndReached += RecordEnemy;
        EnemyHealth.OnEnemyKilled += RecordEnemy;
    }

    private void OnDisable()
    {
        Enemy.OnEndReached -= RecordEnemy;
        EnemyHealth.OnEnemyKilled -= RecordEnemy;
    }
}
