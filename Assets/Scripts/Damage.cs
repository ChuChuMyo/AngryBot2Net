using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Player = Photon.Realtime.Player;

public class Damage : MonoBehaviourPun
{
    // 사망 후 투명 처리를 위한 MeshRenderer 컴포넌트의 배열
    private Renderer[] renderers;

    //캐릭터의 초기 생명
    private int initHp = 100;
    //캐릭터의 현재 생명
    public int currHp = 100;

    private Animator anim;
    private CharacterController cc;

    private GameManager gameManager;

    // 애니메이터 뷰에 생성한 파라미터의 해시값 추출
    private readonly int hashDie = Animator.StringToHash("Die");
    private readonly int hashRespawn = Animator.StringToHash("Respawn");

    private void Awake()
    {
        // 캐릭터 모델의 모든 Renderer 컴포넌트를 추출한 후 배열에 할당
        renderers = GetComponentsInChildren<Renderer>();
        anim = GetComponent<Animator>();
        cc = GetComponent<CharacterController>();
        gameManager = GameObject.FindObjectOfType<GameManager>();
        // 현재 생명치를 초기 생명치로 초깃값 설정
        currHp = initHp;
    }

    private void OnCollisionEnter(Collision coll)
    {
        // 생명 수치가 0보다 크고 충돌체의 태그가 Bullet인 경우에 생명 수치를 차감
        if (currHp > 0 && coll.collider.CompareTag("BULLET"))
        {
            currHp -= 20;
            if(currHp <= 0)
            {
                // 자신의 PhotonView 일 때만 메시지를 출력
                if(photonView.IsMine)
                {
                    // 총알의 ActorNumber를 호출
                    var actoNo = coll.collider.GetComponent<Bullet>().actorNumber;
                    // ActorNumber로 현재 룸에 입장한 플레이어를 추출
                    Player lastShootPlayer = PhotonNetwork.CurrentRoom.GetPlayer(actoNo);

                    // 메시지 출력을 위한 문자열 포맷
                    string msg = string.Format
                        ("\n<color=#00ff00>{0}</color> is killed by <color=#ff0000>{1}</color>", 
                        photonView.Owner.NickName, 
                        lastShootPlayer.NickName);

                    photonView.RPC("KillMassage", RpcTarget.AllBufferedViaServer, msg);
                }
                StartCoroutine(PlayerDie());
            }
        }
    }

    [PunRPC]
    void KillMassage(string msg)
    {
        // 메시지 출력
        gameManager.msgList.text += msg;
    }

    IEnumerator PlayerDie()
    {
        // CharacterController 컴포넌트 비활성화
        cc.enabled = false;
        // 리스폰 비활성화
        anim.SetBool(hashRespawn, false);
        // 캐릭터 사망 애니메이션 실행
        anim.SetTrigger(hashDie);

        yield return new WaitForSeconds(3.0f);

        // 리스폰 활성화
        anim.SetBool(hashRespawn, true);

        // 캐릭터 투명 처리
        SetPlayerVisible(false);

        yield return new WaitForSeconds(1.5f);

        // 생성 위치를 재조정
        Transform[] points = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>();
        int idx = Random.Range(1, points.Length); // 0번은 부모
        transform.position = points[idx].position;

        // 리스폰 시 생명 초깃값 설정
        currHp = 100;
        // 캐릭터를 다시 보이게 처리
        SetPlayerVisible(true);
        // CharacterController 컴포넌트 활성화
        cc.enabled = true;
    }

    void SetPlayerVisible(bool isVisible)
    {
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].enabled = isVisible;
        }
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
