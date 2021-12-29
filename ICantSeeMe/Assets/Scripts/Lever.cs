using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Lever : Trigger
{
    public Sprite activated;
    public Sprite deactivated;
    private bool isActive = false;
    //    public delegate void  OnTrigger();
    //    public static event OnTrigger Trigger;


    private void Awake()
    {
        isActive = false;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

   [PunRPC]
    public override void trigger()
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
}
