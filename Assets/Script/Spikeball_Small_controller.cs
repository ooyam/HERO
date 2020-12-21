using UnityEngine;
using System.Collections;

public class Spikeball_Small_controller : MonoBehaviour
{
    // 回転速度
    private float rotSpeed = 1000f;
    // 移動速度
    private float Speed = 5f;
    //Transformのキャッシュ
    private Transform _transform;
    //Rescue shipのTransformを入れる
    private Vector3 ShipTra;
    //エフェクトを入れる
    public GameObject Effect;

    // Use this for initialization
    void Start()
    {
        //Transformのキャッシュ
        _transform = GetComponent<Transform>();
        //回転を開始する角度を設定
        this.transform.Rotate(0, Random.Range(0, 360), 0);
        //Rescue shipのTransformを取得
        ShipTra = new Vector3(-7.8f, 3.1f,0);
    }

    // Update is called once per frame
    void Update()
    {
        //回転
        this._transform.Rotate(this.rotSpeed * Time.deltaTime, this.rotSpeed * Time.deltaTime, 0);

        // 玉を撃つ
        this._transform.position = Vector3.MoveTowards(this._transform.position, ShipTra, Speed * Time.deltaTime);

        //画面外に出たら破壊
        if (this._transform.position.x < -10.5f || this._transform.position.x > 10.5f || this._transform.position.y < -6 || this._transform.position.y > 6)
        {
            Destroy(this.gameObject);
        }
    }
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Rescue ship")
        {
            //エフェクトを呼び出す
            GameObject effect = Instantiate(Effect);
            effect.transform.position = new Vector3(this._transform.position.x, this._transform.position.y, 3f);
            //破壊
            Destroy(this.gameObject);
        }
    }
    //パーティクル当たり判定
    void OnParticleCollision(GameObject obj)
    {
        //Waveに接触で破壊
        if (obj.gameObject.tag == "Wave")
        {
            //エフェクトを呼び出す
            GameObject effect = Instantiate(Effect);
            effect.transform.position = new Vector3(this._transform.position.x, this._transform.position.y, 3f);
            //破壊
            Destroy(this.gameObject);
        }
    }
}