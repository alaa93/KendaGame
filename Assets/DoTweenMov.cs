using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoTweenMov : MonoBehaviour
{

    public float speed = -15;
    public float duration = 10;
    // Start is called before the first frame update
    void Start()
    {
        transform.DOMoveX(speed,duration,false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
