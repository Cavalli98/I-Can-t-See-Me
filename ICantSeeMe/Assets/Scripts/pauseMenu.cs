using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class pauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;
    public GameObject settingsMenuUI;

    public string level;
    public string launcher;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }

    }
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    void Load(string lvl)
    {
        Time.timeScale = 1f;
        Hashtable entries = new Hashtable { { "Level", lvl } };
        PhotonNetwork.CurrentRoom.SetCustomProperties(entries);
        PhotonNetwork.LoadLevel(level);
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        Load(level);
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        Load(launcher);
    }
    public void Save()
    {
        Debug.Log("Saving");
    }
    public void Settings()
    {
        pauseMenuUI.SetActive(false);
        settingsMenuUI.SetActive(true);
    }
    public void PauseMeu()
    {
        settingsMenuUI.SetActive(false);
        pauseMenuUI.SetActive(true);
    }
}
