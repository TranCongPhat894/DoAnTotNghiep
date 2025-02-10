using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject PauseMenu;
    public void pause()
    {
        PauseMenu.SetActive(true);
        Time.timeScale = 0;

    }
    public void resume()
    {
        PauseMenu.SetActive(false);
        Time.timeScale = 1;
    }
    public void Resume()
    {
        PauseMenu.SetActive(false);
        Time.timeScale = 1;
    }
}
