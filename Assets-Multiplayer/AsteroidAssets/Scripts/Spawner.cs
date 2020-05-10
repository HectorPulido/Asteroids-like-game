using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Spawner : NetworkBehaviour {
    public float timeToSpawn;
    public GameObject[] prefabs;
    public Transform[] spawners;

    [ServerCallback]
    IEnumerator Start () {
        while (true) {
            var asteroid = Instantiate (
                prefabs[Random.Range (0, prefabs.Length)],
                spawners[Random.Range (0, spawners.Length)].position,
                Quaternion.identity
            );

            NetworkServer.Spawn (asteroid);

            yield return new WaitForSeconds (timeToSpawn);
        }
    }

}