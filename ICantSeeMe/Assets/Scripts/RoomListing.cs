using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class RoomListing : MonoBehaviour
{
    public Text text;

    public string pass;

    public RoomInfo RoomInfo;

    public void Start()
    {
        pass = "";
    }

    public void SetRoomInfo(RoomInfo roomInfo)
    {
        RoomInfo = roomInfo;
        text.text = roomInfo.Name;
    }

    public void OnClick()
    {
        PhotonNetwork.JoinRoom(RoomInfo.Name);
    }
}
