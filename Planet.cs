using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;

public class Planet : MonoBehaviour
{
    public int faction;
    public static Planet[,] planetsGrid;
    public static Dictionary<string,Dictionary<string,int>> mobolizedFleet;
    public bool isFriendly;
    public bool isShipyard;
    public Dictionary<string,int> fleet;
    public bool isFleetSupplied;
    public TextMeshProUGUI currentFleetText,nameTag;
    public int xPos;
    public int yPos;
    public int dispatchFrom,buildShipType;
    public GameObject notification,armistice;
    static int armisticeAmount;
    static int BB,CA,CL,FF,TT,EBB,EC,EF;
    public Sprite earth, jupiter, mars, mercury, venus;
    int colorIndex;
    void Awake()
    {
    }
    public void BuildShipyard()
    {
        Assert.IsTrue(isFriendly);
        if(GameManager.money>8000)
        {
            if(fleet["Tanker"]>0)
            {
                GameManager.money-=8000;
                CreateShipyard();
            }
            else
            {
                GameObject note = Instantiate(notification,GameObject.FindWithTag("Canvas").transform);
                note.GetComponent<TextMeshProUGUI>().text = "Please dispatch a tanker to this planet to build the shipyard";
                Destroy(note,2.5f);
            }
        }
        else
        {
            GameObject note = Instantiate(notification,GameObject.FindWithTag("Canvas").transform);
            note.GetComponent<TextMeshProUGUI>().text = "Insufficient money to build shipyard";
            Destroy(note,2.5f);
        }
        
    }
    void ShowRansom()
    {
        GameObject.FindWithTag("AttackedPlanet").GetComponent<TextMeshProUGUI>().text = nameTag.text+" is under attack!";
        GameObject.FindWithTag("Ransom").transform.localScale = Vector3.one;
        GameObject.FindWithTag("RansomMoney").GetComponent<TextMeshProUGUI>().text = ((int)(GameManager.money*UnityEngine.Random.Range(0.75f,1.25f))).ToString();
    }
    public void PayRansom()
    {
        int money = int.Parse(GameObject.FindWithTag("RansomMoney").GetComponent<TextMeshProUGUI>().text);
        if(money>GameManager.money)
        {
            GameObject note = Instantiate(notification,GameObject.FindWithTag("Canvas").transform);
            note.GetComponent<TextMeshProUGUI>().text = "Insufficient money to pay ransom";
            Destroy(note,2f);
            GameManager.IncidentPlanet = null;
            Invoke(nameof(CounterAttackPreceed),2.5f);
        }
        else
        {
            GameManager.IncidentPlanet = null;
            GameObject.FindWithTag("Ransom").transform.localScale = Vector3.zero;
            GameManager.incidentFaction = -1;
            GameManager.money-=money;
            Save();
        }
        
    }

    public void CounterAttackPreceed()
    {
        GameManager.victory = false;
        BB=fleet["Battleship"];
        CA=fleet["Heavy Cruiser"];
        CL=fleet["Light Cruiser"];
        FF=fleet["Frigate"];
        TT=fleet["Tanker"];
        
        GameManager.conqueredX=xPos;
        GameManager.conqueredY=yPos;
        int flagshipCode=0; 
        int r = UnityEngine.Random.Range(0,5); 
        EBB=fleet["Battleship"];
        EC=fleet["Light Cruiser"]+UnityEngine.Random.Range(0,3);
        EF=fleet["Frigate"]+UnityEngine.Random.Range(2,5);
        if(BB+CA+CL+TT+FF<=0)
        {
            GameObject.FindWithTag("Ransom").transform.localScale = Vector3.zero;
            FallToEnemy(false);
            return;
        }
        switch (r)
        {
            case 0:
                if(FF!=0)
                {
                    flagshipCode=0;
                    GameManager.manager.StartCoroutine(GameManager.manager.SceneLoaded(EBB,EC,EF,BB,CA,CL,FF-1,TT,flagshipCode,GameManager.incidentFaction-1, colorIndex));
                    break;
                }
                else
                    goto default;
            case 1:
                if(CL!=0)
                {
                    flagshipCode=1;
                    GameManager.manager.StartCoroutine(GameManager.manager.SceneLoaded(EBB,EC,EF,BB,CA,CL-1,FF,TT,flagshipCode,GameManager.incidentFaction-1, colorIndex));
                    break;
                }
                else
                    goto default;
            case 2:
                if(CA!=0)
                {
                    flagshipCode=2;
                    GameManager.manager.StartCoroutine(GameManager.manager.SceneLoaded(EBB,EC,EF,BB,CA-1,CL,FF,TT,flagshipCode,GameManager.incidentFaction-1, colorIndex));
                    break;
                }
                else
                    goto default;
            case 3:
                if(BB!=0)
                {
                    flagshipCode=3;
                    GameManager.manager.StartCoroutine(GameManager.manager.SceneLoaded(EBB,EC,EF,BB-1,CA,CL,FF,TT,flagshipCode,GameManager.incidentFaction-1, colorIndex));
                    break;
                }
                else
                    goto default;
            case 4:
                if(TT!=0)
                {
                    flagshipCode=4;
                    GameManager.manager.StartCoroutine(GameManager.manager.SceneLoaded(EBB,EC,EF,BB,CA,CL,FF,TT-1,flagshipCode,GameManager.incidentFaction-1, colorIndex));
                    break;
                }
                else
                    goto default;
            default:
                if(BB!=0)
                {
                    flagshipCode=3;
                    GameManager.manager.StartCoroutine(GameManager.manager.SceneLoaded(EBB,EC,EF,BB-1,CA,CL,FF,TT,flagshipCode,GameManager.incidentFaction-1, colorIndex));
                }
                else if(CA!=0)
                {
                    flagshipCode=2;
                    GameManager.manager.StartCoroutine(GameManager.manager.SceneLoaded(EBB,EC,EF,BB,CA-1,CL,FF,TT,flagshipCode,GameManager.incidentFaction-1, colorIndex));
                }
                else if(CL!=0)
                {
                    flagshipCode=1;
                    GameManager.manager.StartCoroutine(GameManager.manager.SceneLoaded(EBB,EC,EF,BB,CA,CL-1,FF,TT,flagshipCode,GameManager.incidentFaction-1, colorIndex));
                }
                else if(TT!=0)
                {
                    flagshipCode=4;
                    GameManager.manager.StartCoroutine(GameManager.manager.SceneLoaded(EBB,EC,EF,BB,CA,CL,FF,TT-1,flagshipCode,GameManager.incidentFaction-1, colorIndex));
                }
                else
                {
                    GameManager.manager.StartCoroutine(GameManager.manager.SceneLoaded(EBB,EC,EF,BB,CA,CL,FF-1,TT,flagshipCode,GameManager.incidentFaction-1, colorIndex));
                }
                break;
        }      
        GameManager.isMissionMode = false;
        SceneManager.LoadScene("SampleScene",LoadSceneMode.Single);
    }
    IEnumerator CAttackDelayed(int faction)
    {
        yield return new WaitForSeconds(0.25f);
        GameManager.IncidentPlanet = this;
        GameManager.incidentFaction = faction;
        switch (faction)
        {
            case 1:
                GameManager.rnAnger=0;
                break;
            case 2:
                GameManager.arasakaAnger=0;
                break;
            case 3:
                GameManager.googleAnger=0;
                break;
            case 4:
                GameManager.cbisAnger=0;
                break;
            default:
                break;
        }
        PlayerPrefs.SetInt("RNAnger",GameManager.rnAnger);
        PlayerPrefs.SetInt("ArasakaAnger",GameManager.arasakaAnger);
        PlayerPrefs.SetInt("GoogleAnger",GameManager.googleAnger);
        PlayerPrefs.SetInt("CBISAnger",GameManager.cbisAnger);
        ShowRansom();
    }
    public void CounterAttack(int faction)
    {
        StartCoroutine(CAttackDelayed(faction));
    }
    IEnumerator FMWFDelayed(int faction)
    {
        yield return new WaitForSeconds(0.25f);
        foreach(var planet in planetsGrid)
        {
            if (planet!=null&&planet.faction == 0)
            {
                if((planet.xPos>0&&planetsGrid[planet.xPos-1,planet.yPos].faction==faction)
                ||(planet.xPos<4&&planetsGrid[planet.xPos+1,planet.yPos].faction==faction)
                ||(planet.yPos>0&&planetsGrid[planet.xPos,planet.yPos-1].faction==faction)
                ||(planet.yPos<4&&planetsGrid[planet.xPos,planet.yPos+1].faction==faction)
                )
                {
                    Debug.Log("JDAJDJA");
                    planet.CounterAttack(faction);
                    break;
                }
            }
        }
    }
    void FindMyNeighbourWithFaction(int faction)
    {
        Debug.Log("PPPGGG");
        StartCoroutine(FMWFDelayed(faction));
    }
    // when the planet is newly conquered
    public void Conquered()
    {
        GameManager.conqueredX = GameManager.conqueredY=-1;
        isFriendly = true;
        isShipyard = false;
        faction=0;
        fleet["Battleship"]=BB-Test.BBYLost;
        fleet["Heavy Cruiser"]=CA-Test.CAYLost;
        fleet["Light Cruiser"]=CL-Test.CLYLost;
        fleet["Frigate"]=FF-Test.FFYLost;
        fleet["Tanker"]=TT-Test.TTYLost;
        InvokeRepeating(nameof(IncrementMoney),5f,10f);
        buildShipType = dispatchFrom = 0;
        nameTag.color = Color.blue;
        isFleetSupplied = false;
        if(currentFleetText!=null)
        foreach (var item in currentFleetText.GetComponentsInChildren<TextMeshProUGUI>())
        {
            if(item.gameObject.name.IndexOf("Current")==-1)
            {
                item.text = "Is Fleet Supplied? False";
                break;
            }
        }
        GameManager.money+=UnityEngine.Random.Range(2000,3000)*GetActionFleetSize();
        Save();
        Debug.Log(GameManager.attackedPlanetFaction+"accc");
        switch (GameManager.attackedPlanetFaction)
        {
            case 1:
                GameManager.rnAnger+=100*UnityEngine.Random.Range(1, 10);
                if(GameManager.rnAnger>600)
                {
                    GameManager.rnAnger=600;
                    FindMyNeighbourWithFaction(GameManager.attackedPlanetFaction);
                    return;
                }
                break;
            case 2:
                GameManager.arasakaAnger+=100*UnityEngine.Random.Range(1, 10);
                if(GameManager.arasakaAnger>600)
                {
                    GameManager.arasakaAnger=600;
                    FindMyNeighbourWithFaction(GameManager.attackedPlanetFaction);
                    return;
                }
                break;
            case 3:
                GameManager.googleAnger+=100*UnityEngine.Random.Range(1, 10);
                if(GameManager.googleAnger>600)
                {
                    GameManager.googleAnger=600;
                    FindMyNeighbourWithFaction(GameManager.attackedPlanetFaction);
                    return;
                }
                break;
            case 4:
                GameManager.cbisAnger+=100*UnityEngine.Random.Range(1, 10);
                if(GameManager.cbisAnger>600)
                {
                    GameManager.cbisAnger=600;
                    FindMyNeighbourWithFaction(GameManager.attackedPlanetFaction);
                    return;
                }
                break;
            default:
                break;
        }
        GameManager.attackedPlanetFaction = -1;
        PlayerPrefs.SetInt("RNAnger",GameManager.rnAnger);
        PlayerPrefs.SetInt("ArasakaAnger",GameManager.arasakaAnger);
        PlayerPrefs.SetInt("GoogleAnger",GameManager.googleAnger);
        PlayerPrefs.SetInt("CBISAnger",GameManager.cbisAnger);
        bool allFriendly = true;
        foreach (var item in planetsGrid)
        {
            if(!item.isFriendly)
            {
                allFriendly = false;
                break;
            }
        }
        if(allFriendly)
        {
            GameObject.FindWithTag("Victory").transform.localScale = Vector3.one;
        }
    }
    public void ChooseDispatchPlanet(Int32 i)
    {
        dispatchFrom = i;
    }

    public void Dispatch()
    {
        int BB=0,CA=0,CL=0,FF=0,TT=0;
        foreach (var item in GetComponentsInChildren<TMP_InputField>())
        {
            if(item.text.IndexOf("-") != -1)
            {
                GameObject note = Instantiate(notification,GameObject.FindWithTag("Canvas").transform);
                note.GetComponent<TextMeshProUGUI>().text = "Please enter valid numbers";
                Destroy(note,2.5f);
                return;
            }
            if(item.gameObject.name.Equals("BB"))
            {
                BB=int.Parse(item.text);
                item.text="0";
            }
            else if(item.gameObject.name.Equals("CA"))
            {
                CA=int.Parse(item.text);
                item.text = "0";
            }
            else if(item.gameObject.name.Equals("CL"))
            {
                CL=int.Parse(item.text);
                item.text = "0";
            }
            else if(item.gameObject.name.Equals("FF"))
            {
                FF=int.Parse(item.text);
                item.text = "0";
            }
            else if(item.gameObject.name.Equals("TT"))
            {
                TT=int.Parse(item.text);
                item.text = "0";
            }
        }
        // if(BB<0||CA<0||CL<0||FF<0||TT<0)
        // {
        //     GameObject note = Instantiate(notification,GameObject.FindWithTag("Canvas").transform);
        //     note.GetComponent<TextMeshProUGUI>().text = "Please enter non-negative numbers";
        //     Destroy(note,2.5f);
        // }
        
        Dictionary<string, int> dispatchedFleet = new Dictionary<string, int>
        {
            { "Battleship", BB },
            { "Heavy Cruiser", CA },
            { "Light Cruiser", CL },
            { "Frigate", FF },
            { "Tanker", TT }
        };
        
        if(GameManager.money<(BB+CA+CL+FF+TT)*50)
        {
            GameObject note = Instantiate(notification,GameObject.FindWithTag("Canvas").transform);
            note.GetComponent<TextMeshProUGUI>().text = "Insufficient Money for ship dispatching";
            Destroy(note,2.5f);
            return;
        }
        
        Planet originPlanet= null;
        switch(dispatchFrom)
        {
            case 0: 
                if(yPos<4&&planetsGrid[xPos,yPos+1]!=null&&planetsGrid[xPos,yPos+1].isFriendly)
                {
                    originPlanet = planetsGrid[xPos, yPos+1];
                }
                break;
            case 1: 
                if(yPos>0&&planetsGrid[xPos,yPos-1]!=null&&planetsGrid[xPos,yPos-1].isFriendly)
                {
                    originPlanet = planetsGrid[xPos, yPos-1];
                }
                break;
            case 2: 
                if(xPos>0&&planetsGrid[xPos-1,yPos]!=null&&planetsGrid[xPos-1,yPos].isFriendly)
                {
                    originPlanet = planetsGrid[xPos-1, yPos];
                }
                break;
            case 3: 
                if(xPos<4&&planetsGrid[xPos+1,yPos]!=null&&planetsGrid[xPos+1,yPos].isFriendly)
                {
                    originPlanet = planetsGrid[xPos+1, yPos];
                }
                break;
            default: break;
        }
        if(originPlanet!=null)
        {
            int fee=0;
            // DispatchHelper(originalPlanet,dispatchedFleet);
            if(dispatchedFleet["Battleship"] >0 && originPlanet.fleet["Battleship"]>=dispatchedFleet["Battleship"])
            {
                originPlanet.fleet["Battleship"]-=dispatchedFleet["Battleship"];
                fee+=dispatchedFleet["Battleship"]*50;
                fleet["Battleship"]+=dispatchedFleet["Battleship"];
            }
            if(dispatchedFleet["Heavy Cruiser"] >0&& originPlanet.fleet["Heavy Cruiser"]>=dispatchedFleet["Heavy Cruiser"])
            {
                originPlanet.fleet["Heavy Cruiser"]-=dispatchedFleet["Heavy Cruiser"];
                fee+=dispatchedFleet["Heavy Cruiser"]*50;
                fleet["Heavy Cruiser"]+=dispatchedFleet["Heavy Cruiser"];
            }
            if(dispatchedFleet["Light Cruiser"] >0&& originPlanet.fleet["Light Cruiser"]>=dispatchedFleet["Light Cruiser"])
            {
                originPlanet.fleet["Light Cruiser"]-=dispatchedFleet["Light Cruiser"];
                fee+=dispatchedFleet["Light Cruiser"]*50;
                fleet["Light Cruiser"]+=dispatchedFleet["Light Cruiser"];
            }
            if(dispatchedFleet["Frigate"] >0&& originPlanet.fleet["Frigate"]>=dispatchedFleet["Frigate"])
            {
                originPlanet.fleet["Frigate"]-=dispatchedFleet["Frigate"];
                fee+=dispatchedFleet["Frigate"]*50;
                fleet["Frigate"]+=dispatchedFleet["Frigate"];
            }
            if(dispatchedFleet["Tanker"] >0&& originPlanet.fleet["Tanker"]>=dispatchedFleet["Tanker"])
            {
                originPlanet.fleet["Tanker"]-=dispatchedFleet["Tanker"];
                fee+=dispatchedFleet["Tanker"]*50;
                fleet["Tanker"]+=dispatchedFleet["Tanker"];
            }
            if(fee==0)
            {
                return;
            }
            GameManager.money-=fee;
            currentFleetText.text="Fleet stationed here:\n"+fleet["Battleship"]+" Battleship\n"+fleet["Heavy Cruiser"]+" Heavy Crsuiers\n"+fleet["Light Cruiser"]+" Light Cuiser\n"+fleet["Frigate"]+" Frigate\n"+fleet["Tanker"]+" Tanker";
            isFleetSupplied = false;
            foreach (var item in currentFleetText.GetComponentsInChildren<TextMeshProUGUI>())
            {
                if(item.gameObject.name.IndexOf("Current")==-1)
                {
                    item.text = "Is Fleet Supplied? False";
                    break;
                }
            }
            Save();
            originPlanet.Save();
        }
    }


    int GetActionFleetSize()
    {
        Assert.IsTrue(isFriendly);
        return fleet["Battleship"]+fleet["Heavy Cruiser"]+fleet["Light Cruiser"]+fleet["Frigate"]+fleet["Tanker"];
    }
    public void ShowArmistice(int myFleetSize)
    {
        armistice.transform.localScale = Vector3.one;
        armisticeAmount = UnityEngine.Random.Range(myFleetSize*3000,myFleetSize*10000);
        armistice.GetComponentInChildren<TextMeshProUGUI>().text = "Enemy offered you an armistice by paying you "+armisticeAmount+" money";
    }
    public void AcceptArmistice()
    {
        GameManager.money+=armisticeAmount;
        armistice.transform.localScale = Vector3.zero;
        fleet["Battleship"]=(int)(fleet["Battleship"]*1.5f);
        fleet["Cruiser"]=(int)(fleet["Cruiser"]*2.1f);
        fleet["Frigate"]=(int)(fleet["Frigate"]*2.2f);
        GameManager.IncidentPlanet=null;
        foreach (var o in GetComponentsInChildren<TextMeshProUGUI>())
        {
            if(o.name.IndexOf("Enemy")!=-1)
            {
                o.text="Enemy Fleet:\n"+fleet["Battleship"]+"*Battleship\n"+fleet["Cruiser"]+"*Cruiser\n"+fleet["Frigate"]+"*Frigate";
                break;
            }
        }
        Save();
    }
    public void PreceedAttack()
    {
        if(xPos>0&&planetsGrid[xPos-1,yPos]!=null&&planetsGrid[xPos-1,yPos].isFriendly&&planetsGrid[xPos-1,yPos].isFleetSupplied)
        {
            planetsGrid[xPos-1,yPos].isFleetSupplied = false;
            planetsGrid[xPos-1,yPos].fleet["Battleship"]=0;
            planetsGrid[xPos-1,yPos].fleet["Heavy Cruiser"]=0;
            planetsGrid[xPos-1,yPos].fleet["Light Cruiser"]=0;
            planetsGrid[xPos-1,yPos].fleet["Frigate"]=0;
            planetsGrid[xPos-1,yPos].fleet["Tanker"]=0;
            planetsGrid[xPos-1,yPos].Save();
        }
        if(xPos<4&&planetsGrid[xPos+1,yPos]!=null&&planetsGrid[xPos+1,yPos].isFriendly&&planetsGrid[xPos+1,yPos].isFleetSupplied)
        {
            planetsGrid[xPos+1,yPos].isFleetSupplied = false;
            planetsGrid[xPos+1,yPos].fleet["Battleship"]=0;
            planetsGrid[xPos+1,yPos].fleet["Heavy Cruiser"]=0;
            planetsGrid[xPos+1,yPos].fleet["Light Cruiser"]=0;
            planetsGrid[xPos+1,yPos].fleet["Frigate"]=0;
            planetsGrid[xPos+1,yPos].fleet["Tanker"]=0;
            planetsGrid[xPos+1,yPos].Save();
        }
        if(yPos<4&&planetsGrid[xPos,yPos+1]!=null&&planetsGrid[xPos,yPos+1].isFriendly&&planetsGrid[xPos,yPos+1].isFleetSupplied)
        {
            planetsGrid[xPos,yPos+1].isFleetSupplied = false;
            planetsGrid[xPos,yPos+1].fleet["Battleship"]=0;
            planetsGrid[xPos,yPos+1].fleet["Heavy Cruiser"]=0;
            planetsGrid[xPos,yPos+1].fleet["Light Cruiser"]=0;
            planetsGrid[xPos,yPos+1].fleet["Frigate"]=0;
            planetsGrid[xPos,yPos+1].fleet["Tanker"]=0;
            planetsGrid[xPos,yPos+1].Save();
        }
        if(yPos>0&&planetsGrid[xPos,yPos-1]!=null&&planetsGrid[xPos,yPos-1].isFriendly&&planetsGrid[xPos,yPos-1].isFleetSupplied)
        {
            planetsGrid[xPos,yPos-1].isFleetSupplied = false;
            planetsGrid[xPos,yPos-1].fleet["Battleship"]=0;
            planetsGrid[xPos,yPos-1].fleet["Heavy Cruiser"]=0;
            planetsGrid[xPos,yPos-1].fleet["Light Cruiser"]=0;
            planetsGrid[xPos,yPos-1].fleet["Frigate"]=0;
            planetsGrid[xPos,yPos-1].fleet["Tanker"]=0;
            planetsGrid[xPos,yPos-1].Save();
        }
        GameManager.conqueredX=xPos;
        GameManager.conqueredY=yPos;
        GameManager.victory = false;
        GameManager.incidentFaction = -1;
        GameManager.attackedPlanetFaction = faction;
        int flagshipCode=0; 
        int r = UnityEngine.Random.Range(0,5); 
        switch (r)
        {
            case 0:
                if(FF!=0)
                {
                    flagshipCode=0;
                    GameManager.manager.StartCoroutine(GameManager.manager.SceneLoaded(fleet["Battleship"],fleet["Cruiser"],fleet["Frigate"],BB,CA,CL,FF-1,TT,flagshipCode,faction-1, colorIndex));
                    break;
                }
                else
                    goto default;
            case 1:
                if(CL!=0)
                {
                    flagshipCode=1;
                    GameManager.manager.StartCoroutine(GameManager.manager.SceneLoaded(fleet["Battleship"],fleet["Cruiser"],fleet["Frigate"],BB,CA,CL-1,FF,TT,flagshipCode,faction-1, colorIndex));
                    break;
                }
                else
                    goto default;
            case 2:
                if(CA!=0)
                {
                    flagshipCode=2;
                    GameManager.manager.StartCoroutine(GameManager.manager.SceneLoaded(fleet["Battleship"],fleet["Cruiser"],fleet["Frigate"],BB,CA-1,CL,FF,TT,flagshipCode,faction-1, colorIndex));
                    break;
                }
                else
                    goto default;
            case 3:
                if(BB!=0)
                {
                    flagshipCode=3;
                    GameManager.manager.StartCoroutine(GameManager.manager.SceneLoaded(fleet["Battleship"],fleet["Cruiser"],fleet["Frigate"],BB-1,CA,CL,FF,TT,flagshipCode,faction-1, colorIndex));
                    break;
                }
                else
                    goto default;
            case 4:
                if(TT!=0)
                {
                    flagshipCode=4;
                    GameManager.manager.StartCoroutine(GameManager.manager.SceneLoaded(fleet["Battleship"],fleet["Cruiser"],fleet["Frigate"],BB,CA,CL,FF,TT-1,flagshipCode,faction-1, colorIndex));
                    break;
                }
                else
                    goto default;
            default:
                if(BB!=0)
                {
                    flagshipCode=3;
                    GameManager.manager.StartCoroutine(GameManager.manager.SceneLoaded(fleet["Battleship"],fleet["Cruiser"],fleet["Frigate"],BB-1,CA,CL,FF,TT,flagshipCode,faction-1, colorIndex));
                }
                else if(CA!=0)
                {
                    flagshipCode=2;
                    GameManager.manager.StartCoroutine(GameManager.manager.SceneLoaded(fleet["Battleship"],fleet["Cruiser"],fleet["Frigate"],BB,CA-1,CL,FF,TT,flagshipCode,faction-1, colorIndex));
                }
                else if(CL!=0)
                {
                    flagshipCode=1;
                    GameManager.manager.StartCoroutine(GameManager.manager.SceneLoaded(fleet["Battleship"],fleet["Cruiser"],fleet["Frigate"],BB,CA,CL-1,FF,TT,flagshipCode,faction-1, colorIndex));
                }
                else if(TT!=0)
                {
                    flagshipCode=4;
                    GameManager.manager.StartCoroutine(GameManager.manager.SceneLoaded(fleet["Battleship"],fleet["Cruiser"],fleet["Frigate"],BB,CA,CL-1,FF,TT-1,flagshipCode,faction-1, colorIndex));
                }
                else
                {
                    GameManager.manager.StartCoroutine(GameManager.manager.SceneLoaded(fleet["Battleship"],fleet["Cruiser"],fleet["Frigate"],BB,CA,CL,FF-1,TT,flagshipCode,faction-1, colorIndex));
                }
                break;
        }      
        GameManager.isMissionMode = false;
        SceneManager.LoadScene("SampleScene",LoadSceneMode.Single);
    }
    public void Attack()
    {
        GameManager.IncidentPlanet=this;
        BB=CA=CL=FF=TT=0;
        foreach (var i in mobolizedFleet.Values)
        {
            BB+=i["Battleship"];
            CA+=i["Heavy Cruiser"];
            CL+=i["Light Cruiser"];
            FF+=i["Frigate"];
            TT+=i["Tanker"];
        } 
        if(BB+CA+CL+FF+TT<=0)
        {
            GameObject note = Instantiate(notification,GameObject.FindWithTag("Canvas").transform);
            note.GetComponent<TextMeshProUGUI>().text = "No ship can be sent for action";
            Destroy(note,2.5f);
            GameManager.IncidentPlanet=null;
            return;
        }
        if(TT==0)
        {
            GameObject note = Instantiate(notification,GameObject.FindWithTag("Canvas").transform);
            note.GetComponent<TextMeshProUGUI>().text = "A tanker is required for each expedition fleet";
            Destroy(note,2.5f);
            GameManager.IncidentPlanet=null;
            return;
        }
        if((fleet["Battleship"]+fleet["Cruiser"]+fleet["Frigate"])*1.5f<(BB+CA+CL+FF+TT)&&(BB+CA+CL+FF+TT)<12)
        {
            ShowArmistice(BB+CA+CL+FF+TT);
        }
        else
        {
            PreceedAttack();
        }
        
    }
    public void Supply()
    {
        Assert.IsTrue(isFriendly);
        if(isShipyard&&GetActionFleetSize()>0)
        {
            isFleetSupplied = true;
            if(GameManager.money<GetActionFleetSize()*10)
            {
                GameObject note = Instantiate(notification,GameObject.FindWithTag("Canvas").transform);
                note.GetComponent<TextMeshProUGUI>().text = "Insufficient Money for supplying ships";
                Destroy(note,2.5f);
                return;
            }
            GameManager.money-=GetActionFleetSize()*10;
            foreach (var item in currentFleetText.GetComponentsInChildren<TextMeshProUGUI>())
            {
                if(item.gameObject.name.IndexOf("Current")==-1)
                {
                    item.text = "Is Fleet Supplied? True";
                    break;
                }
            }
            Save();
            // currentFleetText.GetComponentInChildren<TextMeshProUGUI>().text = "Is Fleet Supplied? True";
        }
    }
    public void ChangeBuildShipType(Int32 i)
    {
        buildShipType = i;
    }
    public void BuildShip()
    {
        switch(buildShipType)
        {
            case 0:
                if(GameManager.money>100000)
                {
                    fleet["Battleship"]+=1;
                    GameManager.money-=100000;
                }
                else
                {
                    GameObject note = Instantiate(notification,GameObject.FindWithTag("Canvas").transform);
                    note.GetComponent<TextMeshProUGUI>().text = "Insufficient Money for ship buildigng";
                    Destroy(note,2.5f);
                    return;
                }
                break;
            case 1:
                if(GameManager.money>40000)
                {
                    fleet["Heavy Cruiser"]+=1;
                    GameManager.money-=40000;
                }
                else
                {
                    GameObject note = Instantiate(notification,GameObject.FindWithTag("Canvas").transform);
                    note.GetComponent<TextMeshProUGUI>().text = "Insufficient Money for ship buildigng";
                    Destroy(note,2.5f);
                    return;
                }
                break;
            case 2:
                if(GameManager.money>15000)
                {
                    fleet["Light Cruiser"]+=1;
                    GameManager.money-=15000;
                }
                else
                {
                    GameObject note = Instantiate(notification,GameObject.FindWithTag("Canvas").transform);
                    note.GetComponent<TextMeshProUGUI>().text = "Insufficient Money for ship buildigng";
                    Destroy(note,2.5f);
                    return;
                }
                break;
            case 3:
                if(GameManager.money>3000)
                {
                    fleet["Frigate"]+=1;
                    GameManager.money-=3000;
                }
                else
                {
                    GameObject note = Instantiate(notification,GameObject.FindWithTag("Canvas").transform);
                    note.GetComponent<TextMeshProUGUI>().text = "Insufficient Money for ship buildigng";
                    Destroy(note,2.5f);
                    return;
                }
                break;
            case 4:
                if(GameManager.money>18000)
                {
                    fleet["Tanker"]+=1;
                    GameManager.money-=18000;
                }
                else
                {
                    GameObject note = Instantiate(notification,GameObject.FindWithTag("Canvas").transform);
                    note.GetComponent<TextMeshProUGUI>().text = "Insufficient Money for ship buildigng";
                    Destroy(note,2.5f);
                    return;
                }
                break;
            default: break;
        }
        isFleetSupplied = false;
        foreach (var item in currentFleetText.GetComponentsInChildren<TextMeshProUGUI>())
        {
            if(item.gameObject.name.IndexOf("Current")==-1)
            {
                item.text = "Is Fleet Supplied? False";
                break;
            }
        }
        Save();
        currentFleetText.text="Fleet stationed here:\n"+fleet["Battleship"]+" Battleship\n"+fleet["Heavy Cruiser"]+" Heavy Crsuiers\n"+fleet["Light Cruiser"]+" Light Cuiser\n"+fleet["Frigate"]+" Frigate\n"+fleet["Tanker"]+" Tanker";
    }
    void CreateShipyard()
    {
        isShipyard = true;
        int i = 0;
        foreach (var item in GetComponentsInChildren<TextMeshProUGUI>())
        {
            if(item.name.IndexOf("Build")!=-1||item.name.IndexOf("Supply")!=-1)
            {
                item.transform.localScale = new Vector3(0.1f,0.1f,0.1f);
                ++i;
            }
            if(i>1) break;
        }
        foreach (var item in GetComponentsInChildren<UnityEngine.UI.Button>())
        {
            if(item.name.IndexOf("Lock")!=-1)
            {
                item.transform.localScale = Vector3.zero;
                break;
            }
        }
        Save();
    }
    public void Save()
    {
        if(GameManager.isResetting) return;
        string key = "Planet"+xPos+","+yPos;
        string value;
        Debug.Log(key);
        if (faction!=0)
        {
            value = nameTag.text+"\n"+faction+"\n"+fleet["Battleship"]+","+fleet["Cruiser"]+","+fleet["Frigate"]+"\n";
        }
        else
        {
            int sup = isFleetSupplied ? 1 : 0;
            int yard = isShipyard? 1 : 0;
            value = nameTag.text+"\n"+faction+"\n"+fleet["Battleship"]+","+fleet["Heavy Cruiser"]+","+fleet["Light Cruiser"]+","+fleet["Frigate"]+","+fleet["Tanker"]+"\n"+sup+","+yard+"\n";
            Debug.Log(value);
        }
        PlayerPrefs.SetString(key,value);
        PlayerPrefs.SetInt("Money",GameManager.money);
    }
    void OnDestroy()
    {
        Save();
    }
    void OnApplicationQuit()
    {
        Save();
    }
    void IncrementMoney()
    {
        if(GameManager.isResetting) return;
        GameManager.money+=400+15*GetActionFleetSize();
        Save();
    }
    void FallToEnemy(bool isBattle)
    {
        isFriendly = false;
        faction = GameManager.incidentFaction;
        GameManager.incidentFaction = -1;
        GameManager.conqueredX = GameManager.conqueredY = -1;
        if(isBattle)
        {
            fleet["Battleship"] = EBB-Test.BBELost;
            fleet["Cruiser"] = EC-Test.CAELost-Test.CLELost;
            fleet["Frigate"] = EF-Test.FFELost;
        }
        else
        {
            fleet["Battleship"] = UnityEngine.Random.Range(0, 3);
            fleet["Cruiser"] = UnityEngine.Random.Range(1, 4);
            fleet["Frigate"] = UnityEngine.Random.Range(2, 5);
        }
        GameManager.money = (int)(GameManager.money*UnityEngine.Random.Range(0.55f, 0.7f));
        Save();

        GameObject t = Instantiate(GameManager.manager.enemyPlanet,GameObject.FindWithTag("Board").transform);
        t.transform.localPosition = new Vector3((xPos-2), (yPos-2), 0);
        t.transform.localRotation = Quaternion.identity;
        Destroy(gameObject);
    }
    void Start()
    {
        armistice = GameObject.FindWithTag("Armistice");
        // armistice.transform.localScale = Vector3.zero;
        colorIndex = UnityEngine.Random.Range(0,5);
        GameObject planetButton = null;
        foreach (var item in GetComponentsInChildren<Button>())
        {
            if(item.gameObject.name.Equals("PlanetButton"))
            {
                planetButton = item.gameObject;
                break;
            }
        }
        if(planetButton != null)
        {
            switch (colorIndex)
            {
                case 0:
                    planetButton.GetComponent<Image>().sprite = earth;
                    break;
                case 1:
                    planetButton.GetComponent<Image>().sprite = jupiter;
                    break;
                case 2:
                    planetButton.GetComponent<Image>().sprite = mars;
                    break;
                case 3:
                    planetButton.GetComponent<Image>().sprite = mercury;
                    break;
                case 4:
                    planetButton.GetComponent<Image>().sprite = venus;
                    break;
                default:
                    break;
            }
        }
        foreach (var item in GetComponentsInChildren<TextMeshProUGUI>())
        {
            if(item.name.IndexOf("CurrentFleet")!=-1)
            {
                currentFleetText = item;
                break;
            }
        }
        fleet = new Dictionary<string, int>();
        foreach (var item in GetComponentsInChildren<TextMeshProUGUI>())
        {
            if(item.name.IndexOf("Name")!=-1)
            {
                nameTag = item;
                break;
            }
        }
        xPos=(int)transform.localPosition.x+2;
        yPos=(int)transform.localPosition.y+2;
        planetsGrid[xPos,yPos] = this;
        
        // read previous saved data... to set fleet & etc...
        // planet information
        // no. 0 line: name
        // no. 1 line: controlled by: 0 for you, 1 to 4 for four factions
        // no. 2 line: fleet data:
        //friednly example of 1 BB, 2 CA, 1 CL, 2 FF, 1 tanker:1,2,1,2,1;
        //enemy example of 1 BB, 3 C, 3 FF:1,3,3
        // no. 3 line: supply & shipyard: 
        // example of supplied and being a shipyard: 1,1
        // example of supplied and not being a shipyard: 1,0
        //
        fleet.Add("Battleship",0);
        fleet.Add("Frigate",0);
        string[] planetInfo = PlayerPrefs.GetString("Planet"+xPos+","+yPos).Split('\n');
        // Assert.IsTrue(planetInfo[0]!=null);
        if(planetInfo.Length>0&&planetInfo[0]!=null)
        {
            nameTag.text = planetInfo[0];
        }
        string[] fleetInfo = null;
        if(planetInfo.Length>2&&planetInfo[2]!=null)
            fleetInfo = planetInfo[2].Split(',');
        fleet.Add("Cruiser",0);
        fleet.Add("Heavy Cruiser",0);
        fleet.Add("Light Cruiser",0);
        fleet.Add("Tanker",0);
        
        if(xPos==GameManager.conqueredX&&yPos==GameManager.conqueredY&&GameManager.victory)
        {
            Conquered();
            
        }
        else if(xPos==GameManager.conqueredX&&yPos==GameManager.conqueredY&&planetInfo.Length>1&&planetInfo[1]!=null&&int.Parse(planetInfo[1])==0) // been conquered by foe
        {
            FallToEnemy(true);                 
        }
        else if(planetInfo.Length>1&&planetInfo[1]!=null)
        {
            faction = int.Parse(planetInfo[1]);
            if(faction==0) // friendly
            {
                isFriendly=true;
                InvokeRepeating(nameof(IncrementMoney),5f,10f);
                buildShipType = dispatchFrom = 0;
                nameTag.color = Color.blue;
                fleet["Battleship"] = int.Parse(fleetInfo[0]);
                fleet["Heavy Cruiser"] = int.Parse(fleetInfo[1]);
                fleet["Light Cruiser"] = int.Parse(fleetInfo[2]);
                fleet["Frigate"] = int.Parse(fleetInfo[3]);
                fleet["Tanker"] = int.Parse(fleetInfo[4]);
                if(planetInfo.Length>3&&planetInfo[3]!=null)
                {
                    string[] supplyAndShipyard = planetInfo[3].Split(',');
                    if(supplyAndShipyard[1].IndexOf("1")!=-1)
                    {
                        CreateShipyard();
                    }
                    if(supplyAndShipyard[0].IndexOf("1")!=-1)
                    {
                        Debug.Log("SUPP");
                        GameManager.money+=GetActionFleetSize()*10;
                        Supply();
                    }
                    else
                    {
                        Debug.Log("NOT");
                        isFleetSupplied = false;
                        foreach (var item in currentFleetText.GetComponentsInChildren<TextMeshProUGUI>())
                        {
                            if(item.gameObject.name.IndexOf("Current")==-1)
                            {
                                item.text = "Is Fleet Supplied? False";
                                break;
                            }
                        }
                    }
                    
                }
            }
            else
            {
                fleet["Battleship"] = int.Parse(fleetInfo[0])>0?int.Parse(fleetInfo[0]):0;
                fleet["Cruiser"] = int.Parse(fleetInfo[1])>0?int.Parse(fleetInfo[1]):0;
                fleet["Frigate"] = int.Parse(fleetInfo[2])>0?int.Parse(fleetInfo[2]):0;
                if(xPos==GameManager.conqueredX&&yPos==GameManager.conqueredY)
                {
                    fleet["Battleship"] -= Test.BBELost;
                    fleet["Cruiser"] -= Test.CAELost+Test.CLELost;
                    fleet["Frigate"] -= Test.FFELost;
                }
                if(faction==1) // faction 1
                {
                    nameTag.color = Color.red;
                }
                else if(faction==2) // faction 2
                {
                    nameTag.color = Color.yellow;
                }
                else if(faction==3) // faction 3
                {
                    nameTag.color = Color.magenta;
                }
                else if(faction==4) // faction 4
                {
                    nameTag.color = Color.white;
                }
                // InvokeRepeating(nameof(ProbeDefense),10f,UnityEngine.Random.Range(20, 40));
            }
        }
        Save();
        // 
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Mouse0)&&Input.GetAxis("Mouse X")!=0)
        {
            foreach (var item in GetComponentsInChildren<UnityEngine.UI.Image>())
            {
                if(item.name.IndexOf("List")!=-1)
                {
                    if(item.GetComponent<RectTransform>().localScale.x!=0)
                    {
                        mobolizedFleet.Clear();
                        foreach(string type in fleet.Keys)
                        {
                            if(!type.Equals("Cruiser"))
                                GameObject.FindWithTag(type).GetComponent<TextMeshProUGUI>().text = 0.ToString();
                        }
                        item.GetComponent<RectTransform>().localScale=Vector3.zero;
                    }
                }
            }
        }
        
    }
    
    public void Show()
    {
        foreach (var item in GameObject.FindGameObjectsWithTag("PlanetButton"))
        {
            if(item.transform.parent!=gameObject)
            item.GetComponent<RectTransform>().localScale=Vector3.zero;
        }
        mobolizedFleet.Clear();
        GameObject.FindWithTag("Battleship").GetComponent<TextMeshProUGUI>().text = "0";
        GameObject.FindWithTag("Heavy Cruiser").GetComponent<TextMeshProUGUI>().text = "0";
        GameObject.FindWithTag("Light Cruiser").GetComponent<TextMeshProUGUI>().text = "0";
        GameObject.FindWithTag("Frigate").GetComponent<TextMeshProUGUI>().text = "0";
        GameObject.FindWithTag("Tanker").GetComponent<TextMeshProUGUI>().text = "0";
        foreach (var item in GetComponentsInChildren<UnityEngine.UI.Image>())
        {
            if(item.name.IndexOf("List")!=-1)
            {
                if(item.GetComponent<RectTransform>().localScale.x==0)
                {
                    if(!isFriendly)
                    {
                        if(!((xPos>0&&planetsGrid[xPos-1,yPos]!=null&&planetsGrid[xPos-1,yPos].isFriendly)||(xPos<4&&planetsGrid[xPos+1,yPos]!=null&&planetsGrid[xPos+1,yPos].isFriendly)||(yPos<4&&planetsGrid[xPos,yPos+1]!=null&&planetsGrid[xPos,yPos+1].isFriendly)||(yPos>0&&planetsGrid[xPos,yPos-1]!=null&&planetsGrid[xPos,yPos-1].isFriendly)))
                        {
                            return;
                        }
                        foreach (var o in GetComponentsInChildren<TextMeshProUGUI>())
                        {
                            if(o.name.IndexOf("Enemy")!=-1)
                            {
                                o.text="Enemy Fleet:\n"+fleet["Battleship"]+"*Battleship\n"+fleet["Cruiser"]+"*Cruiser\n"+fleet["Frigate"]+"*Frigate";
                                break;
                            }
                        }
                        if(xPos>0&&planetsGrid[xPos-1,yPos]!=null&&planetsGrid[xPos-1,yPos].isFriendly&&planetsGrid[xPos-1,yPos].isFleetSupplied)
                        {
                            string s=(xPos-1).ToString()+yPos.ToString();
                            mobolizedFleet.Add(s,planetsGrid[xPos-1,yPos].fleet);
                        }
                        if(xPos<4&&planetsGrid[xPos+1,yPos]!=null&&planetsGrid[xPos+1,yPos].isFriendly&&planetsGrid[xPos+1,yPos].isFleetSupplied)
                        {
                            string s=(xPos+1).ToString()+yPos.ToString();
                            mobolizedFleet.Add(s,planetsGrid[xPos+1,yPos].fleet);
                        }
                        if(yPos<4&&planetsGrid[xPos,yPos+1]!=null&&planetsGrid[xPos,yPos+1].isFriendly&&planetsGrid[xPos,yPos+1].isFleetSupplied)
                        {
                            string s=xPos.ToString()+(yPos+1).ToString();
                            mobolizedFleet.Add(s,planetsGrid[xPos,yPos+1].fleet);
                        }
                        if(yPos>0&&planetsGrid[xPos,yPos-1]!=null&&planetsGrid[xPos,yPos-1].isFriendly&&planetsGrid[xPos,yPos-1].isFleetSupplied)
                        {
                            string s=xPos.ToString()+(yPos-1).ToString();
                            mobolizedFleet.Add(s,planetsGrid[xPos,yPos-1].fleet);
                        }

                        int BB=0,CA=0,CL=0,FF=0,TT=0;
                        foreach (var i in mobolizedFleet.Values)
                        {
                            BB+=i["Battleship"];
                            CA+=i["Heavy Cruiser"];
                            CL+=i["Light Cruiser"];
                            FF+=i["Frigate"];
                            TT+=i["Tanker"];
                        }                                    
                        GameObject.FindWithTag("Battleship").GetComponent<TextMeshProUGUI>().text = BB.ToString();
                        GameObject.FindWithTag("Heavy Cruiser").GetComponent<TextMeshProUGUI>().text = CA.ToString();
                        GameObject.FindWithTag("Light Cruiser").GetComponent<TextMeshProUGUI>().text = CL.ToString();
                        GameObject.FindWithTag("Frigate").GetComponent<TextMeshProUGUI>().text = FF.ToString();
                        GameObject.FindWithTag("Tanker").GetComponent<TextMeshProUGUI>().text = TT.ToString();
                    }
                    else
                    {
                        currentFleetText.text="Fleet stationed here:\n"+fleet["Battleship"]+" Battleship\n"+fleet["Heavy Cruiser"]+" Heavy Crsuiers\n"+fleet["Light Cruiser"]+" Light Cuiser\n"+fleet["Frigate"]+" Frigate\n"+fleet["Tanker"]+" Tanker";
                    }
                    item.GetComponent<RectTransform>().localScale=new Vector3(0.01f,0.01f,0.01f);
                }
                else
                {
                    // mobolizedFleet.Clear();
                    foreach(string type in fleet.Keys)
                    {
                        if(!type.Equals("Cruiser"))
                            GameObject.FindWithTag(type).GetComponent<TextMeshProUGUI>().text = 0.ToString();
                    }
                    item.GetComponent<RectTransform>().localScale=Vector3.zero;

                }
                    
                break;
            }
        }
    }
}
