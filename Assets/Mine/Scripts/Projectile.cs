using ActionGameFramework.Health;
using Core.Utilities;
using UnityEngine;

[RequireComponent(typeof(Damager))]
public class Projectile : MonoBehaviour
{
    public float speed = 50.0f;

    protected bool isEnabled;

    protected Targetable target;

    // the damager attached to this projectile.
    protected Damager damager;

    protected Vector3 dir;

    // Launch projectile towards targetable.
    public void Launch(Vector3 ori, Targetable other)
    {
        isEnabled = true;
        target = other;
        transform.position = ori;
        dir = Vector3.Normalize(target.transform.position - transform.position);
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

    protected virtual void Update()
    {
        if (isEnabled && dir != null)
        {
            transform.Translate(dir * speed * Time.deltaTime);
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