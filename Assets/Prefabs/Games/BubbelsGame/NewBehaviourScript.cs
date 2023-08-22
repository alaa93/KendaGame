using KendaGame.Helpers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField]
    GameObject[] gameObjects;
    void Awake()
    {
        int activeLetter = LettersHelper.GetActiveLetterIndex();
        if(activeLetter<8){
        Instantiate(gameObjects[0]);
        }
        else{
        Instantiate(gameObjects[1]);
        }
    }

}
