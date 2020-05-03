using System.Collections;
using System.Collections.Generic;
using EvolutionaryPerceptron;
using UnityEngine;
public class ShipPlayer : BotHandler {
    Player cs;
    public LayerMask layerMaskForRaycast;
    //Init all variables
    protected override void Start () {
        base.Start ();
        cs = GetComponent<Player> ();
        lastInputs = new double[1, numberOfRaycast];
    }

    double[, ] lastInputs;

    void Update () {
        var inputs = GetInputs ();
        var currentInput = new double[1, numberOfRaycast * 2]; // Sensor info
        for (var i = 0; i < numberOfRaycast; i++) {
            currentInput[0, i] = inputs[0, i];
        }
        for (var i = 0; i < numberOfRaycast; i++) {
            currentInput[0, i + numberOfRaycast] = lastInputs[0, i];
        }
        lastInputs = inputs;
        var output = nb.SetInput (currentInput); //Feed forward
        cs.vertical = (float) output[0, 0];
        cs.vertical = Mathf.Clamp (cs.vertical, -1, 1);
        cs.horizontal = (float) output[0, 1];
        cs.horizontal = Mathf.Clamp (cs.horizontal, -1, 1);
        if (output[0, 2] > 0.5) {
            cs.shooting = true;
            nb.AddFitness (-Time.deltaTime);
        } else {
            cs.shooting = false;
            nb.AddFitness (Time.deltaTime);
        }
    }

    public int numberOfRaycast = 12;

    private double[, ] GetInputs () {

        var inputs = new double[1, numberOfRaycast];
        var currentRotation = transform.eulerAngles.z;

        for (var i = 0; i < numberOfRaycast; i++) {
            var angle = (360f * i) / numberOfRaycast;
            angle += currentRotation;
            var direction = Quaternion.AngleAxis (angle, Vector3.forward) * Vector3.right;
            direction = direction.normalized;

            var ray = Physics2D.Raycast (transform.position, direction, 999999, layerMaskForRaycast);
            var length = ray.collider != null ? ray.distance / 999999 : 1;

            inputs[0, i] = length;

            Debug.DrawRay (transform.position, direction * length * 999999, Color.green);
        }
        return inputs;
    }

    public void Lose () {
        nb.Destroy ();
    }

}