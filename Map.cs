using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void DeclineArmistice()
    {
        if(GameManager.IncidentPlanet!=null)
        {
            GameManager.IncidentPlanet.PreceedAttack();
            GameManager.IncidentPlanet = null;
        }
    }
    public void AcceptArmistice()
    {
        if(GameManager.IncidentPlanet!=null)
        {
            GameManager.IncidentPlanet.AcceptArmistice();
            GameManager.IncidentPlanet = null;
        }
    }
    public void DontPayRansom()
    {
        if(GameManager.IncidentPlanet!=null)
        {
            GameManager.IncidentPlanet.CounterAttackPreceed();
            GameManager.IncidentPlanet = null;
        }
    }
    public void PayRansom()
    {
        if(GameManager.IncidentPlanet!=null)
        {
            GameManager.IncidentPlanet.PayRansom();
            GameManager.IncidentPlanet = null;
        }
    }
    public void EndGameQuit()
    {
        GameManager.manager.Reset();
        Application.Quit();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Test()
    {
        Debug.Log("Testing");
    }
}
