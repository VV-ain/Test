using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestinationController : MonoBehaviour
{
    //�����ʒu
    private Vector3 startPosition;
    //�ړI�n
    [SerializeField] public Vector3 destination;

    [SerializeField] private Transform[] targets;

    [SerializeField] private int order = 0;

    public enum Route { inOrder, random }
    public Route route;

    // Start is called before the first frame update
    void Start()
    {
        //�@�����ʒu�̐ݒ�
        startPosition = transform.position;
        SetDestination(transform.position);
    }

    public void CreateDestination()
    {
        if (route == Route.inOrder)
        {
            CreateInOrderDestination();
        }
    }

    //targets�ɐݒ肵�����ԂɖړI�n���쐬
    private void CreateInOrderDestination()
    {
        if (order < targets.Length - 1)
        {
            order++;
            SetDestination(new Vector3(targets[order].transform.position.x, targets[order].transform.position.y, targets[order].transform.position.z));
        }
        else
        {
            order = 0;
            SetDestination(new Vector3(targets[order].transform.position.x, targets[order].transform.position.y, targets[order].transform.position.z));
        }
    }

    //�@�ړI�n�̐ݒ�
    public void SetDestination(Vector3 position)
    {
        destination = position;
    }

    //�@�ړI�n�̎擾
    public Vector3 GetDestination()
    {
        return destination;
    }
}
