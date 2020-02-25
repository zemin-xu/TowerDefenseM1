using Core.Health;
using UnityEngine;

// The alignment here is to differentiate the enemy and tower party.
// The abstract class which provide a tower with different possible effects : attack effect, slow-down effect...

public abstract class Affector: MonoBehaviour
{
    // the alignment of this affector, which will be defined by its TowerLevel parent
    public IAlignmentProvider alignment { get; protected set; }
}