using ActionGameFramework.Health;
using Core.Utilities;
using UnityEngine;

	[RequireComponent(typeof(Damager))]
	public class Projectile: MonoBehaviour
	{
		public float speed = 100.0f;

		protected bool isEnabled;

		protected Targetable target;

		// the damager attached to this projectile.
		protected Damager damager;

		// the position this projectile will be launched from.
		protected Vector3 origin;

		// Launch projectile towards targetable.
		public void Launch(Vector3 ori, Targetable other)
		{
			isEnabled = true;
			target = other;
			origin = ori;
		}

		public void MoveTowardsTarget()
		{
			Vector3 dir = Vector3.Normalize(origin - target.transform.position);
			transform.Translate(dir);
		}

		protected void DealDamage()
		{
			if (target == null)
			{
				return;
			}
			/*
			ParticleSystem pfxPrefab = m_Damager.collisionParticles;
			var attackEffect = Poolable.TryGetPoolable<ParticleSystem>(pfxPrefab.gameObject);
			attackEffect.transform.position = m_Enemy.position;
			attackEffect.Play();
			*/
			
			target.TakeDamage(damager.damage, target.position, damager.alignmentProvider);
		}

		protected virtual void Awake()
		{
			damager = GetComponent<Damager>();
			isEnabled = false;
		}

		protected virtual void Update() {
			if (isEnabled)
			{
				MoveTowardsTarget();
			}
			
		}

		protected virtual void OnCollisionEnter(Collision other) 
		{
			Targetable tar;
			tar = other.gameObject.GetComponent<Targetable>();
			if (tar != null && tar == target)
			{
				DealDamage();
			}
			isEnabled = false;
			Poolable.TryPool(gameObject);
		}

	}