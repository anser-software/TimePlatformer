using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformBreakable : MonoBehaviour
{

    [SerializeField]
    private GameObject wholeMesh, partParent;
    
    private bool activated;

    private PlatformPart[] parts;

    private bool reverse;

    void Start()
    {
        parts = partParent.transform.GetComponentsInChildren<PlatformPart>();
    }

    public void Activate()
    {
        if (activated)
            return;

        Destroy(wholeMesh);

        activated = true;

        foreach (var part in parts)
        {
            part.Activate();
        }
    }

}
