using System;
using System.Collections.Generic;
using ActionGameFramework.Health;
using Core.Health;
using UnityEngine;

	public class Targetter : MonoBehaviour
	{
		public Transform turret;

        // the max rotation range from -60.0 to 60.0 in y-axis
        public float rotationRange = 60.0f;

        // frequency for searching targetables
		public float searchRate;

		public float rotationSpeed = 60.0f;

		protected SphereCollider attachedCollider;

		protected List<Targetable> targetsInRange;
        
        // the cloiest target in range
		protected Targetable currentTargetable;

		// the seconds until next search is allowed
		protected float searchTimer = 0.0f;

        // the alignment of affector attached
		protected SerializableIAlignmentProvider alignment;

		public float effectRadius
		{
			get
			{
				if (attachedCollider != null)
				{
					return attachedCollider.radius;
				}
				return 0;
			}
		}

		public Targetable GetCurrentTarget()
		{
			return currentTargetable;
		}

		public List<Targetable> GetAllTargets()
		{
			return targetsInRange;
		}

		protected virtual void Start()
		{
            targetsInRange = new List<Targetable>();
			targetsInRange.Clear();
            alignment = transform.parent.parent.GetComponent<Tower>().configuration.alignment;
            Debug.Log(alignment);
			// Reset turret facing.
			if (turret != null)
			{
				turret.localRotation = Quaternion.identity;
			}
		}

		protected virtual void Update()
		{
			searchTimer -= Time.deltaTime;

			if (searchTimer <= 0.0f && currentTargetable == null && targetsInRange.Count > 0)
			{
				currentTargetable = GetNearestTargetable();
				if (currentTargetable != null)
				{
					searchTimer = 1 / searchRate;
				}
			}
			AimTurret();
		}
	 
		protected virtual void OnTriggerEnter(Collider other)
		{
			var targetable = other.GetComponent<Targetable>();
			if (!IsTargetableValid(targetable))
			{
				return;
			}
			targetsInRange.Add(targetable);
		}

		protected virtual void OnTriggerExit(Collider other)
		{
			var targetable = other.GetComponent<Targetable>();
			if (!IsTargetableValid(targetable))
			{
				return;
			}
			if (targetable == currentTargetable)
			{
                currentTargetable = null;
			}
			targetsInRange.Remove(targetable);

            if (currentTargetable != null && targetable == currentTargetable)
			{
				currentTargetable = null;
			}

		}

        // Find out whether the found target is in the opponent alignment.
		protected virtual bool IsTargetableValid(Targetable targetable)
		{
			if (targetable == null)
			{
				return false;
			}
			bool canDamage = alignment.CanHarm(targetable.configuration.alignmentProvider);
			return canDamage;
		}

        // Get the nearest target.
		protected virtual Targetable GetNearestTargetable()
		{
			int length = targetsInRange.Count;

			if (length == 0)
			{
				return null;
			}

			Targetable nearest = null;
			float distance = float.MaxValue;
			for (int i = length - 1; i >= 0; i--)
			{
				Targetable targetable = targetsInRange[i];
                // Remove dead ones in list.
				if (targetable == null || targetable.isDead)
				{
					targetsInRange.RemoveAt(i);
					continue;
				}
				float currentDistance = Vector3.Distance(transform.position, targetable.position);
				if (currentDistance < distance)
				{
					distance = currentDistance;
					nearest = targetable;
				}
			}
			return nearest;
		}

		// Aim the turret at the current target.
		protected virtual void AimTurret()
		{
			if (turret == null)
			{
				return;
			}

			if (currentTargetable != null) // do idle rotation
			{
				Vector3 targetPosition = currentTargetable.position;
                // We only rotate around y-axis.
				targetPosition.y = turret.position.y;
				Vector3 direction = targetPosition - turret.position;
				Quaternion look = Quaternion.LookRotation(direction, Vector3.up);
				Vector3 lookEuler = look.eulerAngles;
				// We need to convert the rotation to a -180/180 wrap so that we can clamp the angle with a min/max
				float x = Wrap180(lookEuler.x);
				lookEuler.x = Mathf.Clamp(x, -rotationRange, rotationRange);
				look.eulerAngles = lookEuler;
				turret.rotation = look;
			}
		}

		/// <summary>
		/// A simply function to convert an angle to a -180/180 wrap
		/// </summary>
		static float Wrap180(float angle)
		{
			angle %= 360;
			if (angle < -180)
			{
				angle += 360;
			}
			else if (angle > 180)
			{
				angle -= 360;
			}
			return angle;
		}
	}