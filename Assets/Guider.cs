using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Guider : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform destination;
    void Start()
    {
       NavMeshAgent agent = GetComponent<NavMeshAgent>();
       agent.SetDestination(destination.position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
