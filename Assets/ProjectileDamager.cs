using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ActionGameFramework.Health;
public class ProjectileDamager : Damager  
{
    protected override void OnCollisionEnter(Collision other)
		{
			if (collisionParticles == null || Random.value > chanceToSpawnCollisionPrefab)
			{
				return;
			}
            GameObject go = Instantiate(collisionParticles.gameObject);
            var pfx = go.GetComponent<ParticleSystem>();


			pfx.transform.position = transform.position;
			pfx.Play();
		}
}
