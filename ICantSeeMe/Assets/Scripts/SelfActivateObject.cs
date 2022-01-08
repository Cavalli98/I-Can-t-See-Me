using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfActivateObject : Trigger
{
    public Transform triggerPoint;

    void Start()
    {
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (collision.collider.OverlapPoint(triggerPoint.position))
            {
                foreach (GameObject t in toActivate)
                    t.GetComponent<Triggerable>().activate();
            }
        }
    }


}
