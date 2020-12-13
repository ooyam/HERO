using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScreen_Controller : MonoBehaviour
{
    //StartScreen_1を入れる
    public GameObject Start1;
    //遅延用変数
    private bool Delay = false;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0;
        StartCoroutine("WaitCoroutine");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Delay == true)
        {
            //説明画面1を出力
            GameObject start1 = Instantiate(Start1);
            start1.transform.position = new Vector3(0, 0, -6);
            //自身を破壊
            Destroy(this.gameObject);
        }
    }
    IEnumerator WaitCoroutine()
    {
        yield return new WaitForSecondsRealtime(1.0f);
        Delay = true;
    }
}
