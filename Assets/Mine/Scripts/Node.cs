using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Collider))]
public class Node : MonoBehaviour
{
    public List<Node> nextNodes;

    // Once enemy arrive at end point, this function will attribute another destination for it.
    public void OnTriggerEnter(Collider other)
    {
            Debug.Log(other.name);
        Enemy enemy = other.gameObject.GetComponent<Enemy>();
        if (enemy != null)
        {
            Node node = GetRandomNextNode();
            enemy.SetNextNodeDestination(node);
        }
    }

    // Get a random next node in list.
    public Node GetRandomNextNode()
    {
        if (nextNodes.Count > 0)
        {
            int i = Random.Range(0, nextNodes.Count);
            Debug.Log(nextNodes[i].name);
            return (nextNodes[i]);
        }
        return (null);
    }

}
