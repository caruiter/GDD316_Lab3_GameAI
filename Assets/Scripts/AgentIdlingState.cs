using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AgentIdlingState : AgentBaseState
{
    bool wandering;
    Vector3 wanderSpot;


    public override void EnterState(AgentController_FSM theAgent)
    {
        Debug.Log("idling");
        wandering = true;
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
        /**if(theAgent.transform.position.x == wanderSpot.x&& theAgent.transform.position.z == wanderSpot.z) //check if agent is at spot
        {
            wandering = false;
            //trigger animation
            theAgent.anim.SetTrigger("IdleBob");
            Debug.Log("IDLE BOB");

        }**/
        if (theAgent.NavMeshAgent.remainingDistance <= 0.2)
        {
            wandering = false;
            //trigger animation
            theAgent.anim.SetTrigger("IdleBob");
            Debug.Log("IDLE BOB 2");

        }
        /**
        if(wandering) //walk agent
        {
            //theAgent.transform.position += (theAgent.transform.forward * theAgent.agentSpeed * Time.deltaTime);
        }

        //theAgent.NavMeshAgent.SetDestination(theAgent.patrolPos);
        //theAgent.TransitionToState(theAgent.PatrolState);
        **/
    }

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
        Debug.Log("New wander pos = " + aim);

        theAgent.NavMeshAgent.SetDestination(aim);
        theAgent.gameObject.transform.LookAt(aim);
        //wanderSpot = aim;
        //theAgent.transform.forward = wanderSpot;
        //wandering = true;
        //trigger walking animation
    }
}