using System.Collections;
using System.Collections.Generic;
using EvolutionaryPerceptron;
using UnityEngine;

[RequireComponent (typeof (RaycastLightRadar))]
[RequireComponent (typeof (Player))]
public class ShipPlayer : BotHandler {
    public static ShipPlayer singleton; 

    Player cs;
    RaycastLightRadar lightdar;

    int dimension;

    //Init all variables
    protected override void Start () {
        singleton = this;
        base.Start ();
        cs = GetComponent<Player> ();
        lightdar = GetComponent<RaycastLightRadar> ();
        dimension = lightdar.numberOfRaycast * 2 + 10;
        lastInputs = new double[1, dimension];
    }

    double[, ] lastInputs;

    void Update () {
        var time = Time.deltaTime;
        var inputs = lightdar.GetInputs ();
        var currentInput = new double[1, dimension];
        // Sensor info
        for (var i = 0; i < lightdar.numberOfRaycast; i++) {
            currentInput[0, i] = inputs[i];
        }
        //Speed info
        Vector3 vel = cs.rb.velocity;
        Vector3 velNorm = vel.normalized;

        var velForward = Vector3.Dot (vel, transform.up);
        var velRight = Vector3.Dot (vel, transform.right);

        currentInput[0, lightdar.numberOfRaycast + 0] = velForward;
        currentInput[0, lightdar.numberOfRaycast + 1] = velRight;
        currentInput[0, lightdar.numberOfRaycast + 2] = cs.horizontal;
        currentInput[0, lightdar.numberOfRaycast + 4] = cs.vertical;

        var newInputCount = lightdar.numberOfRaycast + 5;

        for (var i = 0; i < newInputCount; i++) {
            currentInput[0, i + newInputCount] = (lastInputs[0, i] - currentInput[0, i]) * time;
        }

        Debug.DrawRay (transform.position, transform.up * velForward, Color.blue);
        Debug.DrawRay (transform.position, transform.right * velRight, Color.blue);

        lastInputs = (double[, ]) currentInput.Clone ();;
        var output = nb.SetInput (currentInput); //Feed forward
        cs.vertical = (float) output[0, 0];
        cs.vertical = Mathf.Clamp (cs.vertical, -1, 1);
        cs.horizontal = (float) output[0, 1];
        cs.horizontal = Mathf.Clamp (cs.horizontal, -1, 1);
        cs.shooting = output[0, 2] > 0.5;
        //nb.AddFitness (time);
    }

    public void AddFitness(){
        nb.AddFitness(1);
    }

    public void Lose () {
        nb.Destroy ();
    }

}