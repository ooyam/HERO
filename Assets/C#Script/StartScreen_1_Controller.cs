using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScreen_1_Controller : MonoBehaviour
{
    //StartScreen_2を入れる
    public GameObject Start2;
    //遅延用変数
    private bool Delay = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("WaitCoroutine");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Delay == true)
        {
            //自身を破壊
            Destroy(this.gameObject);
            //説明画面2を出力
            GameObject start2 = Instantiate(Start2);
            start2.transform.position = new Vector3(0, 0, -6);
        }
    }
    IEnumerator WaitCoroutine()
    {
        yield return new WaitForSecondsRealtime(1.0f);
        Delay = true;
    }
}
