using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

//this is a behavior script for an agent pursuing the player

public class AgentPursuitState : AgentBaseState
{
    public override void EnterState(AgentController_FSM theAgent)
    {
        Debug.Log("pursuing");
        theAgent.MeshRenderer.material.color = Color.red;
        //theAgent.transform.position = theAgent.patrolPos;

        theAgent.NavMeshAgent.SetDestination(theAgent.player.transform.position);

        theAgent.anim.SetBool("Pursuing", true);
        theAgent.anim.SetBool("Idling", false);
        theAgent.anim.SetBool("Searching", false);
    }

    public override void OnCollisionEnter(AgentController_FSM theAgent, Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")) //if player collides with agent, attack
        {
            theAgent.anim.SetTrigger("Attacking");
        }
    }

    public override void Update(AgentController_FSM theAgent)
    {
        /**double distance = Mathf.Sqrt(Mathf.Pow(theAgent.transform.position.x - theAgent.player.transform.position.x, 2) + Mathf.Pow(theAgent.transform.position.z - theAgent.player.transform.position.z, 2));
        if (distance < 5f)
        {
            //begin attack anim
            theAgent.anim.SetTrigger("Attacking");

        }**/

        if (theAgent.NavMeshAgent.remainingDistance <= 5f) // if in range of player, attack
        {

            //begin attack anim
            theAgent.anim.SetTrigger("Attacking");
        }


        //update the agent's destination so it pursues player accurately
        theAgent.NavMeshAgent.SetDestination(theAgent.player.transform.position);
        theAgent.gameObject.transform.LookAt(theAgent.player.transform.position);

    }
}