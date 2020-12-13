using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeth_Effect_Controller : MonoBehaviour
{
    private float delta;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        delta += Time.deltaTime;
        if (delta >= 1f)
        {
            Destroy(this.gameObject);
        }
    }
}
