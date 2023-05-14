using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spineBoneFollower : MonoBehaviour
{
    [SerializeField]
    Transform Bone2Follow;


    // Update is called once per frame
    void Update()
    {
        transform.position = Bone2Follow.position;
    }
}
