using System.Collections.Generic;
using ActionGameFramework.Health;
using Core.Utilities;
using UnityEngine;

// Used to provide functionality for radar tower.
// The affector which will create damage to whichever in its zone.
[RequireComponent(typeof(Damager))]
public class ZoneAffector : Affector
{
    public Color affectorRangeColor = Color.yellow;

    // the targetter reference of current Damagable object. Ex: a tower.
    protected Targetter targetter;

    // the points where the laser comes from
    public List<Transform> lamps;
    public Transform zone;
    private LineRenderer line;                           // Line Renderer
    public Material mat;
    protected Damager damager;

    protected virtual void Start()
    {
        line = this.gameObject.AddComponent<LineRenderer>();
        line.material = mat;
        line.startWidth = line.endWidth = 2f;
        // Set the number of vertex fo the Line Renderer
        line.positionCount = 2;
        alignment = transform.parent.parent.GetComponent<Tower>().configuration.alignmentProvider;
        targetter = transform.parent.GetComponentInChildren<Targetter>();
        damager = GetComponent<Damager>();
        if (zone != null)
        {
            float rad = targetter.effectRadius * 2;
            zone.localScale = new Vector3(rad, rad, rad);
        }
    }

    protected virtual void Update()
    {

        List<Targetable> targets = targetter.GetAllTargets();
        if (targets.Count == 0)
        {
            Destroy(line);
        }
        if (targets.Count > 0)
        {
            foreach (Targetable target in targets)
            {
                if (target != null)
                {
                    target.TakeDamage(damager.damage * Time.deltaTime, target.position, damager.alignmentProvider);

                    if (line == null)
                    {
                        line = this.gameObject.AddComponent<LineRenderer>();
                    }
                    line.material = mat;
                    line.startWidth = line.endWidth = 0.1f;
                    line.SetPosition(0, lamps[Random.Range(0,1)].position);
                    line.SetPosition(1, target.transform.position);
                }
            }
        }
    }
}