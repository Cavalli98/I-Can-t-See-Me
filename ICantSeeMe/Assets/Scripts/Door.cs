using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using Photon.Pun;

public class Door : Triggerable
{
    public Sprite open;
    public Sprite closed;
    private bool isTriggered = false;
    public bool isOpen;
    public bool isColliding = false;
    public bool oneTime = false;
    private bool _stop = false;
    //   public EventTrigger trigger;

    private void Awake()
    {
        isTriggered = false;
        isColliding = false;
        //   trigger = GetComponent<EventTrigger>();
        //  Lever.Trigger += Triggered;
    }
    // Start is called before the first frame update
    void Start()
    {
        if (isOpen)
        {
            GetComponent<SpriteRenderer>().sprite = open;
        }
        else if (!isOpen)
        {
            GetComponent<SpriteRenderer>().sprite = closed;
        }
    }
    public override void activate()
    {
        if (_stop)
            return;

        if (oneTime)
            _stop = true;

        if (!isOpen)
        {
            GetComponent<SpriteRenderer>().sprite = open;
            isOpen = true;
        }
        else if (isOpen)
        {
            GetComponent<SpriteRenderer>().sprite = closed;
            isOpen = false;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    [PunRPC]
    private void SetColliding(bool colliding)
    {
        isColliding = colliding;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player" && photonView.IsMine != true)
        {
            return;
        }
        photonView.RPC("SetColliding", RpcTarget.All, true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag != "Player" && photonView.IsMine != true)
        {
            return;
        }
        photonView.RPC("SetColliding", RpcTarget.All, false);
    }
}
