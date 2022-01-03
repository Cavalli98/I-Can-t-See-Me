using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

/*
public class ChainBridge : Triggerable
{
    public bool isUp;
    public float startY;
    public float endY;
    public float speed;

    private bool hasToMoveUp;
    private bool _isMoving;
    private float step;
    private Vector3 endPosition;
    private Vector3 startPosition;

    private void Awake()
    {
        _isMoving = false;
        hasToMoveUp = isUp;
        startPosition = new Vector3(transform.position.x, startY);
        endPosition = new Vector3(transform.position.x, endY);
        step = speed * Time.deltaTime; // calculate distance to move
    }

    // Update is called once per frame
    void Update()
    {
        if (_isMoving)
        {
            // Se isUp prima era true
            if (hasToMoveUp)
            {
                // Move our position a step closer to the target.
                transform.position = Vector3.MoveTowards(transform.position, endPosition, step);

                if (Vector3.Distance(transform.position, endPosition) < 0.01f)
                {
                    _isMoving = false;
                }
            }
            else
            {
                // Move our position a step closer to the target.
                transform.position = Vector3.MoveTowards(transform.position, startPosition, step);

                if (Vector3.Distance(transform.position, startPosition) < 0.01f)
                {
                    _isMoving = false;
                }
            }
        }
    }

    public override void activate()
    {
        hasToMoveUp = !hasToMoveUp;
        _isMoving = true;
    }
} */

public class ChainBridge : Triggerable
{
    //public float startY;
    //public float endY;
    public float speed;

    private bool _hasToMove;
    private float _t;
    public Transform startObj;
    public Transform endObj;
    private Vector3 endPosition;
    private Vector3 startPosition;


    private void Awake()
    {
        _hasToMove = false;
        //startPosition = new Vector3(transform.position.x, startY);
        //endPosition = new Vector3(transform.position.x, endY);
        startPosition = startObj.position;
        endPosition = endObj.position;
        transform.position = startPosition;
        //Debug.Log("x: " + transform.position.x);
    }

    // Update is called once per frame
    private void Update()
    {
        //Debug.Log("y: " + transform.position.y);
        if (_hasToMove)
        {
            _t += Time.deltaTime * speed;
            // Moves the object to target position
            transform.position = Vector3.Lerp(startPosition, endPosition, _t);

            //           Debug.Log("x: " + endPosition.x + " y: " + endPosition.y + " t: " + _t + " Lerp: " + Vector3.Lerp(startPosition, endPosition, _t));

            // Flip the points once it has reached the target
            if (_t >= 1)
            {
                var end = endPosition;
                var start = startPosition;
                startPosition = end;
                endPosition = start;
                _t = 0;
                _hasToMove = false;
            }
        }
    }

    public override void activate()
    {
        _hasToMove = true;
    }
}
