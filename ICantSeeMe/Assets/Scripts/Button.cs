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
        toActivate.GetComponent<Triggerable>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isColliding)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                PhotonView.Get(toActivate).RPC("activate", RpcTarget.All, null);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
       
        if (collider.tag != "Tool")
            return;
        Debug.Log("collisione con bottone");
        isColliding = true;
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag != "Tool")
            return;
        isColliding = false;
    }
}
