using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Lever : Trigger
{
    private float timePassed = 0.0f;

    public Sprite activated;
    public Sprite deactivated;
    private bool isActive = false;

    public bool isTimed = false;
    public Transform[] arm_path;
    public Transform lever_arm;
    private int current_point = 0;
    private float speed_linear = 0.0f;
    private float speed_angular = 0.0f;
    public float desired_duration = 5.0f;


    private void Awake()
    {
        isActive = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        setSpeed();
    }

    void FixedUpdate()
    {
        if (isActive && isTimed) {
            lever_arm.position = Vector2.MoveTowards(lever_arm.position, arm_path[current_point].position, speed_linear*Time.deltaTime);
            lever_arm.Rotate(Vector3.forward*speed_angular*Time.deltaTime);

            if (lever_arm.position == arm_path[current_point].position) {
                if (current_point == 0)
                    isActive = false;
                else current_point--;
            }
        }
    }

   [PunRPC]
    public override void trigger()
    {
        if (!isTimed) {
            if (!isActive)
            {
                GetComponent<SpriteRenderer>().sprite = activated;
                isActive = true;
            }

            else if (isActive)
            {
                GetComponent<SpriteRenderer>().sprite = deactivated;
                isActive = false;
            }
            
            foreach (GameObject t in toActivate)
                t.GetComponent<Triggerable>().activate();
        } else {
            if (!isActive) {
                isActive = true;
                lever_arm.position = arm_path[4].position;
                lever_arm.rotation = arm_path[4].rotation;
                current_point = 4;
            }

            foreach (GameObject t in toActivate)
                t.GetComponent<Triggerable>().activate(desired_duration);
        }
    }

    private void setSpeed()
    {
        float distance = 0.0f;
        float angular_distance = 0.0f;
        for (int i = 0; i < arm_path.Length-1; i++) {
            distance += Vector3.Distance(arm_path[i].position, arm_path[i+1].position);
        }
        angular_distance = Mathf.DeltaAngle(arm_path[arm_path.Length-1].eulerAngles.z, arm_path[0].eulerAngles.z);

        speed_linear = distance/desired_duration;
        speed_angular = angular_distance/desired_duration;
    }
    
    // to print stuff every s seconds
    private void PrintStuff(float s)
    {
        timePassed += Time.deltaTime;
        if(timePassed > s)
        {
            print("isActive= "+isActive+"  speed_angular= "+speed_angular+"  lever_arm.eulerAngles"+lever_arm.eulerAngles);
            timePassed=0f;
        } 
    }
}
