using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAim : MonoBehaviour
{
    [SerializeField]
    private Transform tr_Camera = null;
    [SerializeField]
    private Transform tr_Gun = null;
    [SerializeField]
    private Image aimImage = null;
    [SerializeField]
    private FireButton fireButton = null;
    [SerializeField]
    private Text magazineText = null;

    [SerializeField]
    private AudioClip shotSound = null;
    [SerializeField]
    private AudioClip reloadSound = null;
    [SerializeField]
    private GameObject fireEffect = null;
    [SerializeField]
    private GameObject hitEffect = null;
    [SerializeField]
    private float fireDelay = 0.05f;
    [SerializeField]
    private float reloadTime = 2.0f;
    [SerializeField]
    private int magazineSize = 30;

    private AudioSource m_audioSource = null;

    private Vector3 fire_yCorrection = new Vector3(0.0f, 0.04f, 0.0f);
    private const string enemyTag = "Enemy";
    private int remainBullet;
    private float stackFireDelay;
    private float stackReloadTime;

    void Start()
    {
        m_audioSource = GetComponent<AudioSource>();
        stackFireDelay = 0.0f;
        stackReloadTime = 0.0f;
        remainBullet = magazineSize;
        if(magazineText != null)
        {
            magazineText.text = remainBullet + " / " + magazineSize;
        }
    }

    void Update()
    {
        // 게임이 진행 중일 때만 로직을 실행
        if (tr_Camera != null &&
            GameManager.GetInstance().is_GameStart)
        {
            // 적이 조준선에 잡히면 조준점을 빨갛게 표시
            RaycastHit hitInfo;
            if (Physics.Raycast(tr_Camera.position, tr_Camera.forward, out hitInfo, float.MaxValue))
            {
                if (hitInfo.transform.CompareTag(enemyTag))
                {
                    if (aimImage != null)
                    {
                        aimImage.color = Color.red;
                    }
                }
            }
            else
            {
                if (aimImage != null)
                {
                    aimImage.color = Color.white;
                }
            }

            stackFireDelay += Time.deltaTime;

            if(fireButton.is_Pressed)
            {
                GunFire();
                stackReloadTime = 0.0f;
            }
            else if(remainBullet < magazineSize)
            {
                stackReloadTime += Time.deltaTime;
                if(stackReloadTime >= reloadTime)
                {
                    if(m_audioSource != null &&
                        reloadSound != null)
                    {
                        m_audioSource.PlayOneShot(reloadSound);
                    }

                    remainBullet = magazineSize;
                    if (magazineText != null)
                    {
                        magazineText.text = remainBullet + " / " + magazineSize;
                    }

                    stackReloadTime = 0.0f;
                }
            }
        }
    }

    public void GunFire()
    {
        // 게임이 실행 중일 때만 로직을 실행
        if(GameManager.GetInstance().is_GameStart)
        {
            if (stackFireDelay >= fireDelay &&
                remainBullet > 0)
            {
                RaycastHit hitInfo;
                if (Physics.Raycast(tr_Camera.position, tr_Camera.forward, out hitInfo, float.MaxValue))
                {
                    if (hitInfo.transform.CompareTag(enemyTag))
                    {
                        if (m_audioSource != null &&
                            shotSound != null)
                        {
                            m_audioSource.PlayOneShot(shotSound);
                        }

                        if(fireEffect != null)
                        {
                            Instantiate(fireEffect, tr_Gun.position + tr_Gun.forward * 0.225f + fire_yCorrection, Quaternion.identity);
                        }
                        if(hitEffect != null)
                        {
                            Instantiate(hitEffect, hitInfo.point, Quaternion.identity);
                        }

                        Enemy enemy = hitInfo.transform.GetComponent<Enemy>();
                        if(enemy != null)
                        {
                            enemy.OnHit(1);
                        }
                        remainBullet--;
                        if (magazineText != null)
                        {
                            magazineText.text = remainBullet + " / " + magazineSize;
                        }

                        stackFireDelay = 0.0f;
                    }
                }
            }
        }
    }

    // 디버깅용 조준선
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(tr_Camera.position, tr_Camera.forward * 100);
    }
}
