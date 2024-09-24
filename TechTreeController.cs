using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TechTreeController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void EnterConquerMode()
    {
        PlayerPrefs.SetInt("Money",GameManager.money);
        var thread = new Thread(GameManager.CleaningThread)
        {
            IsBackground = true,
            Priority = System.Threading.ThreadPriority.BelowNormal
        };
        thread.Start();
        SceneManager.LoadScene("Map",LoadSceneMode.Single);
    }
}
