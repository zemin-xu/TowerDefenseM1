using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using ActionGameFramework.Health;
using Core.Utilities;
using Core.Health;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : Targetable 
{
    private NavMeshAgent agent;
    public Transform start;
    public float rotationSpeed = 10f;

    private void OnEnable() {
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(start.position);
        // Subscribe death event.
        configuration.died += OnDied;
        
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

    private void OnDied(HealthChangeInfo healthChangeInfo)
    {
        configuration.died -= OnDied;
        Poolable.TryPool(gameObject);
    }
}
