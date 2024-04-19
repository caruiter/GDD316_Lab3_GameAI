using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentSearchingState : AgentBaseState
{
    public int check;
    public Vector3 alertArea;

    public override void EnterState(AgentController_FSM theAgent)
    {
        Debug.Log("searching");
        theAgent.MeshRenderer.material.color = Color.yellow;
        //theAgent.transform.position = theAgent.patrolPos;
  
        theAgent.anim.SetBool("Pursuing", false);
        theAgent.anim.SetBool("Idling", false);
        theAgent.anim.SetBool("Searching", true);

        check = 0;
        CheckNewArea(theAgent);
        check = 0;


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
        /**if (theAgent.transform.position.x == alertArea.x && theAgent.transform.position.z == alertArea.z) //check if agent is at spot
        {
            check++;
            //trigger animation
            theAgent.anim.SetTrigger("SearchAnim");

            //theAgent.NavMeshAgent.SetDestination(theAgent.patrolPos);
        }**/

        if (theAgent.NavMeshAgent.remainingDistance <= 0.2)
        {
            check++;
            //trigger animation
            theAgent.anim.SetTrigger("SearchAnim");
        }
    }


    public void CheckNewArea(AgentController_FSM theAgent)
    {
        if (check < 3)
        {
            // CheckNewArea(theAgent);
            Vector3 aim = theAgent.alertArea;
            aim = Random.insideUnitSphere * (int)theAgent.wanderRadius;
            aim += theAgent.alertArea;
            alertArea = aim;

            theAgent.NavMeshAgent.SetDestination(aim);

            Debug.Log("New search pos = " + aim);
            theAgent.gameObject.transform.LookAt(aim);
        }
        else
        {
            theAgent.TransitionToState(theAgent.IdleState);
        }
        
    }
}