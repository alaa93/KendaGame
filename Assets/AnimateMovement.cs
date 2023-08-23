using DG.Tweening;
using UnityEngine;

public class AnimateMovement : MonoBehaviour
{
    [Range(-20, 20)]
    public float speed = -15;
    [Range(0, 20)]
    public float duration = 10;
    void Start()
    {
        transform.DOMoveX(speed,duration,false);
    }
}
