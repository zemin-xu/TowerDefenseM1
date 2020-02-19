using System;
using System.Collections.Generic;
using Core.Extensions;
using UnityEngine;
using Core.Utilities;

public class WaveManager : MonoBehaviour
{
    protected int currentWaveIndex;

    public bool startWavesOnAwake;

    public List<Wave> waves = new List<Wave>();

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

    protected virtual void Awake()
    {
        currentWaveIndex = 0;
        if (startWavesOnAwake && waves.Count > 0)
        {
            InitWave();
        }
    }

    protected virtual void InitWave()
    {
        Wave wave = waves[currentWaveIndex];
        wave.Init();
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
    }
}