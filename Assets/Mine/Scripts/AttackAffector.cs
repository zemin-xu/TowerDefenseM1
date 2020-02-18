using System.Collections.Generic;
using ActionGameFramework.Audio;
using ActionGameFramework.Health;
using Core.Health;
using Core.Utilities;
using UnityEngine;

    // The affector which will launch projectile and create damage to its enemies.
	public class AttackAffector : Affector 
	{
		public bool isMultiAttack;

		public float fireRate = 1.0f;
        
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

		public override void Initialize(IAlignmentProvider affectorAlignment)
		{
			base.Initialize(affectorAlignment);
			SetUpTimers();

		}

		void OnDestroy()
		{
		}

		void OnLostTarget()
		{
			trackingEnemy = null;
		}

		void OnAcquiredTarget(Targetable acquiredTarget)
		{
			trackingEnemy = acquiredTarget;
		}

		protected virtual void SetUpTimers()
		{
			fireTimer = 1 / fireRate;
		}

		// Update the timers.
        // Launch projectile when time is up.
		protected virtual void Update()
		{
			fireTimer -= Time.deltaTime;
			if (trackingEnemy != null && fireTimer <= 0.0f)
			{
			    FireProjectile();
				fireTimer = 1 / fireRate;
			}
		}

		protected virtual void FireProjectile()
		{
			if (trackingEnemy == null)
			{
				return;
			}
			Projectile projectile = Poolable.TryGetPoolable<Projectile>(projectilePrefab);
			projectile.Launch(center.position, trackingEnemy);
/*
            // Shoot at all possible enemies.
			if (isMultiAttack)
			{
				List<Targetable> enemies = towerTargetter.GetAllTargets();
				m_Launcher.Launch(enemies, projectile, projectilePoints);
			}
            // Shoot at the close one and stick to it.
			else
			{
				m_Launcher.Launch(m_TrackingEnemy, damagerProjectile.gameObject, projectilePoints);
			}
        */
		}

		/// <summary>
		/// A delegate to compare distances of components
		/// </summary>
		/// <param name="first"></param>
		/// <param name="second"></param>
		protected virtual int ByDistance(Targetable first, Targetable second)
		{
			float firstSqrMagnitude = Vector3.SqrMagnitude(first.position - center.position);
			float secondSqrMagnitude = Vector3.SqrMagnitude(second.position - center.position);
			return firstSqrMagnitude.CompareTo(secondSqrMagnitude);
		}

#if UNITY_EDITOR
		/// <summary>
		/// Draws the search area
		/// </summary>
        /*
		void OnDrawGizmosSelected()
		{
			Gizmos.DrawWireSphere(epicenter.position, towerTargetter.effectRadius);
		}
        */
#endif
	}

	/// <summary>
	/// A delegate for boolean calculation logic
	/// </summary>
	public delegate bool Filter();