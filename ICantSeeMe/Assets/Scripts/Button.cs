using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Button : Trigger
{
    private bool isColliding = false;
    // Start is called before the first frame update
    void Start()
    {
        isColliding = false;
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
        Debug.Log("collisione con bottone");
        isColliding = true;
        photonView.RPC("trigger", RpcTarget.All, null);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Tool")
            return;
        isColliding = false;
    }
}
