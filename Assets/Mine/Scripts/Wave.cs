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



    WaveManager waveManager;

    public Transform startingPoint;

    public float delayToNextSpawn = 1.0f;

    protected int currentIndex;

    // the RepeatingTimer used to spawn enemies
    protected RepeatingTimer spawnTimer;

    public virtual float progress
    {
        get { return (float)(currentIndex) / enemiesToSpawn.Count; }
    }

    // Start the spawning of this wave.
    public virtual void Init()
    {
        if (enemiesToSpawn.Count == 0)
        {
            return;
        }
        spawnTimer = new RepeatingTimer(delayToNextSpawn, SpawnCurrent);
        StartTimer(spawnTimer);
        waveManager = FindObjectOfType<WaveManager>();
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
            spawnTimer.SetTime(delayToNextSpawn);
            StopTimer(spawnTimer);
            waveManager.TryNextWave();
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
        Poolable poolable = Poolable.TryGetPoolable<Poolable>(enemy.gameObject);
    }
}