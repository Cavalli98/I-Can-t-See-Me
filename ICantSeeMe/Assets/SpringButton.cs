using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpringButton : Trigger
{
    private bool _isTriggered = false;
    private GameObject child;
    private Vector2[] _pointsToOverlap;
    private BoxCollider2D _box;

    void Start()
    {
        _box = GetComponent<BoxCollider2D>();
        child = transform.Find("SpringButton").gameObject;
        float hSizeX = _box.size.x/2;
        float hSizeY = _box.size.y/2;
        Vector2 center = _box.transform.position;
        Vector2 topLeft = new Vector2(center.x-hSizeX+0.05f, center.y+hSizeY+0.06f);
        Vector2 topCenter = new Vector2(center.x, center.y+hSizeY+0.06f);
        Vector2 topRight = new Vector2(center.x+hSizeX-0.05f, center.y+0.06f);
        
        _pointsToOverlap = new Vector2[] {
            topLeft,
            topCenter,
            topRight
        };
    }
 

    [PunRPC]
    public override void trigger()
    {
        
        if (!_isTriggered) {
            _isTriggered = true;
            child.transform.localScale = new Vector3(child.transform.localScale.x, 0.45f, child.transform.localScale.z);
            child.transform.position = new Vector3(child.transform.position.x, child.transform.position.y- 0.175f, child.transform.position.z);

            foreach (GameObject t in toActivate)
                t.GetComponent<Triggerable>().activate();

        } else if (_isTriggered) {
            _isTriggered = false;
            child.transform.localScale = new Vector3(child.transform.localScale.x, 0.75f, child.transform.localScale.z);
            child.transform.position = new Vector3(child.transform.position.x, child.transform.position .y+ 0.175f, child.transform.position.z);

            foreach (GameObject t in toActivate)
                t.GetComponent<Triggerable>().activate();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && !_isTriggered)
        {
            foreach (Vector2 point in _pointsToOverlap) {
                if (collision.OverlapPoint(point))
                {
                    // Collided with a surface facing mostly upwards
                    PhotonView.Get(this).RPC("trigger", RpcTarget.All, null);
                    break;
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") 
        {
            if (_isTriggered) {
                PhotonView.Get(this).RPC("trigger", RpcTarget.All, null);
            }
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && !_isTriggered)
        {
            foreach (Vector2 point in _pointsToOverlap) {
                if (collision.OverlapPoint(point))
                {
                    // Collided with a surface facing mostly upwards
                    PhotonView.Get(this).RPC("trigger", RpcTarget.All, null);
                    break;
                }
            }
        }
    }
}
