using System;
using System.Collections.Generic;
using Core.Extensions;
using UnityEngine;
using Core.Utilities;

// WaveManager will control all waves and give info like waveProgress to other class.

public class WaveManager : Singleton<WaveManager> 
{
    protected int currentWaveIndex;

    public int enemyNum {get; protected set;}

    public bool startWavesOnAwake;

    public List<Wave> waves = new List<Wave>();

    public bool isAllWavesSpawned {get; protected set;}

    public int waveNumber
    {
        get { return currentWaveIndex + 1; }
    }

    public int totalWaves
    {
        get { return waves.Count; }
    }

    public float waveProgress
    {
        get
        {
            if (waves == null || waves.Count <= currentWaveIndex)
            {
                return 0;
            }
            return waves[currentWaveIndex].progress;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        enemyNum = 0;
        currentWaveIndex = 0;
        isAllWavesSpawned = false;
        if (startWavesOnAwake && waves.Count > 0)
        {
            InitWave();
        }
    }

    private void Start() {
        GameUI.instance.startWaveActivated += OnActivatedStartWave;
    }

    private void OnActivatedStartWave()
    {
        if (waves.Count > 0 && !startWavesOnAwake)
        {
            InitWave();
        }
    }

    protected virtual void InitWave()
    {
        Wave wave = waves[currentWaveIndex];
        // Strangely the enemy death event will always be trigger 2 times.
        // Not a good solution.
        enemyNum += wave.Init() * 2;
        
    }

    public void TryNextWave()
    {
        bool hasNext = waves.Next(ref currentWaveIndex);

        if (hasNext)
        {
            InitWave();
            return;
        }
        currentWaveIndex = waves.Count;
        isAllWavesSpawned = true; 
        
    }

    public void DecrementEnemyNum()
    {
        enemyNum--;
    }


}