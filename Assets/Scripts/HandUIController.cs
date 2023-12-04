using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HandUIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI instructionText;

    public void SetInformation(string txt)
    {
        if(txt == "")
        {
            this.gameObject.gameObject.SetActive(false);
            return;
        }
        this.gameObject.gameObject.SetActive(true);
        instructionText.text = txt;    
    }
}
