using System;
using System.Collections.Generic;
using Core.Extensions;
using Core.Utilities;
using UnityEngine;

// The class of a single wave.
public class Wave : TimedBehaviour
{
    // the enemies prefab to spawn
    // to be configurated in Editor
    public List<Enemy> enemiesToSpawn;

    public Transform startingPoint;

    public float delayToNextSpawn = 1.0f;

    protected int currentIndex;

    // the RepeatingTimer used to spawn enemies
    protected RepeatingTimer spawnTimer;

    private void Start() {
        Init();
    }

    public virtual float progress
    {
        get { return (float)(currentIndex) / enemiesToSpawn.Count; }
    }

    public virtual void Init()
    {
        if (enemiesToSpawn.Count == 0)
        {
            return;
        }
        spawnTimer = new RepeatingTimer(delayToNextSpawn, SpawnCurrent);
        StartTimer(spawnTimer);
    }

    /// <summary>
    /// Handles spawning the current agent and sets up the next agent for spawning
    /// </summary>
    protected virtual void SpawnCurrent()
    {
        SpawnAgent();
        if (!TryNextSpawn())
        {
            // this is required so wave progress is still accurate
            currentIndex = enemiesToSpawn.Count;
            StopTimer(spawnTimer);
        }
    }

    protected bool TryNextSpawn()
    {
        bool hasNext = enemiesToSpawn.Next(ref currentIndex);
        if (hasNext)
        {
            Enemy enemy = enemiesToSpawn[currentIndex];
            spawnTimer.SetTime(delayToNextSpawn);
        }

        return hasNext;
    }

    protected virtual void SpawnAgent()
    {
        Enemy enemy = enemiesToSpawn[currentIndex];
        if (startingPoint == null)
        {
            return;
        }
        var poolable = Poolable.TryGetPoolable<Poolable>(enemy.gameObject);
        if (poolable == null)
        {
            return;
        }
        var agentInstance = poolable.GetComponent<Enemy>();
        agentInstance.transform.position = startingPoint.position;
        agentInstance.transform.rotation = Quaternion.identity;
    }
}