using UnityEngine;
//This script is taken from Professor Luther's GAI_PathFSMexe example project

public abstract class AgentBaseState
{
    public abstract void EnterState(AgentController_FSM theAgent);

    public abstract void Update(AgentController_FSM theAgent);

    public abstract void OnCollisionEnter(AgentController_FSM theAgent, Collision collision);
}
