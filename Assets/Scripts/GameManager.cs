using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class GameManager : MonoBehaviour
{
    private void Awake()
    {
        CreatePlayer();
    }

    void CreatePlayer()
    {
        // ���� ��ġ ������ �迭�� ����
        Transform[] points = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>();
        int idx = Random.Range(1, points.Length);

        // ��Ʈ��ũ�� ĳ���� ����
        PhotonNetwork.Instantiate("Player", points[idx].position, points[idx].rotation, 0);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
