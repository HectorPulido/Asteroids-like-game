using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {
    public float timeToSpawn;
    public GameObject[] prefabs;
    public Transform[] spawners;

    IEnumerator Start () {
        while (true) {
            Instantiate (
                prefabs[Random.Range (0, prefabs.Length)],
                spawners[Random.Range (0, spawners.Length)].position,
                Quaternion.identity
            );

            yield return new WaitForSeconds (timeToSpawn);
        }
    }

}