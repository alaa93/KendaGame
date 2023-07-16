using UnityEngine.UI;
using UnityEngine;
using System;

public class StoreScreenController : MonoBehaviour
{
    
    [Serializable]
    public struct ItemImage{
        public Items Item;
        public Image image;
    }
    public enum Items {Sword, Hat};
    [Header("Grid")]
    [SerializeField]
    GridLayoutGroup grid;
    [SerializeField]
    GameObject shopItem;
    [SerializeField]
    private ItemImage[] ItemImagePairs;

    private void Awake() {
        
    }
    
}

