using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerInteract : MonoBehaviourPun
{
    private bool isColliding = false;
    private GameObject colliderObj;

    // Start is called before the first frame update
    void Start()
    {
        isColliding = false;
    }

    void Update()
    {
        if (photonView.IsMine == false)
        {
            return;
        }

        if (isColliding)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                PhotonView.Get(colliderObj).RPC("trigger", RpcTarget.All, null);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (photonView.IsMine == false)
        {
            return;
        }

        if (collider.tag != "Trigger")
            return;
        
        isColliding = true;
        colliderObj = collider.gameObject;
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (photonView.IsMine == false)
        {
            return;
        }

        if (collider.tag != "Trigger")
            return;
        
        isColliding = false;
        colliderObj = null;
    }
}
