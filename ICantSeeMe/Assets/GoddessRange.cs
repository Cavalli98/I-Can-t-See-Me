using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GoddessRange : MonoBehaviourPun
{
    private Goddess _parent;
    private BoxCollider2D _box;
    private float _colliderDimension;


    private void Start()
    {
        _parent = GetComponentInParent<Goddess>();
        _box = GetComponent<BoxCollider2D>();
        _colliderDimension = _box.size.x;
    }

    public void setColliderOffset(int dir)
    {
        _box.offset = new Vector2(dir*_box.size.x/2, 0);
    }

    public void setColliderSize(float size)
    {
        _box.size = new Vector2(size, _box.size.y);
    }

    public void setOffsetAndSize(float size, int dir)
    {
        _box.offset = new Vector2(dir*size/2, 0);
        _box.size = new Vector2(size, _box.size.y);
    }

    public float getXScale()
    {
        return transform.localScale.x;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player") {
            this.photonView.RPC("setParent", RpcTarget.All, collision.gameObject.name);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            this.photonView.RPC("setParent", RpcTarget.All, "null");
    }

    [PunRPC]
    void setParent(string playerName)
    {
        if (playerName == "null") {
            _parent.Target = null;
        }
        else {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject p in players)
            {
                if (p.name == playerName)
                    _parent.Target = p.transform;
            }
            
            print("Goddess range, target name: "+_parent.Target.gameObject.name);
        }
    }
}
