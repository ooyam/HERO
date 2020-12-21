using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartScreen_Text_Controller : MonoBehaviour
{
    //自身のTextを入れる
    private Text _text;
    //始めの色を記憶
    private Color TextColor;

    // Start is called before the first frame update
    void Start()
    {
        //自身のTextを取得
        _text = GetComponent<Text>();
        //始めの色を記憶
        TextColor = _text.color;
        StartCoroutine("ColorCoroutine");
    }

    // Update is called once per frame
    void Update()
    {

    }
    IEnumerator ColorCoroutine()
    {
        while (true)
        {
            _text.color = this.TextColor;
            yield return new WaitForSecondsRealtime(0.5f);
            _text.color = new Color(255, 255, 255, 0);
            yield return new WaitForSecondsRealtime(0.5f);
            if (Input.GetMouseButtonDown(0))
            {
                break;
            }
        }
    }
}
