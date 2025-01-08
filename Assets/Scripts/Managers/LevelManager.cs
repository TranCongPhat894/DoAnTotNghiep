using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private int lives = 10;

    public int TotalLives { get; set; }
    public int CurrentWave { get; set; }
    
    private void Start()
    {
        TotalLives = lives;
        CurrentWave = 1;
    }

    private void ReduceLives(Enemy enemy)
    {
        TotalLives--;
        if (TotalLives <= 0)
        {
            TotalLives = 0;
            GameOver();
        }
    }

    private void GameOver()
    {
        // Dừng thời gian trò chơi
        Time.timeScale = 0;
        // hiện bảng game over
        UIManager.Instance.ShowGameOverPanel();
    }
    private IEnumerator GameWonCoroutine()
    {
        // Chờ 1 giây
        yield return new WaitForSeconds(1f);

        // Dừng thời gian trò chơi
        Time.timeScale = 0;

        // Hiển thị bảng thông báo chiến thắng
       
    }
    private void GameWon()
    {
        StartCoroutine(GameWonCoroutine());
       

        UIManager.Instance.ShowWinGamePanel();
    }
    private void WaveCompleted()
    {
        if (CurrentWave >= 6)
        {
            // Đã hoàn thành wave 10, kết thúc trò chơi
            
            GameWon();
            return;
        }
        CurrentWave++;
        AchievementManager.Instance.AddProgress("Waves10", 1);
        AchievementManager.Instance.AddProgress("Waves20", 1);
        AchievementManager.Instance.AddProgress("Waves50", 1);
        AchievementManager.Instance.AddProgress("Waves100", 1);
    }
    
    private void OnEnable()
    {
        Enemy.OnEndReached += ReduceLives;
        Spawner.OnWaveCompleted += WaveCompleted;
    }

    private void OnDisable()
    {
        Enemy.OnEndReached -= ReduceLives;
        Spawner.OnWaveCompleted -= WaveCompleted;
    }
}
