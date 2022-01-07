using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lock : Trigger
{
    private int hours;
    private int minutes;
    public GameObject LockPanel1;
    public GameObject LockPanel2;

    private void Awake()
    {
        // Set code for all
        if (PhotonNetwork.IsMasterClient)
        {
            SetCode();
        }
    }

    [PunRPC]
    public override void trigger()
    {
        
    }

    [PunRPC]
    private void SetCode()
    {
        hours = Random.Range(0, 11);
        minutes = Random.Range(0, 59);
    }
}
