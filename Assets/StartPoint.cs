using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPoint : MonoBehaviour
{



    void Start()
    {
        FindObjectOfType<Player>().transform.position = transform.position;
    }


    void Update()
    {
        
    }


}
