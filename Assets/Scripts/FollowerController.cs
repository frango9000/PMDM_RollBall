using UnityEngine;
using UnityEngine.AI;

public class FollowerController : MonoBehaviour
{
    
    //controlador de agente que persigue
    
    
    protected NavMeshAgent agent;

    public Transform target;

    // Use this for initialization
    protected void Start()
    {
        //obtenemos el NavMeshAgent al inicar para ser utilizado en cada frame
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;
    }


    // Update is called once per frame
    private void Update()
    {
        // actualizamos la posicion de la capsula (enemy/agent)
        // con el player/target
        agent.SetDestination(target.position);
    }
}