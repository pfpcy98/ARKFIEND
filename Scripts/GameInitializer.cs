using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class GameInitializer : MonoBehaviour
{
    [Header("Game Objects")]
    [SerializeField]
    private GameObject planeFinder;
    [SerializeField]
    private GameObject demonObject;

    [Header("Resource Variables")]
    [SerializeField]
    private AudioClip magicCircle_InitSound;
    [SerializeField]
    private AudioClip demon_StartCrying;
    [SerializeField]
    private GameObject prefab_Explosion;

    [Header("Value Variables")]
    [SerializeField]
    private float spawnTime = 3;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void ScriptActivate()
    {
        if(audioSource != null &&
            magicCircle_InitSound != null)
        {
            audioSource.PlayOneShot(magicCircle_InitSound);
        }

        if(planeFinder != null)
        {
            planeFinder.SetActive(false);
        }

        if (demonObject != null)
        {
            StartCoroutine(SizeUpAndGameStart());
        }
    }

    IEnumerator SizeUpAndGameStart()
    {
        Vector3 nowScale = new Vector3(0.67f, 0.0f, 0.67f);

        while (nowScale.y < 0.67f)
        {
            demonObject.transform.localScale = nowScale;
            nowScale.y += Time.deltaTime / spawnTime;
            yield return null;
        }

        if (prefab_Explosion != null)
        {
            Instantiate(prefab_Explosion, demonObject.transform);
        }

        if(audioSource != null &&
            demon_StartCrying != null)
        {
            audioSource.PlayOneShot(demon_StartCrying);
        }

        GameManager.GetInstance().GameStart();
    }
}
