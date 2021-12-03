using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Photon;
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
        this.photonView.RPC("activate", RpcTarget.All,null);
        
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    [PunRPC]
    public new void activate()
    {
        toActivate.GetComponent<Triggerable>().activate();
    }
    // Update is called once per frame
    void Update()
    {
        if (isColliding )
        {
            if (Input.GetKeyDown(KeyCode.E) && !isActive)
            {
                GetComponent<SpriteRenderer>().sprite = activated;
                isActive = true;
             //   Trigger();
            }
            else if (Input.GetKeyDown(KeyCode.E) && isActive)
            {
                GetComponent<SpriteRenderer>().sprite = deactivated;
                isActive = false;
              //  Trigger();
            }
        }
    }

    private void OnTriggerEnter2D (Collider2D Player)
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
