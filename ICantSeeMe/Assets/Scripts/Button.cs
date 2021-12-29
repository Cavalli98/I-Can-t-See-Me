using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Button : Trigger
{
    private bool _done;

    private void Awake()
    {
        _done = false;
    }

    [PunRPC]
    public override void trigger()
    {
        toActivate.GetComponent<Triggerable>().activate();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Tool")
            return;
        //     Debug.Log("collisione con bottone");
        if (!_done)
        {
            photonView.RPC("trigger", RpcTarget.All, null);
            _done = true;
        }
    }
}
