using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestinationController : MonoBehaviour
{
    //初期位置
    private Vector3 startPosition;
    //目的地
    [SerializeField] public Vector3 destination;

    [SerializeField] private Transform[] targets;

    [SerializeField] private int order = 0;

    public enum Route { inOrder, random }
    public Route route;

    // Start is called before the first frame update
    void Start()
    {
        //　初期位置の設定
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

    //targetsに設定した順番に目的地を作成
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

    //　目的地の設定
    public void SetDestination(Vector3 position)
    {
        destination = position;
    }

    //　目的地の取得
    public Vector3 GetDestination()
    {
        return destination;
    }
}
