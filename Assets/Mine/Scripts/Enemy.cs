using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using ActionGameFramework.Health;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : Targetable 
{
    private NavMeshAgent agent;
    public Transform start;
    public float rotationSpeed = 10f;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(start.position);
    }

    private void Update() {
        Debug.Log(configuration.currentHealth);
    }

    public void SetNextNodeDestination(Node node)
    {
        if (node != null)
        {
            agent.SetDestination(node.transform.position);
        }
        else
        {
            // arrive at endPoint 
        }
    }

}
