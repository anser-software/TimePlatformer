using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using System;
public class GameManager : MonoBehaviour
{

    public static GameManager instance { get; private set; }

    public Action<bool> OnCompleted;

    public int score { get; private set; }

    public int currentLevel { get; private set; }

    [SerializeField]
    private float minCorrectPathDist;

    [SerializeField]
    private float timeScale;

    private Platform[] allPlatforms;

    private int maxCheckedPlatformIndex = -1;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        TimeState.instance.OnPause += CheckPlatformPlacement;

        currentLevel = SceneManager.GetActiveScene().buildIndex;

        allPlatforms = FindObjectsOfType<Platform>().OrderBy(p => p.transform.position.x).ToArray();

        Time.timeScale = timeScale;
    }

    private void CheckPlatformPlacement()
    {
        Platform maxPlatform = null;

        try 
        {
            maxPlatform = allPlatforms.First(p => p.transform.position.x > PlayerMove.instance.transform.position.x);
        } catch(System.InvalidOperationException)
        {
            return;
        }

        if (maxPlatform == null)
            return;

        var thisPlatform = allPlatforms.ToList().IndexOf(maxPlatform);
        if (maxCheckedPlatformIndex < thisPlatform)
        {
            maxPlatform.CheckCorrectPos();
            maxCheckedPlatformIndex = thisPlatform;
        }
    }

    public void AddScore(float distance)
    {
        var addition = Mathf.RoundToInt(Mathf.Lerp(5, 1, Mathf.InverseLerp(0F, minCorrectPathDist, distance)));
        score += addition;

        Debug.Log(string.Format("+{0} score", addition));
    }

    public void Win()
    {
        OnCompleted?.Invoke(true);
    }

    public void Lose()
    {
        OnCompleted?.Invoke(false);
    }

}
