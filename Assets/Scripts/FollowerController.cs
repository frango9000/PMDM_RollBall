using UnityEngine;
using UnityEngine.AI;

public class FollowerController : MonoBehaviour
{
    protected NavMeshAgent agent;

    public Transform target;

    // Use this for initialization
    protected void Start()
    {
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