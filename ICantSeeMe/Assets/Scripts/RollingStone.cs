using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;

public class RollingStone : MonoBehaviourPun
{
    private GameObject[] _toIgnore;
    private CircleCollider2D _coll;
    private Rigidbody2D _rb;
    public const byte gameOverEvent = 1;
    private float startingX = 0f;

    void Start()
    {
        startingX = transform.position.x;
        _coll = GetComponent<CircleCollider2D>();
        _rb = GetComponent<Rigidbody2D>();
        _toIgnore = GameObject.FindGameObjectsWithTag("SpringButton");
        foreach(GameObject obj in _toIgnore)
            Physics2D.IgnoreCollision(obj.GetComponent<Collider2D>(), _coll);
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            photonView.TransferOwnership(PhotonView.Get(collision.gameObject).Owner);
        }

        if (collision.gameObject.tag == "Player" &&
            (Mathf.Abs(_rb.velocity.x) > 1.5f || 
            Mathf.Abs(_rb.velocity.y) > 1.5f) && 
            (Mathf.Abs(collision.relativeVelocity.x) > 3.0f || 
            Mathf.Abs(collision.relativeVelocity.y) > 3.0f))
        {
            print("vel: "+collision.relativeVelocity);
            // Send event to all players
            object[] content = null;
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All }; // You would have to set the Receivers to All in order to receive this event on the local client as well
            PhotonNetwork.RaiseEvent(gameOverEvent, content, raiseEventOptions, SendOptions.SendReliable);
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        transform.rotation = Quaternion.Euler(Vector3.forward * (transform.position.x - startingX)*(-100));
    }
}
