using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollableObject : Triggerable
{
    public float speed = 2.0f;
    public float angle;
    public bool oneTime = false;
    public Transform pivot;
    private float _dir;

    private bool _isMoving = false;
    private bool _stop = false;
    private Vector3 endRotation;
    private Vector3 startRotation;
    private Vector3 targetRotation;

    private void Awake()
    {
        startRotation = transform.eulerAngles;
        endRotation = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z+angle);
        targetRotation = startRotation;
    }

    public override void activate()
    {
        changeTarget();
        if(!_isMoving && !_stop)
            StartCoroutine(Rotate());
    }

    private void changeTarget()
    {
        targetRotation = (targetRotation == endRotation) ? startRotation : endRotation;
        _dir = (transform.eulerAngles.z - targetRotation.z > 0) ? -1 : 1;
    }

    IEnumerator Rotate()
    {
        if (oneTime)
            _stop = true;

        _isMoving = true;
        Vector3 closeEnough = new Vector3(0, 0, 0.1f);

        while (Vector3.Distance(transform.eulerAngles, targetRotation) > speed*Time.deltaTime)
        {
            transform.RotateAround(pivot.position, Vector3.forward, _dir*speed*Time.deltaTime);
            yield return null;
        }

        _isMoving = false;
    }

}
