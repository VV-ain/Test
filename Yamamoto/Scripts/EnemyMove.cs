using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(DestinationController))]
public class EnemyMove : MonoBehaviour
{
    private NavMeshAgent navAgent = default;
    [SerializeField] private DestinationController destinationController;
    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        destinationController = GetComponent<DestinationController>();
        navAgent.SetDestination(destinationController.GetDestination());
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, destinationController.GetDestination()) < 1.5f)
        {
            destinationController.CreateDestination();
            navAgent.SetDestination(destinationController.GetDestination());
        }
    }
}
