using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishTrigger : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        FinishController.instance.Begin();
    }

}
