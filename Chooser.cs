using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Chooser : MonoBehaviour
{
    public GameObject BBFrd1, BBFrd2, BBMd, BBBrd1, BBBrd2, CAFrd1, CAFrd2, CAMd, CABrd1, CABrd2, CLMd;
    public static int[] costumeCode;
    void Awake()
    {
        costumeCode = new int[7];
        int storedCode = PlayerPrefs.GetInt("Code", -1);
        if(storedCode != -1)
        {
            for (int i = 0; i < 7; i++)
            {
                costumeCode[7-i-1] = storedCode%10;
                storedCode /= 10;
            }
        }
        Debug.Log("CLM" + costumeCode[6]);
        GameObject.FindWithTag("BBF").GetComponent<TMP_Dropdown>().value = costumeCode[0];
        GameObject.FindWithTag("BBM").GetComponent<TMP_Dropdown>().value = costumeCode[1];
        GameObject.FindWithTag("BBB").GetComponent<TMP_Dropdown>().value = costumeCode[2];
        GameObject.FindWithTag("CAF").GetComponent<TMP_Dropdown>().value = costumeCode[3];
        GameObject.FindWithTag("CAM").GetComponent<TMP_Dropdown>().value = costumeCode[4];
        GameObject.FindWithTag("CAB").GetComponent<TMP_Dropdown>().value = costumeCode[5];
        GameObject.FindWithTag("CLM").GetComponent<TMP_Dropdown>().value = costumeCode[6];
        if (costumeCode[0] == 0)
        {
            BBFrd1.transform.localPosition = new Vector3(0, 0, 0);
            BBFrd2.transform.localPosition = new Vector3(0, 50, 0);
        }
        else if (costumeCode[0] == 1)
        {
            BBFrd1.transform.localPosition = new Vector3(0, 50, 0);
            BBFrd2.transform.localPosition = new Vector3(0, 0, 0);
        }
        if (costumeCode[1] == 0)
        {
            BBMd.transform.localPosition = new Vector3(0, 50, 0);
        }
        else if (costumeCode[1] == 1)
        {
            BBMd.transform.localPosition = new Vector3(0, 0, 0);
        }
        if (costumeCode[2] == 0)
        {
            BBBrd1.transform.localPosition = new Vector3(0, 0, 0);
            BBBrd2.transform.localPosition = new Vector3(0, 50, 0);
        }
        else if (costumeCode[2] == 1)
        {
            BBBrd1.transform.localPosition = new Vector3(0, 50, 0);
            BBBrd2.transform.localPosition = new Vector3(0, 0, 0);
        }
        if (costumeCode[3] == 0)
        {
            CAFrd1.transform.localPosition = new Vector3(0, 0, 0);
            CAFrd2.transform.localPosition = new Vector3(0, 50, 0);
        }
        else if (costumeCode[3] == 1)
        {
            CAFrd1.transform.localPosition = new Vector3(0, 50, 0);
            CAFrd2.transform.localPosition = new Vector3(0, 0, 0);
        }
        if (costumeCode[4] == 0)
        {
            CAMd.transform.localPosition = new Vector3(0, 50, 0);
        }
        else if (costumeCode[4] == 1)
        {
            CAMd.transform.localPosition = new Vector3(0, 0, 0);
        }
        if (costumeCode[5] == 0)
        {
            CABrd1.transform.localPosition = new Vector3(0, 0, 0);
            CABrd2.transform.localPosition = new Vector3(0, 50, 0);
        }
        else if (costumeCode[5] == 1)
        {
            CABrd1.transform.localPosition = new Vector3(0, 50, 0);
            CABrd2.transform.localPosition = new Vector3(0, 0, 0);
        }
        if (costumeCode[6] == 0)
        {
            CLMd.transform.localPosition = new Vector3(0, 50, 0);
        }
        else if (costumeCode[6] == 1)
        {
            CLMd.transform.localPosition = new Vector3(0, 0, 0);
        }
    }
    void OnApplicationQuit()
    {
        int toBeStoredCode = 0;
        for (int i = 0; i < 7; i++)
        {
            toBeStoredCode += costumeCode[i];
            toBeStoredCode *= 10;
        }
        PlayerPrefs.SetInt("Code", toBeStoredCode);
    }

    // // Update is called once per frame
    // void Update()
    // {
        
    // }
    public void Confirm()
    {
        int toBeStoredCode = 0;
        for (int i = 0; i < 7; i++)
        {
            toBeStoredCode *= 10;
            toBeStoredCode += costumeCode[i];
            
        }
        Debug.Log("CLMC"+costumeCode[6]);
        PlayerPrefs.SetInt("Code", toBeStoredCode);
        if(!GameManager.isMissionMode)
            SceneManager.LoadScene("Map",LoadSceneMode.Single);
        else
        {
            SceneManager.LoadScene("MissionMode",LoadSceneMode.Single);
        }

    }
    public void BBFrdTurret(Int32 i)
    {
        if (i == 0)
        {
            BBFrd1.transform.localPosition = new Vector3(0, 0, 0);
            BBFrd2.transform.localPosition = new Vector3(0, 50, 0);
        }
        else if (i == 1)
        {
            BBFrd1.transform.localPosition = new Vector3(0, 50, 0);
            BBFrd2.transform.localPosition = new Vector3(0, 0, 0);
        }
        costumeCode[0] = i;
    }
    public void BBMdTurret(Int32 i)
    {
        if (i == 0)
        {
            BBMd.transform.localPosition = new Vector3(0, 50, 0);
        }
        else if (i == 1)
        {
            BBMd.transform.localPosition = new Vector3(0, 0, 0);
        }
        costumeCode[1] = i;
    }
    public void BBBrdTurret(Int32 i)
    {
        if (i == 0)
        {
            BBBrd1.transform.localPosition = new Vector3(0, 0, 0);
            BBBrd2.transform.localPosition = new Vector3(0, 50, 0);
        }
        else if (i == 1)
        {
            BBBrd1.transform.localPosition = new Vector3(0, 50, 0);
            BBBrd2.transform.localPosition = new Vector3(0, 0, 0);
        }
        costumeCode[2] = i;
    }
    public void CAFrdTurret(Int32 i)
    {
        if (i == 0)
        {
            CAFrd1.transform.localPosition = new Vector3(0, 0, 0);
            CAFrd2.transform.localPosition = new Vector3(0, 50, 0);
        }
        else if (i == 1)
        {
            CAFrd1.transform.localPosition = new Vector3(0, 50, 0);
            CAFrd2.transform.localPosition = new Vector3(0, 0, 0);
        }
        costumeCode[3] = i;
    }
    public void CAMdTurret(Int32 i)
    {
        if (i == 0)
        {
            CAMd.transform.localPosition = new Vector3(0, 50, 0);
        }
        else if (i == 1)
        {
            CAMd.transform.localPosition = new Vector3(0, 0, 0);
        }
        costumeCode[4] = i;
    }
    public void CABrdTurret(Int32 i)
    {
        if (i == 0)
        {
            CABrd1.transform.localPosition = new Vector3(0, 0, 0);
            CABrd2.transform.localPosition = new Vector3(0, 50, 0);
        }
        else if (i == 1)
        {
            CABrd1.transform.localPosition = new Vector3(0, 50, 0);
            CABrd2.transform.localPosition = new Vector3(0, 0, 0);
        }
        costumeCode[5] = i;
    }
    public void CLMdTurret(Int32 i)
    {
        if (i == 0)
        {
            CLMd.transform.localPosition = new Vector3(0, 50, 0);
        }
        else if (i == 1)
        {
            CLMd.transform.localPosition = new Vector3(0, 0, 0);
        }
        costumeCode[6] = i;
    }
    public void GoLeft()
    {
        if(transform.position.x==80)
        {
            transform.position=new Vector3(60,transform.position.y,transform.position.z);
        }
        else if(transform.position.x==60)
        {
            transform.position=new Vector3(40,transform.position.y,transform.position.z);
        }
        else if(transform.position.x==40)
        {
            transform.position=new Vector3(20,transform.position.y,transform.position.z);
        }
        else if(transform.position.x==20)
        {
            transform.position=new Vector3(0,transform.position.y,transform.position.z);
        }
    }
    public void GoRight()
    {
        if(transform.position.x==0)
        {
            transform.position=new Vector3(20,transform.position.y,transform.position.z);
        }
        else if (transform.position.x==20)
        {
            transform.position=new Vector3(40,transform.position.y,transform.position.z);
        }
        else if (transform.position.x==40)
        {
            transform.position=new Vector3(60,transform.position.y,transform.position.z);
        }
        else if (transform.position.x==60)
        {
            transform.position=new Vector3(80,transform.position.y,transform.position.z);
        }
    }
    void FixedUpdate()
    {
        if(gameObject.transform.position.x==0)
        {
            GameObject.FindWithTag("BB").transform.localScale = Vector3.one;
            GameObject.FindWithTag("CA").transform.localScale = Vector3.zero;
            GameObject.FindWithTag("CL").transform.localScale = Vector3.zero;
            GameObject.FindWithTag("FF").transform.localScale = Vector3.zero;
            GameObject.FindWithTag("TT").transform.localScale = Vector3.zero;
        }
        else if(gameObject.transform.position.x==20)
        {
            GameObject.FindWithTag("BB").transform.localScale = Vector3.zero;
            GameObject.FindWithTag("CA").transform.localScale = Vector3.one;
            GameObject.FindWithTag("CL").transform.localScale = Vector3.zero;
            GameObject.FindWithTag("FF").transform.localScale = Vector3.zero;
            GameObject.FindWithTag("TT").transform.localScale = Vector3.zero;
        }
        else if(gameObject.transform.position.x==40)
        {
            GameObject.FindWithTag("BB").transform.localScale = Vector3.zero;
            GameObject.FindWithTag("CA").transform.localScale = Vector3.zero;
            GameObject.FindWithTag("CL").transform.localScale = Vector3.one;
            GameObject.FindWithTag("FF").transform.localScale = Vector3.zero;
            GameObject.FindWithTag("TT").transform.localScale = Vector3.zero;
        }
        else if(gameObject.transform.position.x==60)
        {
            GameObject.FindWithTag("BB").transform.localScale = Vector3.zero;
            GameObject.FindWithTag("CA").transform.localScale = Vector3.zero;
            GameObject.FindWithTag("CL").transform.localScale = Vector3.zero;
            GameObject.FindWithTag("FF").transform.localScale = Vector3.one;
            GameObject.FindWithTag("TT").transform.localScale = Vector3.zero;
        }
        else if(gameObject.transform.position.x==80)
        {
            GameObject.FindWithTag("BB").transform.localScale = Vector3.zero;
            GameObject.FindWithTag("CA").transform.localScale = Vector3.zero;
            GameObject.FindWithTag("CL").transform.localScale = Vector3.zero;
            GameObject.FindWithTag("FF").transform.localScale = Vector3.zero;
            GameObject.FindWithTag("TT").transform.localScale = Vector3.one;
        }
    }
}
