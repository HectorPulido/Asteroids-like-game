using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Mirror;
using UnityEngine;
using Mirror.Websocket;

public class AutoConnect : MonoBehaviour {
    public string ip;
    public int port;
    // Start is called before the first frame update
    void Awake () {
        var nm = GetComponent<NetworkManager> ();
        var t = GetComponent<WebsocketTransport> ();
        if (NetworkManager.isHeadless) {
            var tempPort = port;

            if (!int.TryParse (
                    Environment.GetEnvironmentVariable ("PORT"),
                    out tempPort)) {
                tempPort = port;
            }
            t.port = tempPort;
        } else {
            t.port = port;
            nm.networkAddress = ip;
            nm.StartClient();
        }

    }

}