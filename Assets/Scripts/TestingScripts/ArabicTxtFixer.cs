using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ArabicTxtFixer : MonoBehaviour
{
    public TextMeshProUGUI txt;
    private void Start()
    {
        txt.text = ArabicSupport.ArabicFixer.Fix(txt.text);
    }
}
