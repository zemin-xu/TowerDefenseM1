using System;
using System.Collections.Generic;
using Core.Extensions;
using Core.Utilities;
using UnityEngine;

// The class of a single wave.
// This class is much inspired by the Tower Defense Template implementation.
// Some code is still not understandable.
public class Wave : TimedBehaviour
{
    // the enemies prefab to spawn
    // to be configurated in Editor
    public List<Enemy> enemiesToSpawn;
    WaveManager waveManager;

    public Transform startingPoint;

    public float delayToNextSpawn = 2.0f;

    protected int currentIndex;

    // the RepeatingTimer used to spawn enemies
    protected RepeatingTimer spawnTimer;

    public virtual float progress
    {
        get { return (float)(currentIndex) / enemiesToSpawn.Count; }
    }

    // Start the spawning of this wave.
    public virtual int Init()
    {
        if (enemiesToSpawn.Count == 0)
        {
            return 0;
        }
        spawnTimer = new RepeatingTimer(delayToNextSpawn, SpawnCurrent);
        StartTimer(spawnTimer);
        waveManager = FindObjectOfType<WaveManager>();
        return (enemiesToSpawn.Count);
    }

    // Handles spawning the current agent and sets up the next agent for spawning
    protected virtual void SpawnCurrent()
    {
        SpawnAgent();
        if (!TryNextSpawn())
        {
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