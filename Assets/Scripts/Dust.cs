using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dust : MonoBehaviour
{

    [SerializeField]
    private ParticleSystem dustParticles;

    private void Update()
    {
        var main = dustParticles.main;

        main.simulationSpeed = Mathf.Abs(TimeState.instance.globalTimeScale);
    }

}
