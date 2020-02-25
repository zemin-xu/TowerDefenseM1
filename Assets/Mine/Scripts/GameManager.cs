using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    // In order to make music fluent between menu and main scene.
    private void Awake() {
        DontDestroyOnLoad(this.gameObject);
    }
}