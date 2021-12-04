using System.Collections;
using System.Collections.Generic;
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
        BoyExitDoor.GetComponent<Door>();
        GirlExitDoor.GetComponent<Door>();
    }

    // Update is called once per frame
    void Update()
    {
        if (BoyExitDoor.isOpen && BoyExitDoor.isColliding && GirlExitDoor.isOpen && GirlExitDoor.isColliding)
        {
            PhotonNetwork.LoadLevel(NextLevel);
        }
    }

}
