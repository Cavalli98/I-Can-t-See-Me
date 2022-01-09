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
        //Debug.Log(isColliding);
        if (photonView.IsMine == false)
        {
            return;
        }

        if (isColliding)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                print("Send RPC");
                PhotonView.Get(colliderObj).RPC("trigger", RpcTarget.All, null);
            }
        }
    }

    //private void OnTriggerEnter2D(Collider2D collider)
    //{

    //    Debug.Log(collider);
    //    if (photonView.IsMine == false)
    //    {
    //        return;
    //    }

    //    if (collider.tag != "Trigger")
    //    {
    //        return;
    //    }
    //    isColliding = true;
    //    colliderObj = collider.gameObject;
    //}

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (photonView.IsMine == false)
        {
            return;
        }

        if (collider.tag != "Trigger")
        {
            return;
        }
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (photonView.IsMine == false)
            return;

        if (collision.gameObject.tag != "Trigger")
            return;

        print("player enter coll");
        isColliding = true;
        colliderObj = collision.gameObject;
    }

    private void OnCollisionExit2D(Collision2D collision)
    { 
        if (photonView.IsMine == false)
            return;

        if (collision.gameObject.tag != "Trigger")
            return;

        print("player exit coll");
        isColliding = false;
        colliderObj = null;
    }
}
