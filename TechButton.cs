using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TechButton : MonoBehaviour
{
    public GameObject techName;
    int level;
    // Start is called before the first frame update
    void Start()
    {
        level = 1;
        if(transform.parent.transform.localPosition.y<-1)
        {
            level = 2;
        }
        int playerLevel = PlayerPrefs.GetInt(techName.GetComponent<TextMeshProUGUI>().text+"Level",0);
        if(playerLevel==level-1)
        {
            GetComponent<Button>().interactable = true;
        }
        else if(playerLevel>=level)
        {
            transform.parent.GetComponent<UnityEngine.UI.Image>().color = Color.green;
            GetComponent<Button>().interactable = false;
        }
        else if(playerLevel<level-1)
        {
            GetComponent<Button>().interactable = false;

        }
    }
    void FixedUpdate()
    {
        int playerLevel = PlayerPrefs.GetInt(techName.GetComponent<TextMeshProUGUI>().text+"Level",0);
        if(playerLevel==level-1)
        {
            GetComponent<Button>().interactable = true;
        }
        else if(playerLevel>=level)
        {
            transform.parent.GetComponent<UnityEngine.UI.Image>().color = Color.green;
            GetComponent<Button>().interactable = false;
        }
        else if(playerLevel<level-1)
        {
            GetComponent<Button>().interactable = false;

        }
    }

    public void Upgrade()
    {
        int requiredMoney = int.Parse(GetComponentInChildren<TextMeshProUGUI>().text[1..]);
        if(GameManager.money>requiredMoney)
        {
            GameObject.FindWithTag("Warning").GetComponent<TextMeshProUGUI>().text = "";
            GameManager.money-=requiredMoney;
            transform.parent.GetComponent<UnityEngine.UI.Image>().color = Color.green;
            PlayerPrefs.SetInt(techName.GetComponent<TextMeshProUGUI>().text+"Level",level);
        }
        else
        {
            GameObject.FindWithTag("Warning").GetComponent<TextMeshProUGUI>().text = "Insufficient Fund!";
        }
    }
}
