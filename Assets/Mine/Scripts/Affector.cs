using Core.Health;
using UnityEngine;

// The abstract class which provide a tower with different possible effects : attack effect, slow-down effect...
public abstract class Affector: MonoBehaviour
{
    // the alignment of this affector, which will be defined by its TowerLevel parent
    public SerializableIAlignmentProvider alignment { get; protected set; }
}