using com.example;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Measure : MonoBehaviour
{
    public Text heightText;
    public Button[] buttons;
    public Text placeHolder;
    public GameObject scrollView;
    public GameObject WidthText;
    public GameObject S_SleeveText;
    public GameObject L_SleeveText;

    public GameObject CheckBtn;
    public GameObject NextBtn;



    public void OnclickHeightText()
    {
        scrollView.SetActive(true);
        // Assign the OnButtonClick method to each button's onClick event
        foreach (Button btn in buttons)
        {
            btn.onClick.AddListener(() => OnButtonClick(btn));
        }
    }

    // Method called when a button is clicked
    void OnButtonClick(Button button)
    {
        // Append the button's name to the heightText
        placeHolder.gameObject.SetActive(false);
        if(button.name == "deleteButton")
        {
            if (heightText.text.Length > 0)
            {
                heightText.text = heightText.text.Substring(0, heightText.text.Length - 1);
            }
        }
        else
        {
            heightText.text += button.name;
        }
    }

    public void UploadHeight() // using btn NextPanelBtn in MeasurementPanel
    {
        SupaManager.Instance.UploadHeight(int.Parse(heightText.text));
    }
    public void ChangeButton()
    {
        CheckBtn.SetActive(false);
        NextBtn.SetActive(true);
    }
    public void OnClickHeightUploadButton()
    {
        scrollView.SetActive(false);
        WidthText.SetActive(true);
        S_SleeveText.SetActive(true);
        L_SleeveText.SetActive(true);
    }
}
