using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance_Manager = null;

    [SerializeField]
    private GameObject playUIObject;
    [SerializeField]
    private GameObject readyUIObject;
    [SerializeField]
    private GameObject gameOverUIObject;
    [SerializeField]
    private Image gameOverBackground;
    [SerializeField]
    private Text gameOverResultText;
    [SerializeField]
    private Text gameOverIntroText;
    [SerializeField]
    private Transform tr_Player;
    [SerializeField]
    private Image playerHPGauge;

    [SerializeField]
    private AudioClip bgm;
    [SerializeField]
    private int maxHP = 5;

    private AudioSource audioSource;
    private int remainHP;

    public bool is_GameStart { get; private set; }

    void Start()
    {
        if(instance_Manager == null)
        {
            instance_Manager = this;
        }

        audioSource = GetComponent<AudioSource>();
        remainHP = maxHP;
        is_GameStart = false;
    }

    void Update()
    {
        
    }

    public static GameManager GetInstance()
    {
        if(instance_Manager == null)
        {
            return null;
        }

        return instance_Manager;
    }

    public void GameStart()
    {
        if(playUIObject != null)
        {
            playUIObject.SetActive(true);
        }
        if(readyUIObject != null)
        {
            readyUIObject.SetActive(false);
        }

        if(audioSource != null &&
            bgm != null)
        {
            audioSource.clip = bgm;
            audioSource.loop = true;
            audioSource.volume = 0.33f;
            audioSource.Play();
        }

        is_GameStart = true;
    }

    public void GameClear()
    {
        is_GameStart = false;
        gameOverResultText.text = "Fiend Defeated!";
        gameOverResultText.color = Color.blue;

        StartCoroutine(GameOverFadeIn());
    }

    public void GameFailed()
    {
        is_GameStart = false;
        gameOverResultText.text = "You Defeated...";
        gameOverResultText.color = Color.red;

        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject gameObject in gameObjects)
        {
            BossEnemy boss = gameObject.GetComponent<BossEnemy>();
            if (boss != null)
            {
                boss.Win();
            }
        }

        StartCoroutine(GameOverFadeIn());
    }

    public Transform GetPlayerTransform()
    {
        return tr_Player;
    }

    public void TakeDamage(int value)
    {
        remainHP -= value;

        if(playerHPGauge != null)
        {
            playerHPGauge.fillAmount = (float)remainHP / (float)maxHP;
        }

        if(remainHP <= 0)
        {
            GameFailed();
        }
    }

    public void TakeHeal(int value)
    {
        remainHP += value;

        if(remainHP > maxHP)
        {
            remainHP = maxHP;
        }

        if (playerHPGauge != null)
        {
            playerHPGauge.fillAmount = (float)remainHP / (float)maxHP;
        }
    }

    private IEnumerator GameOverFadeIn()
    {
        float newAlpha = 0.0f;
        
        Color backgroundColor = gameOverBackground.color;
        backgroundColor.a = newAlpha;
        gameOverBackground.color = backgroundColor;

        Color resultTextColor = gameOverResultText.color;
        resultTextColor.a = newAlpha;
        gameOverResultText.color = resultTextColor;

        Color introTextColor = gameOverIntroText.color;
        introTextColor.a = newAlpha;
        gameOverIntroText.color = introTextColor;

        gameOverBackground.gameObject.SetActive(true);

        while(newAlpha < 1.0f)
        {
            newAlpha += Time.deltaTime;

            backgroundColor.a = newAlpha;
            gameOverBackground.color = backgroundColor;

            resultTextColor.a = newAlpha;
            gameOverResultText.color = resultTextColor;

            introTextColor.a = newAlpha;
            gameOverIntroText.color = introTextColor;

            yield return null;
        }

        yield return new WaitForSeconds(1.0f);

        AsyncOperation async = SceneManager.LoadSceneAsync("MainMenu");
        async.allowSceneActivation = true;
    }
}
