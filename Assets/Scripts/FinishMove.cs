using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishMove : MonoBehaviour
{
    public Vector2 pos
    {
        get
        {
            return (this.transform.position);
        }
        set
        {
            this.transform.position = value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 tempPos = pos;
        tempPos.x += 1 * Time.deltaTime;
        pos = tempPos;
    }

}
