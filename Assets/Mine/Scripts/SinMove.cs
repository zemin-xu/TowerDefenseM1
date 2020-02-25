using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// create floatting movement of object.
public class SinMove : MonoBehaviour
{

	public float frequency = 2f;

	public float maxOffset = 4f;
	public float speed = 0.1f;

    private float maxY;
    private float minY;

    private void Start() {
        maxY = transform.position.y + maxOffset;
        minY = transform.position.y - maxOffset;
    }

	void Update () {
        if(transform.position.y > minY && transform.position.y< maxY)
		{
           transform.position = transform.position + Vector3.up* maxOffset * Mathf.Sin(Time.time * frequency) * Time.deltaTime * speed;
        }

	}

}

