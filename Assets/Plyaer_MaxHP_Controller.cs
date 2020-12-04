using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plyaer_MaxHP_Controller : MonoBehaviour
{
    //Plyaerを入れる
    private GameObject Player;
    //Plyaerのtransformを入れる
    private Transform _transform;
    //Plyaerの位置情報を入れる
    private Vector3 PlayerPos;

    // Start is called before the first frame update
    void Start()
    {
        //Playerを取得する
        Player = GameObject.Find("Player");
        //Playerのtransformを取得
        _transform = Player.transform;
    }

    // Update is called once per frame
    void Update()
    {
        //Playerの位置情報を取得
        PlayerPos = _transform.position;
        this.transform.localPosition = new Vector3(PlayerPos.x + 0.5f, PlayerPos.y + 1f, PlayerPos.z);
    }
}
