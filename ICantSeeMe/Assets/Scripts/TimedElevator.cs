using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TimedElevator : Triggerable
{
    private float timePassed = 0;

    public float endY;
    private float speed = 2.0f;
    private int _slowingFactor = 5;
    private int _wait = 2;
    private bool _waiting = false;

    private bool _hasToMove;
    private bool _reachedEnd;
    private Vector3 _endPosition;
    private Vector3 _startPosition;


    private void Awake()
    {
        _hasToMove = false;
        _reachedEnd = false;
        _startPosition = transform.position;
        _endPosition = new Vector3(transform.position.x, endY);
    }

    private void FixedUpdate()
    {
        if (_hasToMove)
        {
            if (!_reachedEnd)
                transform.position = Vector2.MoveTowards(transform.position, _endPosition, speed*Time.deltaTime);
            else if (_waiting)
                StartCoroutine(waiter());
            else
                transform.position = Vector2.MoveTowards(transform.position, _startPosition, speed/_slowingFactor*Time.deltaTime);

            if (transform.position==_endPosition) {
                _reachedEnd = true;
                _waiting = true;
            }
            else if (transform.position==_startPosition) {
                _reachedEnd = false;
                _hasToMove = false;
            }
        }
    }

    public override void activate()
    {
        _hasToMove = true;
    }

    public override void activate(float duration)
    {
        _hasToMove = true;
        float distance = Vector3.Distance(_startPosition, _endPosition);
        speed = distance/((duration - _wait)/(1+_slowingFactor));
    }

    IEnumerator waiter()
    {
        yield return new WaitForSeconds(_wait);
        _waiting = false;
    }

    // to print stuff every s seconds
    private void PrintStuff(float s)
    {
        timePassed += Time.deltaTime;
        if(timePassed > s)
        {
            print("_startPosition= "+_startPosition+" transform.position= "+transform.position);
            timePassed=0f;
        } 
    }
}
