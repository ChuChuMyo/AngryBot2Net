using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks
{
    public TMP_Text roomName;
    public TMP_Text connectInfo;
    public TMP_Text msgList;

    public Button exitBtn;

    private void Awake()
    {
        CreatePlayer();
        // ���� ���� ���� �� ǥ��
        SetRoomInfo();
        // Exit ��ư �̺�Ʈ ����
        exitBtn.onClick.AddListener(() => OnExitClick());
    }

    void CreatePlayer()
    {
        // ���� ��ġ ������ �迭�� ����
        Transform[] points = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>();
        int idx = Random.Range(1, points.Length);

        // ��Ʈ��ũ�� ĳ���� ����
        PhotonNetwork.Instantiate("Player", points[idx].position, points[idx].rotation, 0);
    }

    // �� ���� ������ ���
    void SetRoomInfo()
    {
        Room room = PhotonNetwork.CurrentRoom;
        roomName.text = room.Name;
        connectInfo.text = $"{room.PlayerCount}/{room.MaxPlayers}";
    }

    // Exit ��ư�� OnClick�� ������ �Լ�
    void OnExitClick()
    {
        PhotonNetwork.LeaveRoom();
    }

    // ���� �뿡�� �������� �� ȣ��Ǵ� �ݹ� �Լ�
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("Lobby");
    }

    // ���� �뿡�� �������� �� ȣ��Ǵ� �ݹ��Լ�
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        SetRoomInfo();
        string msg = $"\n<color=#00FF00>{newPlayer.NickName}</color> is joined room";
        msgList.text += msg;
    }

    // �뿡�� ��Ʈ��ũ ������ ���������� ȣ��Ǵ� �ݹ� �Լ�
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        SetRoomInfo();
        string msg = $"\n<color=#00FF00>{ otherPlayer.NickName}</color> is left room";
        msgList.text += msg;
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
