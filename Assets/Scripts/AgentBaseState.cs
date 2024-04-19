using UnityEngine;
//This script is taken from Professor Luther's GAI_PathFSMexe example project
//It acts as a base for the AI behaviors to branch from

public abstract class AgentBaseState
{
    public abstract void EnterState(AgentController_FSM theAgent);

    public abstract void Update(AgentController_FSM theAgent);

    public abstract void OnCollisionEnter(AgentController_FSM theAgent, Collision collision);
}
