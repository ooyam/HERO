using UnityEngine;
using System.Collections;

public class BG_Controller : MonoBehaviour
{
    // スクロール速度
    private float scrollSpeed = -3;
    // 背景終了位置
    private float deadLine = -40;
    // 背景開始位置
    private float startLine = 40f;
    // レベル監視用
    private float Level;
    // start時レベル取得
    private float StartLevel;
    // コルーチン重複防止
    private bool stop;
    //transformをキャッシュする
    private Transform _transform;
    //Materialをキャッシュする
    private Material myMaterial;
    // RenderColorとEmissionColorをセット
    private Color DefaultColor = new Color32(255, 255, 255, 255);
    private Color BlackColor = new Color32(0, 0, 0, 255);
    //他BGのMaterialを入れる
    private Material BgMaterial;
    //次の背景を入れる
    public GameObject NextBG;
    //score_textのスクリプトを入れる
    private score_text_Controller ScoreScr;

    // Use this for initialization
    void Start()
    {
        //transformのキャッシュ
        _transform = GetComponent<Transform>();
        //オブジェクトにアタッチしているMaterialを取得
        this.myMaterial = GetComponent<Renderer>().material;
        //BGのMaterialを取得
        this.BgMaterial = transform.Find("Bg").gameObject.GetComponent<Renderer>().material;
        //score_textのスクリプトを取得
        ScoreScr = GameObject.Find("score_text").GetComponent<score_text_Controller>();
        // start時レベル取得
        this.StartLevel = ScoreScr.Level;
        //StartColorCoroutineを実行
        StartCoroutine(StartColorCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        // 背景を移動する
        _transform.Translate(this.scrollSpeed * Time.deltaTime, 0, 0, Space.World);

        // 画面外に出たら、画面右端に移動する
        if (_transform.position.x < this.deadLine)
        {
            _transform.position = new Vector3(this.startLine, 0, 200);
        }
        //レベル6,11,16～背景更新
        this.Level = ScoreScr.Level;
        if (this.Level % 5 == 1 && this.Level != this.StartLevel && stop == false)
        {
            //EndColorCoroutineを実行
            StartCoroutine(EndColorCoroutine());
            stop = true;
        }
    }
    IEnumerator StartColorCoroutine()
    {
        for (float i = 1f; i >= 0; i -= 0.2f)
        {
            this.myMaterial.color = Color.Lerp(DefaultColor, BlackColor, i);
            this.BgMaterial.color = Color.Lerp(DefaultColor, BlackColor, i);
            yield return new WaitForSecondsRealtime(0.1f);
        }
    }
    IEnumerator EndColorCoroutine()
    {
        for (float i = 1f; i >= 0; i -= 0.2f)
        {
            this.myMaterial.color = Color.Lerp(BlackColor, DefaultColor, i);
            this.BgMaterial.color = Color.Lerp(BlackColor, DefaultColor, i);
            yield return new WaitForSecondsRealtime(0.1f);
        }
        //次のBGを生成する
        GameObject BG = Instantiate(NextBG);
        BG.transform.position = new Vector3(this._transform.position.x, this._transform.position.y, 200);
        //自身を破壊
        Destroy(this.gameObject);
    }
}
