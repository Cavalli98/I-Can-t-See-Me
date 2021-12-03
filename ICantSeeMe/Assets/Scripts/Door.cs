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
    private bool isOpen = false;
 //   public EventTrigger trigger;
  
    private void Awake()
    {
        isOpen = false;
        isTriggered = false;
     //   trigger = GetComponent<EventTrigger>();
      //  Lever.Trigger += Triggered;
    }
    // Start is called before the first frame update
    void Start()
    {


    }
    public new void activate()
    {
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

}
