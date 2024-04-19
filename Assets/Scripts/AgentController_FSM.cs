using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//This script is adapted from Professor Luther's GAI_PathFSMexe example project
//It acts as the base controller for the agent AI

public class AgentController_FSM : MonoBehaviour
{
    #region Agent Variables

    public GameObject player;
    public GameObject itself;

    public Vector3 safePos;
    public Vector3 fightPos;
    public Vector3 patrolPos;
    public Vector3 startPos;
    public Vector3 alertArea;
    public Animator anim;

    public double wanderRadius;
    public int agentSpeed;

    //private Rigidbody rbody;
    private MeshRenderer meshRenderer;
    private NavMeshAgent agent;

    //public Rigidbody Rigidbody
    //{
    //    get { return rbody; }
    //}

    public MeshRenderer MeshRenderer
    {
        get { return meshRenderer; }
    }
    public NavMeshAgent NavMeshAgent
    {
        get { return agent; }
    }
    #endregion

    private AgentBaseState currentState;

    public AgentBaseState CurrentState
    {
        get { return currentState; }
    }

    public readonly AgentIdlingState IdleState = new AgentIdlingState();
    public readonly AgentSearchingState SearchingState = new AgentSearchingState();
    public readonly AgentPursuitState PursuitState = new AgentPursuitState();

    private void Awake()
    {
        //rbody = GetComponent<Rigidbody>();
        meshRenderer = GetComponent<MeshRenderer>();
        agent = GetComponent<NavMeshAgent>();
        safePos = new Vector3(2, 0, 2);
        fightPos = new Vector3(-2, 0, -2);
        patrolPos = new Vector3(2, 0, -2);
        startPos = transform.position;
        anim = GetComponent<Animator>();
        itself = this.gameObject;
    }

    private void Start()//begin idling once woken up
    {
       TransitionToState(IdleState);
   
    }

    // Update is called once per frame
    void Update()
    {
        currentState.Update(this);
    }

    private void OnCollisionEnter(Collision collision)
    {
        currentState.OnCollisionEnter(this, collision); //on collision, trigger reaction in agent
    }

    public void TransitionToState(AgentBaseState state)
    {
        currentState = state;
        currentState.EnterState(this);
    }

    public void HeardNoise(Vector3 SoundPos) // noise occurs within hearing range. agent begins searching
    {
        TransitionToState(SearchingState);
        SearchingState.alertArea = SoundPos;
    }

    public void CaughtSight() // noise occurs within visible range. agent begins pursuing
    {
        TransitionToState(PursuitState);
    }

    public void FinishedIdling() // triggered through animation. agent finishes idling action at current goalpoint, finds next location
    {
        if(currentState == IdleState)
        {
            Debug.Log("finished idling");
            IdleState.GetNewWanderPos(this, startPos);
        }
    }

    public void FinishedSearchAnim() // triggered through animation. agent finishes searching animation at current goalpoint
    {
        if(currentState == SearchingState)
        {
            //if player is within visible range, begin pursuit
            if (NavMeshAgent.remainingDistance <= player.GetComponent<PlayerSoundControl>().GetSightRange())
            {
                TransitionToState(PursuitState);
            }
            else
            { //find new area to search
                SearchingState.CheckNewArea(this);
            }
        }
    }

    public void FinishAttack() // attack is finished, evaluate next choice
    {
        /**double distance = Mathf.Sqrt(Mathf.Pow(transform.position.x - player.transform.position.x, 2) + Mathf.Pow(transform.position.z - player.transform.position.z, 2));
        if (distance <= player.GetComponent<PlayerSoundControl>().GetSightRange())
        {
            TransitionToState(PursuitState);
        }
        else
        {
            TransitionToState(SearchingState);
        }**/

        if (NavMeshAgent.remainingDistance <= player.GetComponent<PlayerSoundControl>().GetSightRange())
        {
            TransitionToState(PursuitState); //if player still within sight -> continue pursuing
        }
        else // begin searching for player
        {
            TransitionToState(SearchingState);
        }

    }

}
