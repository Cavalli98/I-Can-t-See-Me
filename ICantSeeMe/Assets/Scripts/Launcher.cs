using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using ExitGames.Client.Photon;
using System.Collections.Generic;

public class Launcher : MonoBehaviourPunCallbacks
{
    #region Private Serializable Fields

    /// <summary>
    /// The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created.
    /// </summary>
    [Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created")]
    [SerializeField]
    private byte maxPlayersPerRoom = 2;

    [Tooltip("The Ui Panel to let the user enter name, connect and play")]
    [SerializeField]
    private GameObject controlPanel;

    [Tooltip("The UI Label to inform the user that the connection is in progress")]
    [SerializeField]
    private GameObject progressLabel;

    [SerializeField]
    private InputField joinRoomNumber;

    [SerializeField]
    private InputField joinRoomPassword;

    [SerializeField]
    private InputField createRoomName;

    [SerializeField]
    private InputField createRoomPassword;

    [SerializeField]
    private Text ErrorText;

    [SerializeField]
    private UIManager uiManager;
    #endregion


    #region Private Fields

    /// <summary>
    /// This client's version number. Users are separated from each other by gameVersion (which allows you to make breaking changes).
    /// </summary>
    private string gameVersion = "1";

    /// <summary>
    /// Keep track of the current process. Since connection is asynchronous and is based on several callbacks from Photon,
    /// we need to keep track of this to properly adjust the behavior when we receive call back by Photon.
    /// Typically this is used for the OnConnectedToMaster() callback.
    /// </summary>
    private bool _isConnecting;

    // Bool to check if player joining player is also the creator
    private bool _isCreator;

    // Room Name
    private string roomName;

    //Room Pass
    private string roomPass;

    #endregion

    #region MonoBehaviour CallBacks

    /// <summary>
    /// MonoBehaviour method called on GameObject by Unity during early initialization phase.
    /// </summary>
    void Awake()
    {
        // #Critical
        // this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
        PhotonNetwork.AutomaticallySyncScene = true;
        _isCreator = false;
        ErrorText.text = "";
    }

    /// <summary>
    /// MonoBehaviour method called on GameObject by Unity during initialization phase.
    /// </summary>
    void Start()
    {
           
    }

    #endregion


    #region Public Methods

    /// <summary>
    /// Start the connection process.
    /// - If already connected, we attempt joining a random room
    /// - if not yet connected, Connect this application instance to Photon Cloud Network
    /// </summary>
    //public void Connect()
    //{
    //    // we check if we are connected or not, we join if we are , else we initiate the connection to the server.
    //    if (PhotonNetwork.IsConnected)
    //    {
    //        // #Critical we need at this point to attempt joining a Random Room. If it fails, we'll get notified in OnJoinRandomFailed() and we'll create one.
    //        PhotonNetwork.JoinRandomRoom();
    //    }
    //    else
    //    {
    //        // #Critical, we must first and foremost connect to Photon Online Server.
    //        isConnecting  = PhotonNetwork.ConnectUsingSettings();
    //        PhotonNetwork.GameVersion = gameVersion;
    //    }
    //}

    public void Connect()
    {
        if (PhotonNetwork.IsConnected)
        {
            // Switch from control panel to progress label
            uiManager.PlayButton();
        }
        else
        {
            //# Critical, we must first and foremost connect to Photon Online Server.
            _isConnecting = PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = gameVersion;
        }
    }

    public void CreateRoom()
    {
        roomName = createRoomName.text;
        roomPass = createRoomPassword.text;

        if (string.IsNullOrEmpty(roomName))
        {
            ErrorText.text = "Your room must have a name";
        }
        else
        {
            _isCreator = true;
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = maxPlayersPerRoom;
            PhotonNetwork.CreateRoom(roomName, roomOptions);
        }
    }

    public void JoinRoom()
    {
        roomName = joinRoomNumber.text;
        roomPass = joinRoomPassword.text;
        // If room number and password fields are not empty, creates a room
        if (!string.IsNullOrEmpty(roomName) && !string.IsNullOrEmpty(roomPass))
        {
            _isCreator = false;
            PhotonNetwork.JoinRoom(roomName + roomPass);
        }
    }

    #endregion

    #region Private Methods

    private void SetRoomNumberProperty()
    {
        Hashtable entries = new Hashtable { { "RoomNumber", roomName } };
        PhotonNetwork.CurrentRoom.SetCustomProperties(entries);
    }

    #endregion

    #region MonoBehaviourPunCallbacks Callbacks

    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster() was called by PUN");
        // we don't want to do anything if we are not attempting to join a room.
        // this case where isConnecting is false is typically when you lost or quit the game, when this level is loaded, OnConnectedToMaster will be called, in that case
        // we don't want to do anything.
        //if (_isConnecting)
        //{
        //    print("First time connecting to lobby");
        //    PhotonNetwork.JoinLobby();
        //    // #Critical: The first we try to do is to join a potential existing room. If there is, good, else, we'll be called back with OnJoinRandomFailed()
        //    //PhotonNetwork.JoinRandomRoom();
        //}
        //else
        //{
        //    print("Already connected to lobby");
        //    PhotonNetwork.JoinLobby();
        //}
        print("Connecting to lobby");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        print("OnJoinedLobby was called by PUN");
        //if (_isConnecting)
        //{
        //    _isConnecting = false;
        //} 
        uiManager.OnConnected();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        _isConnecting = false;
        progressLabel.SetActive(false);
        controlPanel.SetActive(true);
        Debug.LogWarningFormat("OnDisconnected() was called by PUN with reason {0}", cause);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("OnCreateRoomFailed(). Name already exists. Generating other number...");

        ErrorText.text = "Room name already in use.Try another name.";
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("OnJoinRoomFailed(). Name already exists. Generating other number...");

        if (!_isCreator)
        {
            //TODO: show error message
            Debug.Log("Room number or password is wrong");
        }
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom() called by PUN. Now this client is in a room.");

        // #Critical: We only load if we are the first player, else we rely on `PhotonNetwork.AutomaticallySyncScene` to sync our instance scene.
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            SetRoomNumberProperty();
            Debug.Log("Load the lobby");
            // Load the lobby.
            PhotonNetwork.LoadLevel("Lobby");
        }
    }

    #endregion
}