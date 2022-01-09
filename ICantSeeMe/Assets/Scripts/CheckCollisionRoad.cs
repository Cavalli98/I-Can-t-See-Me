using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CheckCollisionRoad : MonoBehaviourPun
{

    public bool collided;
    public bool ritorna;
    // Start is called before the first frame update
    void Start()
    {
        collided = false;
        ritorna = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //Debug.Log("Entrato check");
            PhotonView.Get(transform.parent.gameObject).TransferOwnership(PhotonView.Get(collision.gameObject).Owner);
            collided = true;
        }
        else if (collision.gameObject.tag != "Border")
        {
            ritorna = true;
            //Debug.Log("collisione piattaforma-oggetto");
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {


    }
}
