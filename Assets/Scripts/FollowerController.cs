using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowerController : MonoBehaviour
{

	public Transform target;

	protected NavMeshAgent agent;
	
	// Use this for initialization
	protected void Start ()
	{
		agent = GetComponent<NavMeshAgent>();
		agent.autoBraking = false;
	}

	
    // Update is called once per frame
    void Update ()
    {
	    // actualizamos la posicion de la capsula (enemy/agent)
	    // con el player/target
	    agent.SetDestination(target.position);
    }
}