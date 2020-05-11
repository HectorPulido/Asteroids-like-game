using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Spawner : NetworkBehaviour {
    public int asteroidsLimit = 10;
    public float timeToSpawn;
    public GameObject[] prefabs;
    public Transform[] spawners;

    [ServerCallback]
    IEnumerator Start () {
        while (true) {
            var asteroids = GameObject.FindGameObjectsWithTag ("Obstacle").Length;
            if (asteroids < asteroidsLimit) {
                var asteroid = Instantiate (
                    prefabs[Random.Range (0, prefabs.Length)],
                    spawners[Random.Range (0, spawners.Length)].position,
                    Quaternion.identity
                );

                NetworkServer.Spawn (asteroid);
            }

            yield return new WaitForSeconds (timeToSpawn);
        }
    }

}