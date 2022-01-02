using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CheckCollisionRoad : MonoBehaviourPun
{

    public bool collided;
    // Start is called before the first frame update
    void Start()
    {
        collided = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Entrato check");
        if (collision.gameObject.tag == "Player")
        {
            photonView.TransferOwnership(PhotonView.Get(collision.gameObject).Owner);
            collided = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {


    }
}
