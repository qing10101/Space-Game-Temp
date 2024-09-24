using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MissionModeController : MonoBehaviour
{
    public GameObject panel;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void GoUp()
    {
        if(panel.transform.localPosition.y >= -4000f)
            panel.transform.localPosition -= new Vector3(0f, 250f, 0f);
    }
    public void GoDown()
    {
        if(panel.transform.localPosition.y <= 2000f)
            panel.transform.localPosition += new Vector3(0f, 250f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        float wheel = Input.GetAxis("Mouse ScrollWheel");
        if (wheel > 0 && panel.transform.localPosition.y <= 2000f)
        {
            panel.transform.position += new Vector3(0f, wheel, 0f);
        }
        else if(wheel < 0 && panel.transform.localPosition.y>=-4000f)
        {
            panel.transform.position += new Vector3(0f, wheel, 0f);
        }
    }
    public void Mission1()
    {
        GameManager.manager.StartCoroutine(GameManager.manager.MissionModeSceneLoaded(GameManager.manager.myHeavyCruiser,null,GameManager.manager.mfoeTanker,null,0,3,0));
        GameManager.isMissionMode = true;
        SceneManager.LoadScene("SampleScene",LoadSceneMode.Single);
    }
    public void Mission2()
    {
        GameManager.manager.StartCoroutine(GameManager.manager.MissionModeSceneLoaded(GameManager.manager.myLightCruiser,GameManager.manager.friendlyLightCruiser,GameManager.manager.mfoeCA,null,2,1,0));
        GameManager.isMissionMode = true;
        SceneManager.LoadScene("SampleScene",LoadSceneMode.Single);
    }
    public void Mission3()
    {
        GameManager.manager.StartCoroutine(GameManager.manager.MissionModeSceneLoaded(GameManager.manager.myHeavyCruiser,null,GameManager.manager.mfoeCAdef,null,0,1,0));
        GameManager.isMissionMode = true;
        SceneManager.LoadScene("SampleScene",LoadSceneMode.Single);
    }
    public void Mission4()
    {
        GameManager.manager.StartCoroutine(GameManager.manager.MissionModeSceneLoaded(GameManager.manager.myHeavyCruiser,GameManager.manager.mfriendTanker,GameManager.manager.mfoeCL,null,2,4,0));
        GameManager.isMissionMode = true;
        SceneManager.LoadScene("SampleScene",LoadSceneMode.Single);
    }
    public void Mission5()
    {
        GameManager.manager.StartCoroutine(GameManager.manager.MissionModeSceneLoaded(GameManager.manager.myBattleship,null,GameManager.manager.mfoeCAdef,GameManager.manager.mfoeCAdef2,0,1,1));
        GameManager.isMissionMode = true;
        SceneManager.LoadScene("SampleScene",LoadSceneMode.Single);
    }
    public void Mission6()
    {
        GameManager.manager.StartCoroutine(GameManager.manager.MissionModeSceneLoaded(GameManager.manager.myBattleship,GameManager.manager.friendlyHeavyCruiser,GameManager.manager.mfoeTanker,GameManager.manager.mfoeCL,1,4,2));
        GameManager.isMissionMode = true;
        SceneManager.LoadScene("SampleScene",LoadSceneMode.Single);
    }
    public void Mission7()
    {
        GameManager.manager.StartCoroutine(GameManager.manager.MissionModeSceneLoaded(GameManager.manager.myHeavyCruiser,GameManager.manager.friendlyHeavyCruiser,GameManager.manager.mfoeCA,GameManager.manager.mfoeCL,1,1,2));
        GameManager.isMissionMode = true;
        SceneManager.LoadScene("SampleScene",LoadSceneMode.Single);
    }
}
