using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySight : MonoBehaviour
{
    private NavMeshAgent navAgent = default;
    private RaycastHit[] raycastHits = new RaycastHit[10];
    private DestinationController destinationController;
    private Animator animator;
    [SerializeField] private float sightAngle = 60;
    [SerializeField] private GameObject head;
    private NavMeshHit navHit;
    private EnemyAttack enemyAttack;
    // Start is called before the first frame update
    private void Start()
    {
        navAgent = GetComponentInParent<NavMeshAgent>();
        animator = GetComponentInParent<Animator>();
        animator.SetTrigger("MoveForward");
        enemyAttack = GetComponent<EnemyAttack>();
    }

    private void OnTriggerStay(Collider other)
    {
        ChacePlayer(other);
    }

    //視界に入ったら追いかける
    public void ChacePlayer(Collider other)
    {
        if (other.tag =="Player")
        {
            var playerDirection = other.transform.position - transform.position;　　//プレイヤーとの距離
            //var distance = playerDirection.magnitude;　　//プレイヤーの向き
            //var direction = playerDirection.normalized;　　//プレイヤーとの距離を正規化
            //var hitCount = Physics.RaycastNonAlloc(transform.position, direction, raycastHits, distance);　　//Rayを飛ばして当たったオブジェクトの数を格納する
            var angle = Vector3.Angle(transform.forward, playerDirection);　　//敵の前方からのプレイヤーの向き
            Vector3 target = other.transform.position;
            head.transform.LookAt(target);
            //プレイヤーの向きが視界の範囲内かつ障害物がなければ反応
            if (navAgent.Raycast(target, out navHit) && animator.GetBool("Dead") == false)
            {
                return;
            }
            else if(angle <= sightAngle)
            {
                navAgent.destination = other.transform.position;
            }
        }
    }

    //コライダーから出たら追いかけるのをやめる
    public void ChaceEnd(Collider other)
    {
        if (other.tag == "Player")
        {
            animator.SetTrigger("MoveForward");
            navAgent.destination = destinationController.destination;
        }
    }
}
