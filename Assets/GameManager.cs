using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    public GameObject prefab;
    public NavMeshAgent agent;

    public Transform startP;
    public Transform endP;


    private void Start() {

        GameObject go = Instantiate(prefab, startP);
        agent = go.GetComponent<NavMeshAgent>();

        agent.SetDestination(endP.position);
        
    }

}
