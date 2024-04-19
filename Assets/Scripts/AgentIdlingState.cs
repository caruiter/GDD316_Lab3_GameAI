using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

//this is a behavior script for an idling agent

public class AgentIdlingState : AgentBaseState
{
    Vector3 wanderSpot;


    public override void EnterState(AgentController_FSM theAgent)
    {
        Debug.Log("idling");
        theAgent.MeshRenderer.material.color = Color.green;
        //theAgent.transform.position = theAgent.patrolPos;
        GetNewWanderPos(theAgent, theAgent.startPos);

        theAgent.anim.SetBool("Idling", true);
        theAgent.anim.SetBool("Pursuing", false);
        theAgent.anim.SetBool("Searching", false);
    }

    public override void OnCollisionEnter(AgentController_FSM theAgent, Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")) //if player bumps into agent, begin pursuit
        {
            theAgent.CaughtSight();
        }
    }

    public override void Update(AgentController_FSM theAgent)
    {
        if (theAgent.NavMeshAgent.remainingDistance <= 0.2) //when close to destination, trigger animation
        {
            //trigger animation
            theAgent.anim.SetTrigger("IdleBob");
            //Debug.Log("IDLE BOB 2");

        }

    }

    //find a new destination 
    public void GetNewWanderPos(AgentController_FSM theAgent, Vector3 startPos)
    {
        Vector3 aim = theAgent.startPos;

        //check if agent is within its wandering range
        double distance = Mathf.Sqrt(Mathf.Pow(theAgent.transform.position.x - startPos.x, 2) + Mathf.Pow(theAgent.transform.position.z - startPos.z, 2));
        if(distance > theAgent.wanderRadius)
        {
            //if not, direct it to wander back in right direction
            aim = new Vector3(theAgent.transform.position.x - startPos.x, 0f, theAgent.transform.position.z - startPos.z);
            float rand = Random.Range(0f, 1f);
            aim = aim * rand;
        }
        else
        { //else give it a random point within range to wander to 
            aim = Random.insideUnitSphere * (int)theAgent.wanderRadius;
            aim += theAgent.transform.position;
        }
        //Debug.Log("New wander pos = " + aim);

        theAgent.NavMeshAgent.SetDestination(aim);
        theAgent.gameObject.transform.LookAt(aim);
        //wanderSpot = aim;
        //theAgent.transform.forward = wanderSpot;
        //wandering = true;
        //trigger walking animation
    }
}