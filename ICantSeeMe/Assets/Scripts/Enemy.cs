using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Triggerable
{
    public Transform Target;
    private Transform jogTarget;
    public float speed_running = 4.0f;
    public float speed_jog = 2.0f;
    private bool _activated = false;

    private GameObject[] path = null;
    private int path_counter = 0;
    private int path_maxCount;
    //private bool reached_jogTarget = false;
    private Range _range;
    private float _prevX;

    void Start()
    {
        _activated = false;
        _range = GetComponentInChildren<Range>();
        _prevX = transform.position.x;
        path = GameObject.FindGameObjectsWithTag("EnemyPath");
        path_maxCount = path.Length-1;
        if (path_maxCount != 0)
            jogTarget = path[0].transform;
    }

    void Update()
    {

    }

    void FixedUpdate()
    {
        if (!_activated)
            return;
        
        setRange();

        if (Target != null) {
            followTarget();
        }
        else if (path != null && jogTarget != null) {
            jog();
        }
    }

    private void setRange()
    {
        _range.setColliderOffset((_prevX < transform.position.x) ? 1: -1);
        _prevX = transform.position.x;
    }

    private void followTarget()
    {
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(Target.position.x, transform.position.y), speed_running*Time.deltaTime);
    }

    private void jog()
    {
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(jogTarget.position.x, transform.position.y), speed_jog*Time.deltaTime);
        if (jogTarget.position.x == transform.position.x)
            updateJogTarget();
    }

    private void updateJogTarget()
    {
        path_counter = (path_counter == path_maxCount) ? 0 : path_counter+1;
        jogTarget = path[path_counter].transform;
    }

    public override void activate()
    {
        _activated = true;
    }
}
