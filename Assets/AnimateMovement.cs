using DG.Tweening;
using System.Collections;
using UnityEngine;

public class AnimateMovement : MonoBehaviour
{
    [Range(-20, 20)]
    public float speed = -15;
    [Range(0, 20)]
    public float duration = 10;
    void Start()
    {

        loop();
    }

    public void loop()
    {
        gameObject.transform.localPosition = new Vector3(16, 3.5f, 0);
        gameObject.transform.DOMoveX(speed, duration, false).OnComplete(() => loop());
    }

}
