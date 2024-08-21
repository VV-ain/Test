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

    //���E�ɓ�������ǂ�������
    public void ChacePlayer(Collider other)
    {
        if (other.tag =="Player")
        {
            var playerDirection = other.transform.position - transform.position;�@�@//�v���C���[�Ƃ̋���
            //var distance = playerDirection.magnitude;�@�@//�v���C���[�̌���
            //var direction = playerDirection.normalized;�@�@//�v���C���[�Ƃ̋����𐳋K��
            //var hitCount = Physics.RaycastNonAlloc(transform.position, direction, raycastHits, distance);�@�@//Ray���΂��ē��������I�u�W�F�N�g�̐����i�[����
            var angle = Vector3.Angle(transform.forward, playerDirection);�@�@//�G�̑O������̃v���C���[�̌���
            Vector3 target = other.transform.position;
            head.transform.LookAt(target);
            //�v���C���[�̌��������E�͈͓̔�����Q�����Ȃ���Δ���
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

    //�R���C�_�[����o����ǂ�������̂���߂�
    public void ChaceEnd(Collider other)
    {
        if (other.tag == "Player")
        {
            animator.SetTrigger("MoveForward");
            navAgent.destination = destinationController.destination;
        }
    }
}
