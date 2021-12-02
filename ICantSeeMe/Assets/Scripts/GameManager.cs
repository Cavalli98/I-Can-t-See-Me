using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;


public class GameManager : MonoBehaviourPunCallbacks
{
    public GameObject playerBoy;
    public GameObject playerGirl;

    #region Private Methods
    private void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {
            return;
        }
        Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManager.GetActiveScene());
        if (PhotonNetwork.IsMasterClient)
        {
            // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
            GameObject player = PhotonNetwork.Instantiate(this.playerBoy.name, new Vector3(-7f, 2f, 0f), Quaternion.identity, 0);
            Ghost.instance.transform.position = new Vector3(13f, 2f, 0f);
            Ghost.instance.transform.SetParent(player.transform);
        }
        else
        {
            GameObject player = PhotonNetwork.Instantiate(this.playerGirl.name, new Vector3(13f, 2f, 0f), Quaternion.identity, 0);
            Ghost.instance.transform.position = new Vector3(-7, 2f, 0f);
            Ghost.instance.transform.SetParent(player.transform);
        }
    }

    private void ReturnToLobby()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
        }
        Debug.LogFormat("PhotonNetwork : Loading Lobby");
        PhotonNetwork.LoadLevel("Lobby");
    }    

    #endregion

    #region Photon Callbacks

    /// <summary>
    /// Called when the local player left the room. We need to load the launcher scene.
    /// </summary>
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }

    public override void OnPlayerLeftRoom(Player other)
    {
        Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects


        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom


            ReturnToLobby();
        }
    }

    #endregion


    #region Public Methods


    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }


    #endregion
}
