using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Controller : MonoBehaviour
{
    public void ManualGoHome()
    {
        //SceneManager.LoadScene("BattleOptions",LoadSceneMode.Single);
        if(!GameManager.isMissionMode)
            SceneManager.LoadScene("Map",LoadSceneMode.Single);
        else
        {
            SceneManager.LoadScene("MissionMode",LoadSceneMode.Single);
        }
    }
    void Awake()
    {
        StopAllCoroutines();
        GameObject.FindWithTag("ELost").GetComponent<TextMeshProUGUI>().text="Lost:\n\nBattleship * "+Test.BBELost+"\n\nHeavy Cruiser * "+Test.CAELost+"\n\nLight Cruiser * "+Test.CLELost+"\n\nFrigate * "+Test.FFELost+"\n\nTanker * "+Test.TTELost;
        GameObject.FindWithTag("YLost").GetComponent<TextMeshProUGUI>().text="Lost:\n\nBattleship * "+Test.BBYLost+"\n\nHeavy Cruiser * "+Test.CAYLost+"\n\nLight Cruiser * "+Test.CLYLost+"\n\nFrigate * "+Test.FFYLost+"\n\nTanker * "+Test.TTYLost;
        GameObject.FindWithTag("EKIA").GetComponent<TextMeshProUGUI>().text="Total Casualty: "+Test.enemyTotalCasualty+" KIA";
        GameObject.FindWithTag("YKIA").GetComponent<TextMeshProUGUI>().text="Total Casualty: "+Test.yourTotalCasualty+" KIA";
    }

    // // Update is called once per frame
    // void Update()
    // {
        
    // }
}
