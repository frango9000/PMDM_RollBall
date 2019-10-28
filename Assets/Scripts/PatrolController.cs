using UnityEngine;
using UnityEngine.AI;

public class PatrolController : MonoBehaviour
{
    
    //controlador de agente que patrulla
    
    
    protected NavMeshAgent agent;
    private int destPoint;

    
    //lista de puntos que el agente va a seguir -> waypoints
    public Transform[] points;


    // Use this for initialization
    protected void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;
        GotoNextPoint();
    }


    private void GotoNextPoint()
    {
        // retorna si no hay waypoints
        if (points.Length == 0)
            return;

        // establecer la nueva direccion del agente
        agent.destination = points[destPoint].position;

        //  establecer el siguiente waypoint
        destPoint = (destPoint + 1) % points.Length;
    }


    private void Update()
    {
        // Choose the next destination point when the agent gets
        // close to the current one.
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
            GotoNextPoint();
    }
}