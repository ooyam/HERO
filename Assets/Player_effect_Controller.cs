using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_effect_Controller : MonoBehaviour
{
    private float delta = 0;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        delta += Time.deltaTime;
        if(delta >= 0.5f)
        {
            Destroy(this.gameObject);
        }
    }
    public void DestroyObj()
    {
        Destroy(this.gameObject);
    }
}
