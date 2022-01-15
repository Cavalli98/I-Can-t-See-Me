using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfRotateObject : Triggerable
{
    public float speed = 2.0f;
    public bool oneTime = false;
    public Transform pivot;
    public int dir;

    private bool _isMoving = false;
    private bool _stop = false;
    private Vector3 startRotation;
    private float _angleMove = 0;

    private void Awake()
    {
        startRotation = transform.eulerAngles;
    }

    public override void activate()
    {
        if (_stop) return;
        
        if(!_isMoving) {
            StartCoroutine(Rotate());
            if (oneTime) _stop = true;
        }
    }

    public override void activate(float f)
    {
        if (_stop) return;
        
        if(!_isMoving) {
            StartCoroutine(Rotate());
            if (oneTime) _stop = true;
        }
    }

    IEnumerator Rotate()
    {
        _isMoving = true;

        while (!loopDone())
        {
            transform.RotateAround(pivot.position, Vector3.forward, dir*speed*Time.deltaTime);
            yield return null;
        }

        _isMoving = false;
    }

    private bool loopDone()
    {
        _angleMove += speed*Time.deltaTime;
        if (Mathf.Abs(_angleMove - 360) < speed*Time.deltaTime)
        {
            _angleMove = 0;
            transform.eulerAngles = startRotation;
            return true;
        }
        else return false;
    }

}
