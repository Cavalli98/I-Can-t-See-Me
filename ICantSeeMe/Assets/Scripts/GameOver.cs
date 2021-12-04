using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameOver : MonoBehaviour
{
    private string currentLevel;
    
    public void Restart()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            currentLevel = (string)PhotonNetwork.CurrentRoom.CustomProperties["Level"];
            PhotonNetwork.LoadLevel(currentLevel);
        }
    }
}
