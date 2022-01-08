using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Range : MonoBehaviour
{
    private Enemy _parent;
    private BoxCollider2D _box;
    private float _colliderDimension;


    private void Start()
    {
        _parent = GetComponentInParent<Enemy>();
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
            _parent.Target = collision.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player") {
            _parent.Target = null;
        }
    }
}
