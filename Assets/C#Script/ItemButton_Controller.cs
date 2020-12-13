using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemButton_Controller : MonoBehaviour
{
    private RectTransform myTra;

    // Start is called before the first frame update
    void Start()
    {
        myTra = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void ButtonDown()
    {
        //ItemButtonを画面外に押し出す
        myTra.anchoredPosition = new Vector2(0, 100);
    }
}
