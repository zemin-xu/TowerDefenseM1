using ActionGameFramework.Health;
using Core.Utilities;
using Core.Health;
using UnityEngine;

// Projectile class controls the direction towards enemy and the behaviour when they collide.
// Damager is a class to provide damage to enemies.
[RequireComponent(typeof(Damager))]
public class Projectile : MonoBehaviour
{
    public float speed = 50.0f;

    protected bool isEnabled;

    protected Targetable target;

    private Rigidbody rigidBody;


    // the damager attached to this projectile.
    protected Damager damager;

    protected Vector3 dir;

    // Launch projectile towards targetable.
    public void Launch(Vector3 ori, Targetable other)
    {
        if (other != null)
        {
            isEnabled = true;
            target = other;
            transform.position = ori;

            dir = Vector3.Normalize(target.transform.position - transform.position);
            target.removed += OnTargetDied;

        }
    }

    // Create damage to enemy.
    protected void DealDamage()
    {
        if (target == null)
        {
            return;
        }
        target.TakeDamage(damager.damage, target.position, damager.alignmentProvider);
    }

    protected virtual void Awake()
    {
        damager = GetComponent<Damager>();
        rigidBody = GetComponent<Rigidbody>();
        isEnabled = false;
    }

    protected virtual void Update()
    {
        if (isEnabled && dir != null)
        {
            rigidBody.AddForce(dir * speed, ForceMode.Acceleration);
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
        Destroy(gameObject);
    }

    private void OnTargetDied(DamageableBehaviour targetable)
    {
        targetable.removed -= OnTargetDied;
        target = null;
    }
}