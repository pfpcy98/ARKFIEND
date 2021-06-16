using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossEnemy : Enemy
{
    [SerializeField]
    private Image bossHpGauge;
    [SerializeField]
    private AudioClip onDieSound;
    [SerializeField]
    private AudioClip onAttackSound;
    [SerializeField]
    private AudioClip onWinSound;
    [SerializeField]
    private GameObject prefab_FireStone;

    [SerializeField]
    private float spawn_Range_X = 3.0f;
    [SerializeField]
    private float spawn_Range_Y = 3.0f;
    [SerializeField]
    private float init_Spawn_Delay = 3.0f;
    [SerializeField]
    private float max_Spawn_Delay_Decrease = 1.5f;

    private Animator animator;
    private AudioSource audioSource;
    private float initHP;
    private float hpRate;
    private float stack_Spawn_Delay;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        initHP = hp;
        stack_Spawn_Delay = 0.0f;
    }

    void Update()
    {
        // 게임 진행 중, 생존 중일 때만 로직을 수행
        if(GameManager.GetInstance().is_GameStart &&
            hp > 0)
        {
            if(animator.GetInteger("animation") != 1)
            {
                animator.SetInteger("animation", 1);
            }

            // 항상 플레이어를 바라봄
            Vector3 lookPos = GameManager.GetInstance().GetPlayerTransform().position;
            lookPos.y = transform.position.y;

            transform.LookAt(lookPos);

            // 남은 HP 비율을 측정
            hpRate = hp / initHP;

            if (prefab_FireStone != null)
            {
                stack_Spawn_Delay += Time.deltaTime;
                // 화염석 스폰 간격은 체력에 비례하여 감소함
                if (stack_Spawn_Delay > init_Spawn_Delay - (1 - hpRate) * max_Spawn_Delay_Decrease)
                {
                    animator.SetInteger("animation", 3);
                    audioSource.PlayOneShot(onAttackSound);

                    // 화염석 스폰 갯수는 체력 33% 감소 시마다 1개 증가
                    for (float f = 1.0f; f >= hpRate; f -= 0.33f)
                    {
                        Vector3 spawnPos = transform.position;
                        // 스폰 포인트를 좌, 우 및 상단 방향 랜덤으로 벌림
                        spawnPos += transform.right * Random.Range(-spawn_Range_X, spawn_Range_X);
                        spawnPos.y += Random.Range(0, spawn_Range_Y);

                        FireStone fireStone = Instantiate(prefab_FireStone, spawnPos, Quaternion.identity).GetComponent<FireStone>();
                        // 화염석의 속도는 체력이 감소한 만큼 비례하여 증가(최대 2배)
                        fireStone.moveSpeed += fireStone.moveSpeed * (1 - hpRate);
                    }

                    stack_Spawn_Delay = 0.0f;
                }
            }
        }
    }

    public override void OnHit(float dmg)
    {
        hp -= dmg;

        if(bossHpGauge != null)
        {
            bossHpGauge.fillAmount = hp / initHP;
        }

        if(hp <= 0)
        {
            OnDie();
        }
    }

    public override void OnDie()
    {
        if(audioSource != null &&
            onDieSound != null)
        {
            audioSource.PlayOneShot(onDieSound);
        }

        animator.SetInteger("animation", 5);

        GameManager.GetInstance().GameClear();
    }

    public void Win()
    {
        if(audioSource != null &&
            onWinSound != null)
        {
            audioSource.PlayOneShot(onWinSound);
        }
    }
}
