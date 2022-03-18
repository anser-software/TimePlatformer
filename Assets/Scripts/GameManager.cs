using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class GameManager : MonoBehaviour
{

    public static GameManager instance { get; private set; }

    public int score { get; private set; }

    [SerializeField]
    private float minCorrectPathDist;

    private Platform[] allPlatforms;

    private int maxCheckedPlatformIndex = -1;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        TimeState.instance.OnPause += CheckPlatformPlacement;

        allPlatforms = FindObjectsOfType<Platform>().OrderBy(p => p.transform.position.x).ToArray();
    }

    private void CheckPlatformPlacement()
    {
        var maxPlatform = allPlatforms.First(p => p.transform.position.x > PlayerMove.instance.transform.position.x);
        var thisPlatform = allPlatforms.ToList().IndexOf(maxPlatform);
        if (maxCheckedPlatformIndex < thisPlatform)
        {
            maxPlatform.CheckCorrectPos();
            maxCheckedPlatformIndex = thisPlatform;
        }
    }

    public void AddScore(float distance)
    {
        score += Mathf.RoundToInt(Mathf.Lerp(5, 1, Mathf.InverseLerp(0F, minCorrectPathDist, distance)));

        Debug.Log(string.Format("Score = {0}", score));
    }

}
