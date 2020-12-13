using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rescueship_Effect_Controller : MonoBehaviour
{
    //Rescue shipを入れる
    private GameObject Ship;
    //Rescue shipのスクリプトを入れる
    private Rescueship_controller ShipScr;
    //Rescue shipのHP監視用変数
    private float ShipHP;

    // Start is called before the first frame update
    void Start()
    {
        //Rescue shipのオブジェクトを取得する
        Ship = GameObject.Find("Rescue ship");
        //Rescue shipのスクリプトを取得する
        ShipScr = Ship.GetComponent<Rescueship_controller>();
    }

    // Update is called once per frame
    void Update()
    {
        //Rescue shipのHPを監視、回復した場合はエフェクト破壊
        ShipHP = ShipScr.HP;
        if(ShipHP > 31)
        {
            Destroy(this.gameObject);
        }
    }
}
