using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PyramidTrigger : Trigger
{
    public Pyramid[] Pyramids;

    public int[] combination;

    private bool _active;
    private bool _unlocked;

    private void Awake()
    {
        _active = false;
    }

    // Update is called once per frame
    public void Check()
    {
        _unlocked = true;
        int i = 0;
        foreach (Pyramid p in Pyramids)
        {
            if(p.face!= combination[i])
            {
                _unlocked = false;
            }
            i++;
        }
        if (_unlocked)
        {
            foreach (GameObject t in toActivate)
                t.GetComponent<Triggerable>().activate();
            _active = true;
        }
        else
        {
            if (_active)
            {
                foreach (GameObject t in toActivate)
                    t.GetComponent<Triggerable>().activate();
                _active = false;
            }
        }
    }
}
