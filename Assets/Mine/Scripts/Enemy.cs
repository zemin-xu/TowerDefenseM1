using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using ActionGameFramework.Health;
using Core.Utilities;
using Core.Health;

// Enemy class is used to get connection with NavMeshAgent like getting node.
// The parent class Targetable here is to provide it with basic health settings which is created by Unity.
[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : Targetable
{
    private NavMeshAgent agent;

    // Spawning point.
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
        // When not arriving destination, get the next destination.
        if (node != null)
        {
            agent.SetDestination(node.transform.position);
        }
        // When enemy arrive at destination it will attack homebase.
        else
        {
            StartCoroutine(AttackHomeOnSuicided());
        }
    }

    // DamageableBehaviour records the health info the enemy and several events when it hurts or dies.
    private void OnDied(DamageableBehaviour damageableBehaviour)
    {
        died -= OnDied;
        OnRemoved(damageableBehaviour);
    }

    private void OnRemoved(DamageableBehaviour damageableBehaviour)
    {
        LevelManager.instance.UpdateMoney((int)damageableBehaviour.configuration.maxHealth / 10);
        WaveManager.instance.DecrementEnemyNum();
        LevelManager.instance.DetectAllEnemiesCleaned();
        //Poolable.TryPool(gameObject);    // a bug here so I have to change it to destroy
        Destroy(this.gameObject);
    }

    private IEnumerator AttackHomeOnSuicided()
    {
        yield return new WaitForSeconds(1.0f);
        LevelManager.instance.UpdateLife(-1);
        Kill();
    }

}
