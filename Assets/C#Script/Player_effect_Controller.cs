using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_effect_Controller : MonoBehaviour
{
    private float delta = 0;
    //GameOver_Textのゲームオブジェクト/スクリプトを入れる
    private GameOver_Text_Controller GameOverTextScr;
    private bool GameOver;

    // Start is called before the first frame update
    void Start()
    {
        //GameOver_Textゲームオブジェクト/スクリプトの取得
        GameOverTextScr = GameObject.Find("GameOver_Text").GetComponent<GameOver_Text_Controller>();
    }

    // Update is called once per frame
    void Update()
    {
        delta += Time.deltaTime;
        if(delta >= 0.3f)
        {
            Destroy(this.gameObject);
        }
        ///GameOver状態の監視
        this.GameOver = GameOverTextScr._Text1;
        if (this.GameOver == true && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)))
        {
            Destroy(this.gameObject);
        }
    }
}
