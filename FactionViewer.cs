using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class FactionViewer : MonoBehaviour
{
    float moveConstant;
    // Start is called before the first frame update
    void Start()
    {
        moveConstant = 2.16f;
    }

    // // Update is called once per frame
    // void Update()
    // {
        
    // }
    public void Done()
    {
        SceneManager.LoadScene("Map",LoadSceneMode.Single);
    }
    public void MoveLeft()
    {
        if(transform.position.x>0.1f)
        {
            transform.position -= new Vector3(moveConstant, 0f, 0f);
        }
    }
    public void MoveRight()
    {
        if(transform.position.x<2.16f*3f)
        {
            transform.position += new Vector3(moveConstant, 0f, 0f);
        }
    }
}
