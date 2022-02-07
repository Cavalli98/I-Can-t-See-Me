using ExitGames.Client.Photon;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    private string currentLevel;
    public string strangerLevel;

    public Text Error;

    public void Restart()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            currentLevel = (string)PhotonNetwork.CurrentRoom.CustomProperties["Level"];
            PhotonNetwork.LoadLevel(currentLevel);
        }
        else
        {
            Error.gameObject.SetActive(true);
        }
    }

    public void RestartFromStrangerLevel()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Hashtable entries = new Hashtable { { "Level", strangerLevel } };
            PhotonNetwork.CurrentRoom.SetCustomProperties(entries);
            PhotonNetwork.LoadLevel(strangerLevel);
        }
        else
        {
            Error.gameObject.SetActive(true);
        }
    }
}
