using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SelfActivateObject : Trigger
{
    public Transform triggerPoint;


    [PunRPC]
    public override void trigger()
    {        
        foreach (GameObject t in toActivate)
            t.GetComponent<Triggerable>().activate();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (collision.collider.OverlapPoint(triggerPoint.position))
            {
                this.photonView.RPC("trigger", RpcTarget.All, null);
            }
        }
    }


}
