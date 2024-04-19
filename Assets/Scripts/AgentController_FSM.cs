using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//This script is adapted from Professor Luther's GAI_PathFSMexe example project

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

    private void Start()
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
        currentState.OnCollisionEnter(this, collision);
    }

    public void TransitionToState(AgentBaseState state)
    {
        currentState = state;
        currentState.EnterState(this);
    }

    public void HeardNoise(Vector3 SoundPos)
    {
        TransitionToState(SearchingState);
        SearchingState.alertArea = SoundPos;
    }

    public void CaughtSight()
    {
        TransitionToState(PursuitState);
    }

    public void FinishedIdling()
    {
        if(currentState == IdleState)
        {
            Debug.Log("finished idling");
            IdleState.GetNewWanderPos(this, startPos);
        }
    }

    public void FinishedSearchAnim()
    {
        if(currentState == SearchingState)
        {
            if (NavMeshAgent.remainingDistance <= player.GetComponent<PlayerSoundControl>().GetSightRange())
            {
                TransitionToState(PursuitState);
            }
            else
            {
                SearchingState.CheckNewArea(this);
            }
        }
    }

    public void FinishAttack()
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
            TransitionToState(PursuitState);
        }
        else
        {
            TransitionToState(SearchingState);
        }

    }

}
