using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{

    public static GameMaster instance;
    public CharacterStats p1Character;
    public CharacterStats p2Character;

    public int p1Score;
    public int p2Score;

    public float p1percentage;
    public float p2percentage;

    public int timer;
    public int timerCount;

    public int p1stocks;

    public int p1cash;

    public float[] p1StatBoosts = new float[5];

    public float[] p2StatBoosts = new float[5];

    public CharacterStats[] characterStats;
    Coroutine timerCO;

    private void Awake()
    {
        if(instance && instance != this)
        {
            Destroy(gameObject);
        } else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

    }

    private void Start()
    {
        timerCount = timer;
    }

    IEnumerator DecrementTimerCount()
    {
        yield return new WaitForSeconds(1);
        print("decreaseTimerCO happened!");
        if(timerCount <= 0)
        {
            p1cash += p1Score;
            ResetGame();
            SceneManager.LoadScene("PowerupSelect");
        } else
        {
            timerCount--;
            DecreaseTimer();
        }
    }

    void DecreaseTimer()
    {
        print("decreaseTimer called!");
        timerCO = StartCoroutine(DecrementTimerCount());
    }

    public void StartGame()
    {
        print("start called!");
        timerCount = timer;
        if (timerCO != null) StopCoroutine(timerCO);
        RandomlyPickP2Character();
        DecreaseTimer();
    }

    void ResetGame()
    {
        p1Score = 0;
        p2Score = 0;
        timerCount = timer;
    }

    public void IncrementScore(bool p1)
    {
        if (p1) p1Score++; else p2Score++;
    }

    public void UpdateP1Character(string character)
    {
        p1Character = characterStats.Where(x => x.charName == character).FirstOrDefault();
    }

    public void UpdateP2Character(string character)
    {
        p2Character = characterStats.Where(x => x.charName == character).FirstOrDefault();
    }

    public void RandomlyPickP2Character()
    {
        int rng = Random.Range(0, characterStats.Length);
        p2Character = characterStats[rng];
    }

    public void SetP1Percentage(float percent)
    {
        p1percentage = percent;
    }

    public void SetP2Percentage(float percent)
    {
        p2percentage = percent;
    }
}
