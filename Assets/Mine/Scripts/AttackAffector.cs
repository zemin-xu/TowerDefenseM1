using System.Collections.Generic;
using ActionGameFramework.Health;
using Core.Utilities;
using UnityEngine;

// The affector which will launch projectile and create damage to its enemies.
public class AttackAffector : Affector
{
    public bool isMultiAttack;

    public float fireRate = 1f;
    public GameObject projectilePrefab;

    // the color of it effect range to be visualized in editor.
    public Color affectorRangeColor = Color.yellow;

    // the possible launch position of projectiles.
    public Transform[] projectilePoints;

    // the targetter reference of current Damagable object. Ex: a tower.
    protected Targetter targetter;

    // the center of an AttackAffector
    protected Transform center;

    // a timer before next fire, equals 1 / fireRate
    protected float fireTimer;

    // current tracking enemy
    protected Targetable trackingEnemy;

    // When lost enemy.
    public void OnLostTarget()
    {
        trackingEnemy = null;
    }

    // When the targetter get the enemy.
    public void OnAcquiredTarget(Targetable acquiredTarget)
    {
        trackingEnemy = acquiredTarget;
    }

    protected virtual void Start()
    {
        center = transform.parent;
        alignment = transform.parent.parent.GetComponent<Tower>().configuration.alignmentProvider;
        targetter = transform.parent.GetComponentInChildren<Targetter>();
        targetter.acquireTargetable += OnAcquiredTarget;
        targetter.loseTargetable += OnLostTarget;
    }

    // Update the timers.
    // Launch projectile when time is up.
    // bug here, will always trigger two times
    protected virtual void Update()
    {
        fireTimer -= Time.deltaTime;
        if (trackingEnemy != null && fireTimer < 0.0f)
        {
            fireTimer = 1 / fireRate;
            FireProjectile();
        }
    }

    protected virtual void FireProjectile()
    {
        // Shoot at all possible enemies.
        if (isMultiAttack)
        {
            int i = 0;
            List<Targetable> targetables = targetter.GetAllTargets();
            foreach(Targetable t in targetables)
            {
                if (t != null)
                {
                    Projectile projectile = Poolable.TryGetPoolable<Projectile>(projectilePrefab);
                    projectile.Launch(projectilePoints[i].position, t);
                }
                i++;
                 
            }
        }
        // Shoot at the cloiest one and stick to it.
        else
        {
            Projectile projectile = Poolable.TryGetPoolable<Projectile>(projectilePrefab);
          
            projectile.Launch(projectilePoints[0].position, trackingEnemy);
        }
    }
}