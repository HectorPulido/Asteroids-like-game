using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

[System.Serializable]
public struct Borders {
    public float superiorBorder, inferiorBorder, leftBorder, rightBorder;
}

public class ToroidalUniverse : NetworkBehaviour {
    public bool isForClient;

    public Borders borders;

    private void Update () {
        var pos = transform.position;
        var x = transform.position.x;
        var y = transform.position.y;
        var move = false;

        if (x > borders.rightBorder) {
            pos.x = borders.leftBorder;
            move = true;
        }
        if (x < borders.leftBorder) {
            pos.x = borders.rightBorder;
            move = true;
        }
        if (y > borders.superiorBorder) {
            pos.y = borders.inferiorBorder;
            move = true;
        }
        if (y < borders.inferiorBorder) {
            pos.y = borders.superiorBorder;
            move = true;
        }

        if (move) {
            if (isForClient) {
                if (isLocalPlayer) {
                    CmdChangePosition (pos);
                }
            } else {
                ChangePosition (pos);
            }
        }

    }

    void ChangePosition (Vector3 position) {
        transform.position = position;
    }

    [Command]
    void CmdChangePosition (Vector3 position) {
        RpcChangePosition (position);
    }

    [ClientRpc]
    void RpcChangePosition (Vector3 position) {
        transform.position = position;
    }

}