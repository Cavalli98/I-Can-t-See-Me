using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;

public class EndLevel : MonoBehaviourPun
{
    public Door BoyExitDoor;
    public Door GirlExitDoor;
    public string NextLevel;
    private bool _done;

    // Start is called before the first frame update
    void Start()
    {
        _done = false;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (!_done && PhotonNetwork.IsMasterClient && BoyExitDoor.isOpen && BoyExitDoor.isColliding && GirlExitDoor.isOpen && GirlExitDoor.isColliding)
        {
            Hashtable entries = new Hashtable { { "Level", NextLevel } };
            PhotonNetwork.CurrentRoom.SetCustomProperties(entries);
            PhotonNetwork.LoadLevel(NextLevel);
            _done = true;
        }
    }

}
