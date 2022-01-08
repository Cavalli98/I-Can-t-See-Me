using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switcher : Triggerable
{
    public Transform other;
    public bool oneTime = false;

    private bool _stop = false;
    private bool _isSwitching = false;
    

    public override void activate()
    {
        if (_stop)
            return;
            
        Vector3 temp = other.position;
        other.position = transform.position;
        transform.position = temp;

        if(oneTime) _stop = true;
    }

    public override void activate(float time)
    {
        if (_stop)
            return;
        
        if (!_isSwitching)
            StartCoroutine(switcher(time));
    }

    IEnumerator switcher(float s)
    {
        _isSwitching = true;

        yield return new WaitForSeconds(s);

        Vector3 temp = other.position;
        other.position = transform.position;
        transform.position = temp;

        _isSwitching = false;
        if(oneTime) _stop = true;
    }
}
