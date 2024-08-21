using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private float fireLate;
    [SerializeField] private int capacity;
    [SerializeField] private float reloadTime;
    [SerializeField] private GameObject barrel;
    [SerializeField] private SphereCollider rangeCollider;
    [SerializeField] private GameObject muzzleFlash;
    [SerializeField] private GameObject hitEffect;
    [SerializeField] private float fireTime;
    private int shootedBullet = 0;
    private float currentTime = 0;
    private float shootRange = 0;
    private NavMeshAgent navAgent = default;
    private Animator animator;
    private RaycastHit[] raycastHits = new RaycastHit[10];
    private GameObject muzzleFlashParticle;
    private GameObject hitEffectParticle;
    private NavMeshHit navHit;
    // Start is called before the first frame update
    void Start()
    {
        navAgent = GetComponentInParent<NavMeshAgent>();
        animator = GetComponentInParent<Animator>();
        rangeCollider = GetComponent<SphereCollider>();
        shootRange = rangeCollider.radius;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            //var playerDirection = other.transform.position - transform.position;  //プレイヤーとの距離
            //var distance = playerDirection.magnitude;  //プレイヤーの向き
            //var direction = playerDirection.normalized;  //プレイヤーとの距離を正規化
            //var hitCount = Physics.RaycastNonAlloc(transform.position, direction, raycastHits, distance);  //Rayを飛ばして当たったオブジェクトの数を格納する
            Vector3 target = other.transform.position;
            this.transform.LookAt(target);
            if (navAgent.Raycast (target, out navHit))
            {
                return;
            }
            else
            {
                animator.SetTrigger("Idle");
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        ShootPlayer(other);
    }

    private void OnTriggerExit(Collider other)
    {
        navAgent.isStopped = false;
    }

    public void ShootPlayer(Collider other)
    {
        if (other.tag == "Player")
        {
            navAgent.isStopped = true;
            //var playerDirection = other.transform.position - transform.position;　　//プレイヤーとの距離
            //var distance = playerDirection.magnitude;　　//プレイヤーの向き
            //var direction = playerDirection.normalized;　　//プレイヤーとの距離を正規化
            //var hitCount = Physics.RaycastNonAlloc(transform.position, direction, raycastHits, distance);　　//Rayを飛ばして当たったオブジェクトの数を格納する
            Vector3 target = other.transform.position;
            this.transform.LookAt(target);
            //障害物がなければ射撃
            if (navAgent.Raycast(target, out navHit))
            {
                return;
            }
            else
            {
                if (capacity == shootedBullet)
                {
                    Reload();
                }
                else if (animator.GetBool("Dead") == false)
                {
                    BulletShot(other);
                }
            }
        }
    }

    //プレイヤーの方を向いて射撃
    private void BulletShot(Collider other)
    {
        navAgent.isStopped = true;
        currentTime += Time.deltaTime;
        if (fireLate <= currentTime)
        {
            currentTime = 0;
            animator.SetTrigger("Fire");
            Ray bulletLine = new Ray(barrel.transform.position, transform.forward);
            RaycastHit hit;
            if (Physics.Raycast(bulletLine, out hit, shootRange))
            {
               MuzzleFlash();
               HitEffect(hit);
            }
            shootedBullet++;
        }
        DeleteParticle();
    }

    //リロード
    private void Reload()
    {
        currentTime += Time.deltaTime;
        if(reloadTime <= currentTime)
        {
            shootedBullet = 0;
        }
    }

    private void HitEffect(RaycastHit hit)
    {
        if (hitEffect != null)
        {
            if (hitEffectParticle != null)
            {
                hitEffectParticle.transform.position = hit.point;
                hitEffectParticle.transform.rotation = Quaternion.FromToRotation(Vector3.forward, hit.normal);
                hitEffectParticle.SetActive(true);
            }
            else
            {
                hitEffectParticle = Instantiate(hitEffect, hit.point, Quaternion.identity);
            }
        }
    }

    private void MuzzleFlash()
    {
        if (muzzleFlash != null)
        {
            if (muzzleFlashParticle != null)
            {
                muzzleFlashParticle.SetActive(true);
            }
            else
            {
                muzzleFlashParticle = Instantiate(muzzleFlash, barrel.transform.position, barrel.transform.rotation);
                muzzleFlashParticle.transform.SetParent(barrel.transform);
            }
        }

    }

    private void DeleteParticle()
    {
        if(fireTime <= currentTime)
        {
            if (muzzleFlashParticle != null)
            {
                muzzleFlashParticle.SetActive(false);
            }
            if (hitEffectParticle != null)
            {
                if (hitEffectParticle.activeSelf)
                {
                    hitEffectParticle.SetActive(false);
                }
            }
        }
    }
}
