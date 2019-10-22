﻿using System.Collections;
using System.Collections.Generic;
using System.Security.Permissions;
using UnityEngine;
using UnityEngine.AI;

public class PatrolController : MonoBehaviour {

	public Transform[] points;
	private int destPoint = 0;
	
	public Transform target;

	protected NavMeshAgent agent;
	
	
	// Use this for initialization
	protected void Start ()
	{
		agent = GetComponent<NavMeshAgent>();
		agent.autoBraking = false;
		GotoNextPoint();
	}


	void GotoNextPoint() {
		// Returns if no points have been set up
		if (points.Length == 0)
			return;

		// Set the agent to go to the currently selected destination.
		agent.destination = points[destPoint].position;

		// Choose the next point in the array as the destination,
		// cycling to the start if necessary.
		destPoint = (destPoint + 1) % points.Length;
	}


	void Update ()
	{
		// Choose the next destination point when the agent gets
		// close to the current one.
		if (!agent.pathPending && agent.remainingDistance < 0.5f)
			GotoNextPoint();
	}
}
