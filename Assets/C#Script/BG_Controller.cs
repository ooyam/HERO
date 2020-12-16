using UnityEngine;
using System.Collections;

public class BG_Controller : MonoBehaviour
{

    // スクロール速度
    private float scrollSpeed = -3;
    // 背景終了位置
    private float deadLine = -20;
    // 背景開始位置
    private float startLine = 59.8f;
    //Color変更重複防止用変数
    private bool Color1;
    private bool Color2;
    //時間計算用変数
    private float delta;
    //transformをキャッシュする
    private Transform _transform;
    //Materialをキャッシュする
    private Material myMaterial;
    // RenderColorとEmissionColorをセット
    private Color RenderColor = new Color32(255, 155, 155, 255);
    private Color EmissionColor1 = new Color32(15, 0, 0, 0);
    private Color EmissionColor2 = new Color32(30, 0, 0, 0);
    //他BGのMaterialを入れる
    private Material BG_1_2Material;
    private Material BG_2_3Material;
    private Material BG_3_4Material;
    private Material BG_4_1Material;

    // Use this for initialization
    void Start()
    {
        //transformのキャッシュ
        _transform = GetComponent<Transform>();
        //オブジェクトにアタッチしているMaterialを取得
        this.myMaterial = GetComponent<Renderer>().material;
        //BG_0のMaterialを取得
        this.BG_1_2Material = GameObject.Find("BG_1_2").GetComponent<Renderer>().material;
        this.BG_2_3Material = GameObject.Find("BG_2_3").GetComponent<Renderer>().material;
        this.BG_3_4Material = GameObject.Find("BG_3_4").GetComponent<Renderer>().material;
        this.BG_4_1Material = GameObject.Find("BG_4_1").GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        // レベル5,10,15で背景色変更
        if (Color2 == false)
        {
            delta += Time.deltaTime;
            if (delta >= 120f && Color1 == false)
            {
                myMaterial.color = RenderColor;
                myMaterial.SetColor("_EmissionColor", EmissionColor1);
                BG_1_2Material.color = RenderColor;
                BG_1_2Material.SetColor("_EmissionColor", EmissionColor1);
                BG_2_3Material.color = RenderColor;
                BG_2_3Material.SetColor("_EmissionColor", EmissionColor1);
                BG_3_4Material.color = RenderColor;
                BG_3_4Material.SetColor("_EmissionColor", EmissionColor1);
                BG_4_1Material.color = RenderColor;
                BG_4_1Material.SetColor("_EmissionColor", EmissionColor1);
                Color1 = true;
            }
            else if (delta >= 270f && Color2 == false)
            {
                myMaterial.SetColor("_EmissionColor", EmissionColor2);
                BG_1_2Material.SetColor("_EmissionColor", EmissionColor2);
                BG_2_3Material.SetColor("_EmissionColor", EmissionColor2);
                BG_3_4Material.SetColor("_EmissionColor", EmissionColor2);
                BG_4_1Material.SetColor("_EmissionColor", EmissionColor2);
                Color2 = true;
            }
        }
        // 背景を移動する
        _transform.Translate(this.scrollSpeed * Time.deltaTime, 0, 0);

        // 画面外に出たら、画面右端に移動する
        if (_transform.position.x < this.deadLine)
        {
            _transform.position = new Vector3(this.startLine, 0, 200);
        }
    }
}
