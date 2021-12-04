using UnityEngine;
using Photon.Pun;

public class EndLevel : MonoBehaviourPun
{
    public Door BoyExitDoor;
    public Door GirlExitDoor;
    public string NextLevel;

    // Start is called before the first frame update
    void Start()
    {

    }
    
    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.IsMasterClient && BoyExitDoor.isOpen && BoyExitDoor.isColliding && GirlExitDoor.isOpen && GirlExitDoor.isColliding)
        {
            PhotonNetwork.LoadLevel(NextLevel);
        }
    }

}
