using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using ExitGames.Client.Photon;

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
    private InputField createRoomPassword;

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

    // Store the RoomNumber Key to avoid typos
    private const string roomNumberPrefKey = "RoomNumber";

    // Bool to check if player joining player is also the creator
    private bool _isCreator;

    // Room Number
    private string roomNumber;

    //Room pass
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
            uiManager.OnConnected();
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
        roomPass = createRoomPassword.text;
        // If room password field is not empty, creates a room
        if (!string.IsNullOrEmpty(roomPass))
        {
            _isCreator = true;
            roomNumber = Random.Range(1000, 9999).ToString();
            //string roomNumber = "1234";
            PlayerPrefs.SetString(roomNumberPrefKey, roomNumber);
            PhotonNetwork.CreateRoom(roomNumber + roomPass, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
        }
    }

    public void JoinRoom()
    {
        roomNumber = joinRoomNumber.text;
        roomPass = joinRoomPassword.text;
        // If room number and password fields are not empty, creates a room
        if (!string.IsNullOrEmpty(roomNumber) && !string.IsNullOrEmpty(roomPass))
        {
            _isCreator = false;
            PhotonNetwork.JoinRoom(roomNumber + roomPass);
        }
    }

    #endregion

    #region Private Methods

    private void SetRoomNumberProperty()
    {
        Hashtable entries = new Hashtable { { "RoomNumber", roomNumber } };
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
        if (_isConnecting)
        {
            // #Critical: The first we try to do is to join a potential existing room. If there is, good, else, we'll be called back with OnJoinRandomFailed()
            //PhotonNetwork.JoinRandomRoom();
            uiManager.OnConnected();
            _isConnecting = false;
        }
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

        //Try with another number
        CreateRoom();
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