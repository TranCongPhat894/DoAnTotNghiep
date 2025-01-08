using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
   public void playgame()
    {
        SceneManager.LoadSceneAsync(1);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void OpenLevel(int levelId)
    {
        LoadLevel(levelId); 
    }

  
    private void LoadLevel(int levelId)
    {
        if (levelId >= 0 && levelId < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(levelId); 
        }
        else
        {
            Debug.LogWarning("Scene index out of range: " + levelId);
        }
    }

}
