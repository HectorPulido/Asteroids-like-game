using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager {
    public static float Vertical {
        get { return Input.GetAxis ("Vertical"); }
    }
    public static float Horizontal {
        get { return Input.GetAxis ("Horizontal"); }
    }
    public static bool FireDown {
        get { return Input.GetKeyDown (KeyCode.Space); }
    }
}