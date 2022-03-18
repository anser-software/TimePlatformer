using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActionTrigger : MonoBehaviour
{

    [SerializeField]
    private UnityAction OnEnter;

    private bool activated;

    private void OnTriggerEnter(Collider other)
    {
        if (activated)
            return;

        activated = true;

        OnEnter?.Invoke();   
    }

}
