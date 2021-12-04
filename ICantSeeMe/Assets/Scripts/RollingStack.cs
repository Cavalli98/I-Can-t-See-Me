using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RollingStack : MonoBehaviourPun
{
    private float startingX = 0f;
    // Start is called before the first frame update
    void Awake()
    {
        startingX = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            photonView.TransferOwnership(PhotonView.Get(collision.gameObject).Owner);
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        transform.rotation = Quaternion.Euler(Vector3.forward * (transform.position.x - startingX)*(-100));
    }
}
