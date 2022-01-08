using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;
using UnityEngine.Audio;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class pauseMenu : MonoBehaviourPun
{
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;
    public GameObject settingsMenuUI;

    public string level;
    public string launcher;

    public AudioMixer audioMixer;
    public Dropdown resolutionDropdown;
    Resolution[] resolutions;

    private void Start()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if(resolutions[i].width == Screen.currentResolution.width && resolutions[i].height==Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

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
        //Time.timeScale = 0f;
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
        PhotonNetwork.LeaveRoom();
        //Load(launcher);
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
    public void SetMusicVolume(float volume)
    {
        //Debug.Log(volume);
        audioMixer.SetFloat("MusicVolume", volume);
    }
    public void SetSoundsVolume(float volume)
    {
        //Debug.Log(volume);
        audioMixer.SetFloat("SoundsVolume", volume);
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width,resolution.height, Screen.fullScreen);
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }
}
