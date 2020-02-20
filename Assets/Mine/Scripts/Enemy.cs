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

    private void OnEnable()
    {
        transform.position = start.position;
        transform.rotation = Quaternion.identity;
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(start.position);
        // Subscribe death event.
        died += OnDied;
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

    private void OnDied(DamageableBehaviour damageableBehaviour)
    {
        died -= OnDied;
        OnRemoved(damageableBehaviour);
    }

    private void OnRemoved(DamageableBehaviour damageableBehaviour)
    {
        //Poolable.TryPool(gameObject);    // a bug here so I have to change it to destroy
        Destroy(this.gameObject);
    }

}
