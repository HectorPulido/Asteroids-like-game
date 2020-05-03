using System.Collections;
using System.Collections.Generic;
using EvolutionaryPerceptron.MendelMachine;
using UnityEngine;

public class AsteroidManager : MendelMachine {
    public static AsteroidManager asteroidManager;

    public float lifeTime = 100;
    public Transform playerSpawner;

    int index = 0; //Just one way to change the generation
    //Init all variables
    protected override void Start () {
        if (asteroidManager != null) {
            Destroy (this);
            return;
        }
        asteroidManager = this;
        base.Start ();
        StartCoroutine (InstantiateBotCoroutine ());
        index = individualsPerGeneration;
    }
    //When a bot die
    public override void NeuralBotDestroyed (Brain neuralBot) {
        //Consolidate the fitness
        print("bot Fitness: " + neuralBot.Fitness);
        base.NeuralBotDestroyed (neuralBot);

        //Doo some cool stuff, read the examples
        Destroy (neuralBot.gameObject); //Don't forget to destroy the gameObject

        index--;
        if (index <= 0) {
            Save (); //don't forget to save when you change the generation
            population = Mendelization ();
            generation++;
            StartCoroutine (InstantiateBotCoroutine ());
            index = this.individualsPerGeneration;
        } else {
            StartCoroutine (InstantiateBotCoroutine ());
        }
    }
    //You can instantiate one, two, what you want
    IEnumerator InstantiateBotCoroutine () {
        InstantiateBot (population[index], lifeTime, playerSpawner, index); // A way to instantiate
        yield return null;
    }
}