using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform Target;
    private Transform jogTarget;
    public float speed_running = 4.0f;
    public float speed_jog = 2.0f;

    private GameObject[] path = null;
    private int path_counter = 0;
    private int path_maxCount;
    private bool reached_jogTarget = false;
    private Range _range;

    void Start()
    {
        _range = GetComponentInChildren<Range>();
        path = GameObject.FindGameObjectsWithTag("EnemyPath");
        path_maxCount = path.Length;
        if (path_maxCount != 0)
            jogTarget = path[0].transform;
    }

    void Update()
    {
        
    }

    void FixedUpdate()
    {
        if (Target != null) {
            followTarget();
        }
        else if (path != null && jogTarget != null) {
            jog();
        }
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
}
