using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LifeSystem : MonoBehaviour
{
    [SerializeField] private DamageInfo[] damageInfos;
    [System.Serializable]
    private struct DamageInfo
    {
        public GameObject bullet;
        public int bulletDamage;
    }

    [SerializeField] private int life;
    [SerializeField] private int maxLife; // �ő�̗�
    [SerializeField] private int shield; // �V�[���h�̒l
    [SerializeField] private GameObject shieldObject; // �V�[���h�I�u�W�F�N�g

    private Animator animator;
    private NavMeshAgent navAgent;

    private bool shieldActive; // �V�[���h����

    void Start()
    {
        animator = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();
        shieldActive = shield > 0;
        if (shieldObject != null)
        {
            shieldObject.SetActive(shieldActive);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        int NexusCount = GameObject.FindGameObjectsWithTag("Nexus").Length;
        int damageMultiplier = 1;

        switch (NexusCount)
        {
            case 4:
                damageMultiplier = 1;
                break;
            case 3:
                damageMultiplier = 2;
                break;
            case 2:
                damageMultiplier = 3;
                break;
            case 1:
                damageMultiplier = 4;
                break;
            default:
                damageMultiplier = 1; // Nexus��0�̏ꍇ�A�_���[�W�{����1�{
                shieldActive = false;
                if (shieldObject != null)
                {
                    shieldObject.SetActive(false); // �V�[���h�I�u�W�F�N�g�𖳌���
                }
                shield = 0;
                Debug.Log("Nexus��0�΂肠nasi");
                break;
        }


        for (int i = 0; i < damageInfos.Length; i++)
        {
            if (collision.gameObject.tag == damageInfos[i].bullet.tag)
            {
                int damageAmount = damageInfos[i].bulletDamage * damageMultiplier;

                if (shieldActive)
                {
                    shield -= damageAmount;
                    if (shield <= 0)
                    {
                        shieldActive = false;
                        Debug.Log("���������");
                        if (shieldObject != null)
                        {
                            shieldObject.SetActive(false); // �V�[���h�I�u�W�F�N�g�𖳌���
                        }
                    }
                }
                else
                {
                    life -= damageAmount;
                }

                if (life <= 0)
                {
                    navAgent.isStopped = true;
                    animator.SetBool("Dead", true);
                    Debug.Log("daai");
                    Destroy(this.gameObject, 2f); // 2�b��ɃI�u�W�F�N�g���폜
                }
            }
        }
    }
}
