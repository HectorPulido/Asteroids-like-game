using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[System.Serializable]
public struct Borders {
    public float superiorBorder, inferiorBorder, leftBorder, rightBorder;
}

public class ToroidalUniverse : NetworkBehaviour {
    public Borders borders;

    [ServerCallback]
    private void Update () {
        var pos = transform.position;
        var x = transform.position.x;
        var y = transform.position.y;

        if (x > borders.rightBorder) {
            pos.x = borders.leftBorder;
            RpcChangePosition(pos);
        }
        if (x < borders.leftBorder) {
            pos.x = borders.rightBorder;
            RpcChangePosition(pos);
        }

        if (y > borders.superiorBorder) {
            pos.y = borders.inferiorBorder;
            RpcChangePosition(pos);
        }
        if (y < borders.inferiorBorder) {
            pos.y = borders.superiorBorder;
            RpcChangePosition(pos);
        }
    }

    [ClientRpc]
    void RpcChangePosition (Vector3 position) {
        transform.position = position;
    }

}