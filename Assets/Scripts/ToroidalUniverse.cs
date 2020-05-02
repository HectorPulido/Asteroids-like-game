using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Borders {
    public float superiorBorder, inferiorBorder, leftBorder, rightBorder;
}

public class ToroidalUniverse : MonoBehaviour {
    public Borders borders;

    void Update () {
        var pos = transform.position;
        var x = transform.position.x;
        var y = transform.position.y;
        
        if (x > borders.rightBorder) {
            pos.x = borders.leftBorder;
            transform.position = pos;
        }
        if (x < borders.leftBorder) {
            pos.x = borders.rightBorder;
            transform.position = pos;
        }

        if (y > borders.superiorBorder) {
            pos.y = borders.inferiorBorder;
            transform.position = pos;
        }
        if (y < borders.inferiorBorder) {
            pos.y = borders.superiorBorder;
            transform.position = pos;
        }
    }
}