using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MoveableObject : Triggerable
{
    public float speed = 2.0f;

    private bool _isMoving = false;
    public Transform startObj;
    public Transform endObj;
    private Vector3 endPosition;
    private Vector3 startPosition;
    private Vector3 targetPosition;

    public string Sound;

    private void Awake()
    {
        startPosition = startObj.position;
        endPosition = endObj.position;
        targetPosition = startPosition;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {

    }

    public override void activate()
    {
        changeTarget();
        if(!_isMoving)
            StartCoroutine(Move());
    }

    private void changeTarget()
    {
        targetPosition = (targetPosition == endPosition) ? startPosition : endPosition;
    }

    IEnumerator Move()
    {
        _isMoving = true;
        AudioManager.instance.RpcPlaySound(Sound);
        AudioManager.instance.RpcLoopSound(Sound);
        while (transform.position != targetPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed*Time.deltaTime);
            yield return null;
        }
        AudioManager.instance.RpcStopLoopSound(Sound);
        AudioManager.instance.RpcStopSound(Sound);
        _isMoving = false;
    }
}