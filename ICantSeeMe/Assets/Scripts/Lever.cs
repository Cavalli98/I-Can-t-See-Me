using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Lever : Trigger
{
    public Sprite activated;
    public Sprite deactivated;
    private bool isColliding = false;
    private bool isActive = false;
    //    public delegate void  OnTrigger();
    //    public static event OnTrigger Trigger;


    private void Awake()
    {
        isActive = false;
        isColliding = false;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    [PunRPC]
    public new void trigger()
    {
        Debug.Log("test");
        if (!isActive)
        {
            GetComponent<SpriteRenderer>().sprite = activated;
            isActive = true;
        }
        else if (isActive)
        {
            GetComponent<SpriteRenderer>().sprite = deactivated;
            isActive = false;
        }
        toActivate.GetComponent<Triggerable>().activate();
    }

    // Update is called once per frame
    void Update()
    {

        if (isColliding)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                photonView.RPC("trigger", RpcTarget.All, null);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D Player)
    {
        if (Player.tag != "Player")
            return;
        isColliding = true;
    }
    private void OnTriggerExit2D(Collider2D Player)
    {
        if (Player.tag != "Player")
            return;
        isColliding = false;
    }
}
