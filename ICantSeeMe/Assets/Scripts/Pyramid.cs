using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;

public class Pyramid : Trigger
{
    public int face;
    private float _rotation;
    private float _increment;
    private bool _hasToRotate;

    public PyramidTrigger father;

    public void Awake()
    {
        face = 0;
        _increment = 1f;
        _rotation = 0;
        _hasToRotate = false;
    }

    // Start is called before the first frame update
    void Start()
    {

    }
    public void Update()
    {
        if (_hasToRotate)
        {
            rotate();
        }
    }

    [PunRPC]
    public override void trigger()
    {
        _hasToRotate = true;
        //Debug.Log("piramide attivata");
    }

    [PunRPC]
    void rotate()
    {
        _rotation += _increment;
        if (_rotation < 120)
        {
            transform.RotateAround(transform.position, Vector3.up, _increment);
        }
        else
        {
            _rotation = 0;
            _hasToRotate = false;
            face++;
            face %= 3;
            father.Check();
            //Debug.Log(face);
        }
    }
}
