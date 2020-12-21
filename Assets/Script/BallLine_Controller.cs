using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallLine_Controller : MonoBehaviour
{
    //回転体回転速度
    private float rotSpeed = 50f;
    //Transformを入れる
    private Transform _transform;

    // Start is called before the first frame update
    void Start()
    {
        _transform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        _transform.Rotate(0, this.rotSpeed * Time.deltaTime, 0, Space.World);
    }
}
