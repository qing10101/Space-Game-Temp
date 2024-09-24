using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ShipViewer : MonoBehaviour
{
    public static int[] shipfactions;
    public static string[] shipNames;
    Camera myCamera;
    GameObject shipNameTag;
    int index;
    void Awake()
    {
        index = 0;
        shipNameTag = GameObject.FindWithTag("ShipName");
        shipNames = new string[18];
        shipfactions = new int[18]{3,2,0,1,3,1,0,2,1,1,0,2,1,3,1,1,0,2};
        myCamera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        index = (int)(transform.position.x-30)/400;
        if(Input.GetAxis("Mouse ScrollWheel")>0&&myCamera.fieldOfView<90||Input.GetAxis("Mouse ScrollWheel")<0&&myCamera.fieldOfView>30)
        {
            myCamera.fieldOfView += Input.GetAxis("Mouse ScrollWheel");
        }
        shipNameTag.GetComponent<TextMeshProUGUI>().text = shipNames[index];
        Color color= Color.red;
        switch(shipfactions[index])
        {
            case 1:
                color = Color.yellow;
                break;
            case 2:
                color = Color.magenta;
                break;
            case 3:
                color = Color.white;
                break;
            default:
                break;

        }
        shipNameTag.GetComponent<TextMeshProUGUI>().color = color;
    }
    public void GoLeft()
    {
        if(transform.position.x>30)
        {
            transform.position += new Vector3(-400f,0f,0f);
        }
    }
    public void GoRight()
    {
        if(transform.position.x<6830)
        {
            transform.position += new Vector3(400f,0f,0f);
        }
    }
    public void Done()
    {
        SceneManager.LoadScene("Map",LoadSceneMode.Single);
    }
}
