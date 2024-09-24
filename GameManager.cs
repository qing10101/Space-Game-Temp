using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static int incidentFaction,attackedPlanetFaction,formation;
    public Material earth, jupiter, mars, mercury, venus, earthMobile, jupiterMobile, marsMobile, mercuryMobile, venusMobile;
    public static GameManager manager;
    public GameObject BBFT,BBFT2,BBMT,BBBT,BBBT2,CAFT,CAFT2,CAMT,CABT,CABT2,CLMT;
    public GameObject AppleBattleship, AppleHCruiser, AppleLCruiser, AppleFrigate,AppleTanker;
    public GameObject GoogleBattleship, GoogleHCruiser, GoogleLCruiser, GoogleFrigate,GoogleTanker;
    public GameObject SBBattleship,SBBattleship2, SBHCruiser,SBHCruiser2, SBLCruiser, SBLCruiser2, SBFrigate,SBTanker;
    public GameObject StatBattleship, StatHCruiser, StatLCruiser, StatFrigate, StatTanker;
    public GameObject enemyBattleship, enemyHeavyCruiser, enemyLightCruiser, enemyFrigate, enemyTanker;
    public GameObject friendlyBattleship, friendlyHeavyCruiser, friendlyLightCruiser, friendlyFrigate, friendlyTanker;
    public GameObject myBattleship, myHeavyCruiser, myLightCruiser, myFrigate, myTanker;
    public GameObject friendlyPlanet, enemyPlanet;
    public static int YNum,ENum;
    static bool goHomeRunning;
    public static int money;
    public static bool victory;
    [SerializeField]
    public static int conqueredX, conqueredY;
    public static Planet IncidentPlanet;
    public static bool isResetting;
    public static bool isMissionMode;
    public GameObject  mfoeCA, mfoeCAdef, mfoeCAdef2, mfoeCL, mfoeTanker, mfriendTanker;
    public static int arasakaAnger, rnAnger, googleAnger, cbisAnger;
    void OnApplicationQuit()
    {
        if(SceneManager.GetActiveScene().name.IndexOf("Map")!=-1)
        {
            PlayerPrefs.SetInt("Money", money);
        }
        Test.BBELost=Test.BBYLost=Test.CAELost=Test.CAYLost=Test.CLELost=Test.CLYLost=Test.FFELost=Test.FFYLost=Test.TTELost=Test.TTYLost=0;
        var thread = new Thread(CleaningThread)
        {
            IsBackground = true,
            Priority = System.Threading.ThreadPriority.BelowNormal
        };
        thread.Start();
    }
    public void Reset()
    {
        IncidentPlanet = null;
        conqueredX = conqueredY = -50;
        isResetting=true;
        if(Planet.planetsGrid!=null)
            foreach (var item in Planet.planetsGrid)
            {
                if(item!=null)
                    Destroy(item);
            }
        var thread = new Thread(CleaningThread)
        {
            IsBackground = true,
            Priority = System.Threading.ThreadPriority.BelowNormal
        };
        thread.Start();
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetString("Planet4,1","Sunnyvale\n4\n0,5,16\n");
        PlayerPrefs.SetString("Planet4,2","Santa Clara\n4\n0,2,14\n");
        PlayerPrefs.SetString("Planet4,3","San Jose\n4\n0,4,12\n");
        PlayerPrefs.SetString("Planet3,0","Campbell\n4\n0,3,15\n");
        PlayerPrefs.SetString("Planet2,1","Sunol\n4\n0,1,10\n");
        PlayerPrefs.SetString("Planet4,0","East Foothills\n4\n0,3,13\n");


        PlayerPrefs.SetString("Planet3,2","Los Gatos\n2\n0,2,2\n");
        PlayerPrefs.SetString("Planet3,1","Saratoga\n2\n2,4,3\n");
        PlayerPrefs.SetString("Planet4,4","Cambrian Park\n2\n3,3,4\n");
        PlayerPrefs.SetString("Planet2,0","Alamitos\n2\n1,2,2\n");
        PlayerPrefs.SetString("Planet3,4","Lexington Hills\n2\n4,4,3\n");
        PlayerPrefs.SetString("Planet3,3","New Almadon\n2\n0,5,4\n");

        PlayerPrefs.SetString("Planet0,2","Mountain View\n3\n1,3,4\n");
        PlayerPrefs.SetString("Planet1,4","Alviso\n3\n0,4,4\n");
        PlayerPrefs.SetString("Planet2,3","Fremont\n3\n0,2,4\n");
        PlayerPrefs.SetString("Planet2,4","Alum Rock\n3\n1,4,3\n");
        PlayerPrefs.SetString("Planet0,3","Newark\n3\n2,2,4\n");
        PlayerPrefs.SetString("Planet0,4","Union City\n3\n1,4,3\n");

        PlayerPrefs.SetString("Planet2,2","Cupertino\n0\n0,0,2,2,1\n0,0");
        
        
        PlayerPrefs.SetString("Planet1,1","Milpitas\n1\n1,2,2\n");
        PlayerPrefs.SetString("Planet0,1","Los Altos\n1\n3,2,2\n");
        PlayerPrefs.SetString("Planet0,0","Palo Alto\n1\n3,5,2\n");
        PlayerPrefs.SetString("Planet1,0","San Manteo\n1\n4,1,3\n");
        PlayerPrefs.SetString("Planet1,3","Milbrae\n1\n0,2,3\n");
        PlayerPrefs.SetString("Planet1,2","Stanford\n1\n0,1,1\n");

        PlayerPrefs.SetInt("ShieldCode", 1);
        money=50000;
        PlayerPrefs.SetInt("Money",50000);
        PlayerPrefs.SetInt("AutopilotLevel",0);
        PlayerPrefs.SetInt("ShieldLevel",0);
        PlayerPrefs.SetInt("ThrustLevel",0);

        PlayerPrefs.SetInt("RNAnger",0);
        PlayerPrefs.SetInt("ArasakaAnger",0);
        PlayerPrefs.SetInt("GoogleAnger",0);
        PlayerPrefs.SetInt("CBISAnger",0);
        
        SceneManager.LoadScene("Map",LoadSceneMode.Single);
        // isResetting=false;
    }
    void Start()
    {
        
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void DeclineArmistice()
    {
        if(IncidentPlanet!=null)
        {
            IncidentPlanet.PreceedAttack();
            IncidentPlanet = null;
        }
    }
    public void AcceptArmistice()
    {
        if(IncidentPlanet!=null)
        {
            IncidentPlanet.AcceptArmistice();
            IncidentPlanet = null;
        }
    }
    public void DontPayRansom()
    {
        if(IncidentPlanet!=null)
        {
            IncidentPlanet.CounterAttackPreceed();
            IncidentPlanet = null;
        }
    }
    public void PayRansom()
    {
        if(IncidentPlanet!=null)
        {
            IncidentPlanet.PayRansom();
            IncidentPlanet = null;
        }
    }
    void Awake()
    {
        isResetting=false;
        IncidentPlanet = null;

        // Screen.SetResolution(Display.main.renderingWidth,Display.main.renderingHeight,true);
        Screen.fullScreenMode=FullScreenMode.FullScreenWindow;

        if(UnityEngine.Application.platform == RuntimePlatform.Android)
        {
            earth = earthMobile;
            jupiter = jupiterMobile;
            mars = marsMobile;
            venus = venusMobile;
            mercury = mercuryMobile;
        }
        


        goHomeRunning=false;
        if(SceneManager.GetActiveScene().name.IndexOf("Start")!=-1)
        {
            string model = UnityEngine.SystemInfo.deviceModel;
            int coreNum = UnityEngine.SystemInfo.processorCount;
            int coreFreq = UnityEngine.SystemInfo.processorFrequency;
            string gpuName = UnityEngine.SystemInfo.graphicsDeviceName;
            int vram = UnityEngine.SystemInfo.graphicsMemorySize;
            int ram = UnityEngine.SystemInfo.systemMemorySize;
            GameObject.FindWithTag("SystemInfo").GetComponent<TextMeshProUGUI>().text = "Device Model: "+model+"\nRAM: "+ram+" MB\nCPU: "+coreNum+" cores at "+coreFreq+" MHz\nGPU: "+gpuName+"\nVRAM: "+vram+" MB";
            if(ram<4000||(coreFreq<1200&&coreFreq!=0)||coreNum<3||vram<1000)
            {
                GameObject.FindWithTag("SystemInfo").GetComponent<TextMeshProUGUI>().color = Color.yellow;
                GameObject.FindWithTag("SystemInfo").GetComponent<TextMeshProUGUI>().text += "\nCaution: You Are Not Meeting Our Basic System Requirements. This Game May Damage Your Device!";
            }
        }
        else if(SceneManager.GetActiveScene().name.IndexOf("Mission")!=-1)
        {
            GameObject.Find("Canvas").GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, 2160);
            Chooser.costumeCode = new int[7];
            int storedCode = PlayerPrefs.GetInt("Code", -1);
            if(storedCode != -1)
            {
                for (int i = 0; i < 7; i++)
                {
                    Chooser.costumeCode[7-i-1] = storedCode%10;
                    storedCode /= 10;
                }
            }
        }
        else if(SceneManager.GetActiveScene().name.IndexOf("Map")!=-1)
        {
            if(PlayerPrefs.GetInt("Money",-1)==-1)
            {
                Reset();
            }
            Chooser.costumeCode = new int[7];
            int storedCode = PlayerPrefs.GetInt("Code", -1);
            if(storedCode != -1)
            {
                for (int i = 0; i < 7; i++)
                {
                    Chooser.costumeCode[7-i-1] = storedCode%10;
                    storedCode /= 10;
                }
            }
            Planet.planetsGrid=new Planet[5,5];
            Planet.mobolizedFleet = new Dictionary<string, Dictionary<string, int>>();
            
            rnAnger = PlayerPrefs.GetInt("RNAnger",0);
            arasakaAnger = PlayerPrefs.GetInt("ArasakaAnger",0);
            googleAnger = PlayerPrefs.GetInt("GoogleAnger",0);
            cbisAnger = PlayerPrefs.GetInt("CBISAnger",0);

            money=PlayerPrefs.GetInt("Money",50000);
            for(int i = 0; i <= 4; i++)
            {
                for(int j = 0; j <= 4; j++)
                {
                    string[] planetInfo = PlayerPrefs.GetString("Planet"+i+","+j).Split('\n');
                    if(planetInfo.Length>1)
                    {
                        GameObject t=null;
                        if(planetInfo[1].IndexOf("0")!=-1)
                        {
                            t = Instantiate(friendlyPlanet,GameObject.FindWithTag("Board").transform);
                            
                        }
                        else
                        {
                            t = Instantiate(enemyPlanet,GameObject.FindWithTag("Board").transform);
                        }
                        if(t!=null)
                        {
                            t.transform.localPosition = new Vector3((i-2), (j-2), 0);
                            t.transform.localRotation = Quaternion.identity;
                        }

                    }
                }
            }
        }
        else if(SceneManager.GetActiveScene().name.IndexOf("Option")!=-1)
        {
            var thread = new Thread(CleaningThread)
            {
                IsBackground = true,
                Priority = System.Threading.ThreadPriority.BelowNormal
            };
            thread.Start();
        }
        if(manager == null)
        {
            manager = this;
            conqueredX = conqueredY = -1;

            if(SceneManager.GetActiveScene().name.IndexOf("Start")!=-1)
                DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
        
    }
    public void EnterConquerMode()
    {
        var thread = new Thread(CleaningThread)
        {
            IsBackground = true,
            Priority = System.Threading.ThreadPriority.BelowNormal
        };
        thread.Start();
        SceneManager.LoadScene("Map",LoadSceneMode.Single);
    }
    public void EnterMissionMode()
    {
        var thread = new Thread(CleaningThread)
        {
            IsBackground = true,
            Priority = System.Threading.ThreadPriority.BelowNormal
        };
        thread.Start();
        SceneManager.LoadScene("MissionMode",LoadSceneMode.Single);
    }
    public void EnterMainMenu()
    {
        if(SceneManager.GetActiveScene().name.IndexOf("Map")!=-1)
            foreach (var item in Planet.planetsGrid)
            {
                if(item!=null)
                {
                    item.Save();
                }
            }
        var thread = new Thread(CleaningThread)
        {
            IsBackground = true,
            Priority = System.Threading.ThreadPriority.BelowNormal
        };
        thread.Start();
        SceneManager.LoadScene("StartGame",LoadSceneMode.Single);
    }
    public static void CleaningThread()
    {
        GC.Collect(GC.MaxGeneration,GCCollectionMode.Forced);
    }
    void FixedUpdate()
    {
        if (SceneManager.GetActiveScene().name.IndexOf("Map")!=-1)
        {
            formation = GameObject.FindWithTag("Formation").GetComponent<TMP_Dropdown>().value;
            GameObject.FindWithTag("Arasaka").GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, arasakaAnger);
            GameObject.FindWithTag("RN").GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, rnAnger);
            GameObject.FindWithTag("Google").GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, googleAnger);
            GameObject.FindWithTag("CBIS").GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, cbisAnger);
        }
        else if(SceneManager.GetActiveScene().name.IndexOf("Sample")!=-1&&GameObject.FindWithTag("Win")!=null)
        {   
            if(GameObject.FindGameObjectsWithTag("Enemy").Length<1)
            {
                GameObject.FindWithTag("Win").transform.localScale=new Vector3(1,1,1);
                victory = true;
                if(!goHomeRunning)
                {
                    goHomeRunning = true;
                    if(!isMissionMode)
                    {
                        string originalIncidentPlanetInfo = PlayerPrefs.GetString("Planet"+conqueredX+","+conqueredY);
                        string[] infos = originalIncidentPlanetInfo.Split("\n");
                        PlayerPrefs.SetString("Planet"+conqueredX+","+conqueredY,infos[0]+"\n"+"0\n0,0,0,0,0\n0,0");
                        
                    }
                    
                    manager.StartCoroutine(GoBackToHome());
                    
                }
            }
            else if(GameObject.FindGameObjectsWithTag("Player").Length<1)
            {
                victory = false;
                // conqueredX=conqueredY=-5;
                GameObject.FindWithTag("Lose").transform.localScale=new Vector3(1,1,1);
                if(!goHomeRunning)
                {
                    goHomeRunning = true;
                    manager.StartCoroutine(GoBackToHome());

                }
            }
        }
    }
    public void Configure(bool missionMode)
    {
        isMissionMode = missionMode;
        if(!missionMode)
            foreach (var item in Planet.planetsGrid)
            {
                if(item!=null)
                {
                    item.Save();
                }
            }
        var thread = new Thread(CleaningThread)
        {
            IsBackground = true,
            Priority = System.Threading.ThreadPriority.BelowNormal
        };
        thread.Start();
        SceneManager.LoadScene("Customizer",LoadSceneMode.Single);
    }
    public void ManualGoHome()
    {
        if(YNum==0||ENum==0)
        SceneManager.LoadScene("BattleConclusion",LoadSceneMode.Single);

    }

    IEnumerator GoBackToHome()
    {
        yield return new WaitForSeconds(5f);
        ManualGoHome();
    }

    // Update is called once per frame
    void Update()
    {
        if(SceneManager.GetActiveScene().name.IndexOf("Tech")!=-1)
        {
            GameObject.FindWithTag("Money").GetComponent<TextMeshProUGUI>().text = "Money: "+money.ToString();
        }
        if(SceneManager.GetActiveScene().name.IndexOf("Map")!=-1)
        {
            GameObject.FindWithTag("Money").GetComponent<TextMeshProUGUI>().text = "Money: "+money.ToString();
            if(IncidentPlanet==null&&Input.GetKey(KeyCode.Mouse0))
            {
                Camera.main.transform.position+=Time.deltaTime*new Vector3(-Input.GetAxis("Mouse X"),-Input.GetAxis("Mouse Y"),0f);
                float revisedX = Camera.main.transform.position.x;
                float revisedY = Camera.main.transform.position.y;
                if(Camera.main.transform.position.y>4)
                {
                    revisedY = 4;
                }
                if(Camera.main.transform.position.y<-2)
                {
                    revisedY = -2;
                }
                if(Camera.main.transform.position.x<-3)
                {
                    revisedX = -3;
                }
                if(Camera.main.transform.position.x>3)
                {
                    revisedX = 3;
                }
                Camera.main.transform.position = new Vector3(revisedX, revisedY, -10f);
            }
        }
        if(SceneManager.GetActiveScene().name.IndexOf("Sample")!=-1&&GameObject.FindWithTag("Score")!=null)
        {
            GameObject.FindWithTag("Score").GetComponent<TextMeshProUGUI>().text="You "+YNum+" : "+ENum+" Enemy";
        }
        

    }
    public IEnumerator MissionModeSceneLoaded(GameObject you, GameObject friend, GameObject enemy1, GameObject enemy2, int fNum, int e1Num, int e2Num)
    {
        yield return new WaitUntil(IsBattleScene);
        MissionModeGenerate(you, friend, enemy1, enemy2, fNum, e1Num, e2Num);
    }
    public void MissionModeGenerate(GameObject you, GameObject friend, GameObject enemy1, GameObject enemy2, int fNum, int e1Num, int e2Num)
    {
        if(SceneManager.GetActiveScene().name.IndexOf("Sample")!=-1)
        {
            var thread = new Thread(CleaningThread)
            {
                IsBackground = true,
                Priority = System.Threading.ThreadPriority.BelowNormal
            };
            thread.Start();
            int materialRandomizer = UnityEngine.Random.Range(0,5);
            switch (materialRandomizer)
            {
                case 0:
                    GameObject.Find("PlanetSphere").GetComponent<MeshRenderer>().material = earth;
                    break;
                case 1:
                    GameObject.Find("PlanetSphere").GetComponent<MeshRenderer>().material = jupiter;
                    break;
                case 2:
                    GameObject.Find("PlanetSphere").GetComponent<MeshRenderer>().material = mars;
                    break;
                case 3:
                    GameObject.Find("PlanetSphere").GetComponent<MeshRenderer>().material = mercury;
                    break;
                case 4:
                    GameObject.Find("PlanetSphere").GetComponent<MeshRenderer>().material = venus;
                    break;
                default:
                    break;
            }
            YNum=fNum+1;
            ENum=e1Num+e2Num;
            GameObject p = Instantiate(you,new Vector3(100f,0f,0f),new Quaternion(0f,180f,0f,0f));
            GameObject.FindWithTag("Canvas").GetComponent<Canvas>().worldCamera=GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
            int r = UnityEngine.Random.Range(-2,3);
            for (int i = e1Num; i > 0 ; i--)
            {
                Instantiate(enemy1,new Vector3(-6300,r*120,-i*250),new Quaternion(0,90,0,0));
            }
            r = UnityEngine.Random.Range(-2,3);
            for (int i = e2Num; i > 0 ; i--)
            {
                Instantiate(enemy2,new Vector3(-6800,r*100,-i*350),new Quaternion(0,90,0,0));
            }
            r = UnityEngine.Random.Range(-4,1);
            for (int i = fNum; i > 0 ; i--)
            {
                Instantiate(friend,new Vector3(150,r*50,-i*250),new Quaternion(0,-90,0,0));
            }
            p.transform.LookAt(GameObject.FindWithTag("Enemy").transform);
        }
    }
    public IEnumerator SceneLoaded(int eBB, int eC, int eFF, int mBB, int mCA, int mCL, int mFF, int mTT, int fsc, int esc, int material=0)
    {
        yield return new WaitUntil(IsBattleScene);
        GenerateEnemies(eBB, eC, eFF, mBB, mCA, mCL, mFF, mTT, fsc, esc, material);
    }
    bool IsBattleScene()
    {
        return SceneManager.GetActiveScene().name.IndexOf("Sample")!=-1;
    }
    public void GenerateEnemies(int eBNum, int eCNum, int eFNum, int bNum, int hcNum, int lcNum, int fNum, int tNum, int flagshipCode=0, int enemyShipCode=0, int material = 0)
    {
        if(SceneManager.GetActiveScene().name.IndexOf("Sample")!=-1)
        {
            var thread = new Thread(CleaningThread)
            {
                IsBackground = true,
                Priority = System.Threading.ThreadPriority.BelowNormal
            };
            thread.Start();
            switch (material)
            {
                case 0:
                    GameObject.Find("PlanetSphere").GetComponent<MeshRenderer>().material = earth;
                    break;
                case 1:
                    GameObject.Find("PlanetSphere").GetComponent<MeshRenderer>().material = jupiter;
                    break;
                case 2:
                    GameObject.Find("PlanetSphere").GetComponent<MeshRenderer>().material = mars;
                    break;
                case 3:
                    GameObject.Find("PlanetSphere").GetComponent<MeshRenderer>().material = mercury;
                    break;
                case 4:
                    GameObject.Find("PlanetSphere").GetComponent<MeshRenderer>().material = venus;
                    break;
                default:
                    break;
            }

            YNum=bNum+hcNum+lcNum+fNum+tNum+1;
            ENum=eBNum+eCNum+eFNum;
            GameObject myShip=null;
            switch (flagshipCode)
            {
                case 0:
                myShip = myFrigate;
                break;
                case 1:
                myShip = myLightCruiser;
                break;
                case 2:
                myShip = myHeavyCruiser;
                break;
                case 3:
                myShip = myBattleship;
                break;
                case 4:
                myShip = myTanker;
                break;
                default:
                break;
            }
            Assert.IsFalse(myShip==null);
            GameObject p = Instantiate(myShip,new Vector3(100f,0f,0f),new Quaternion(0f,180f,0f,0f));
            switch (enemyShipCode)
            {
                case 0:
                    enemyBattleship = AppleBattleship;
                    enemyHeavyCruiser = AppleHCruiser;
                    enemyLightCruiser = AppleLCruiser;
                    enemyFrigate = AppleFrigate;
                    enemyTanker = AppleTanker;
                    break;
                case 1:
                    int dice = UnityEngine.Random.Range(1, 3);
                    switch (dice)
                    {
                        case 1:
                        enemyBattleship = SBBattleship; break;
                        case 2:
                        enemyBattleship = SBBattleship2; break;
                        default:
                            break;
                    }
                    
                    dice = UnityEngine.Random.Range(1, 3);
                    switch (dice)
                    {
                        case 1:
                        enemyHeavyCruiser = SBHCruiser; break;
                        case 2:
                        enemyHeavyCruiser = SBHCruiser2; break;
                        default:
                            break;
                    }
                    // enemyHeavyCruiser = SBHCruiser;
                    // enemyLightCruiser = SBLCruiser;
                    dice = UnityEngine.Random.Range(1, 3);
                    switch (dice)
                    {
                        case 1:
                        enemyLightCruiser = SBLCruiser; break;
                        case 2:
                        enemyLightCruiser = SBLCruiser2; break;
                        default:
                            break;
                    }
                    enemyFrigate = SBFrigate;
                    enemyTanker = SBTanker;
                    break;
                case 2:
                    enemyBattleship = GoogleBattleship;
                    enemyHeavyCruiser = GoogleHCruiser;
                    enemyLightCruiser = GoogleLCruiser;
                    enemyFrigate = GoogleFrigate;
                    enemyTanker = GoogleTanker;
                    break;
                case 3:
                    enemyBattleship = StatBattleship;
                    enemyHeavyCruiser = StatHCruiser;
                    enemyLightCruiser = StatLCruiser;
                    enemyFrigate = StatFrigate;
                    enemyTanker = StatTanker;
                    break;
                default:
                    break;
            }
            GameObject.FindWithTag("Canvas").GetComponent<Canvas>().worldCamera=GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
            int dic = UnityEngine.Random.Range(0, 2);
            
            int r = UnityEngine.Random.Range(-2,3);
            if (dic == 0)
            {
                for (int i = eBNum; i > 0 ; i--)
                {
                    Instantiate(enemyBattleship,new Vector3(-6300,r*120,-i*290),new Quaternion(0,90,0,0));
                }
                int eLCNum = UnityEngine.Random.Range(0,eCNum+1);
                r = UnityEngine.Random.Range(-2,3);
                for (int i = eCNum-eLCNum; i > 0 ; i--)
                {
                    Instantiate(enemyHeavyCruiser,new Vector3(-6700,r*150,-i*340),new Quaternion(0,90,0,0));
                }
                r = UnityEngine.Random.Range(-2,3);
                for (int i = eLCNum; i > 0 ; i--)
                {
                    Instantiate(enemyLightCruiser,new Vector3(-6500,r*150,-i*310),new Quaternion(0,90,0,0));
                }
                
                
                r = UnityEngine.Random.Range(-2,3);
                for (int i = eFNum; i > 0 ; i--)
                {
                    Instantiate(enemyFrigate,new Vector3(-6800,r*100,-i*360),new Quaternion(0,90,0,0));
                }
            }
            else
            {
                int j = 1;
                for (int i = eBNum; i > 0 ; i--,j++)
                {
                    Instantiate(enemyBattleship,new Vector3(-7000+250*j,r*120,50),new Quaternion(0,90,0,0));
                }
                int eLCNum = UnityEngine.Random.Range(0,eCNum+1);
                r = UnityEngine.Random.Range(-2,3);
                for (int i = eCNum-eLCNum; i > 0 ; i--,j++)
                {
                    Instantiate(enemyHeavyCruiser,new Vector3(-7000+250*j,r*150,50),new Quaternion(0,90,0,0));
                }
                r = UnityEngine.Random.Range(-2,3);
                for (int i = eLCNum; i > 0 ; i--,j++)
                {
                    Instantiate(enemyLightCruiser,new Vector3(-7000+250*j,r*150,50),new Quaternion(0,90,0,0));
                }
                
                
                r = UnityEngine.Random.Range(-2,3);
                for (int i = eFNum; i > 0 ; i--)
                {
                    Instantiate(enemyFrigate,new Vector3(-6800,r*100,-i*220),new Quaternion(0,90,0,0));
                }
            }
            

            if(incidentFaction > 0)
            {
                int eTNum = UnityEngine.Random.Range(1,3);
                ENum+=eTNum;
                r = UnityEngine.Random.Range(-2,3);
                for (int i = eTNum; i > 0 ; i--)
                {
                    Instantiate(enemyTanker,new Vector3(-7000,r*114,-i*280),new Quaternion(0,90,0,0));
                }
            }
            
            if(formation==0)
            {
                r = UnityEngine.Random.Range(-4,1);
                for (int i = bNum; i > 0 ; i--)
                {
                    Instantiate(friendlyBattleship,new Vector3(150,r*50,-i*270),new Quaternion(0,-90,0,0)).transform.LookAt(GameObject.FindWithTag("Enemy").transform);
                }
                r = UnityEngine.Random.Range(-4,1);
                for (int i = hcNum; i > 0 ; i--)
                {
                    Instantiate(friendlyHeavyCruiser,new Vector3(100,r*60,-i*300),new Quaternion(0,-90,0,0)).transform.LookAt(GameObject.FindWithTag("Enemy").transform);
                }
                r = UnityEngine.Random.Range(-4,1);
                for (int i = lcNum; i > 0 ; i--)
                {
                    Instantiate(friendlyLightCruiser,new Vector3(50,r*40,-i*330),new Quaternion(0,-90,0,0)).transform.LookAt(GameObject.FindWithTag("Enemy").transform);
                }
                r = UnityEngine.Random.Range(-4,1);
                for (int i = fNum; i > 0 ; i--)
                {
                    Instantiate(friendlyFrigate,new Vector3(0,r*35,-i*260),new Quaternion(0,-90,0,0)).transform.LookAt(GameObject.FindWithTag("Enemy").transform);
                }
                r = UnityEngine.Random.Range(-4,1);
                for (int i = tNum; i > 0 ; i--)
                {
                    Instantiate(friendlyTanker,new Vector3(0,r*30,-i*290),new Quaternion(0,-90,0,0)).transform.LookAt(GameObject.FindWithTag("Enemy").transform);
                }
            }
            else
            {
                r = UnityEngine.Random.Range(-4,1);
                int j = 1;
                r = UnityEngine.Random.Range(-4,1);
                for (int i = lcNum; i > 0 ; i--,j++)
                {
                    Instantiate(friendlyLightCruiser,new Vector3(j*220,r*5,0),new Quaternion(0,-90,0,0)).transform.LookAt(GameObject.FindWithTag("Enemy").transform);
                }
                r = UnityEngine.Random.Range(-4,1);
                for (int i = hcNum; i > 0 ; i--,j++)
                {
                    Instantiate(friendlyHeavyCruiser,new Vector3(j*220,r*15,0),new Quaternion(0,-90,0,0)).transform.LookAt(GameObject.FindWithTag("Enemy").transform);
                }
                for (int i = bNum; i > 0; i--,j++)
                {
                    Instantiate(friendlyBattleship,new Vector3(j*220,r*10,0),new Quaternion(0,-90,0,0)).transform.LookAt(GameObject.FindWithTag("Enemy").transform);
                }
                
                r = UnityEngine.Random.Range(-4,1);
                for (int i = fNum; i > 0 ; i--)
                {
                    Instantiate(friendlyFrigate,new Vector3(0,r*35,-i*250),new Quaternion(0,-90,0,0)).transform.LookAt(GameObject.FindWithTag("Enemy").transform);
                }
                r = UnityEngine.Random.Range(-4,1);
                for (int i = tNum; i > 0 ; i--)
                {
                    Instantiate(friendlyTanker,new Vector3(0,r*30,-i*290),new Quaternion(0,-90,0,0)).transform.LookAt(GameObject.FindWithTag("Enemy").transform);
                }
            }
            
            p.transform.LookAt(GameObject.FindWithTag("Enemy").transform);
        }

    }

    public void ViewEnemyShips()
    {
        foreach (var item in Planet.planetsGrid)
        {
            if(item!=null)
            {
                item.Save();
            }
        }
        var thread = new Thread(CleaningThread)
        {
            IsBackground = true,
            Priority = System.Threading.ThreadPriority.BelowNormal
        };
        thread.Start();
        SceneManager.LoadScene("CompareTesting",LoadSceneMode.Single);
    }
    public void ViewEnemyFactions()
    {
        foreach (var item in Planet.planetsGrid)
        {
            if(item!=null)
            {
                item.Save();
            }
        }
        var thread = new Thread(CleaningThread)
        {
            IsBackground = true,
            Priority = System.Threading.ThreadPriority.BelowNormal
        };
        thread.Start();
        SceneManager.LoadScene("Factions",LoadSceneMode.Single);
    }
    public void EnterTechTree()
    {
        foreach (var item in Planet.planetsGrid)
        {
            if(item!=null)
            {
                item.Save();
            }
        }
        var thread = new Thread(CleaningThread)
        {
            IsBackground = true,
            Priority = System.Threading.ThreadPriority.BelowNormal
        };
        thread.Start();
        SceneManager.LoadScene("TechTree",LoadSceneMode.Single);
    }
    public void ShowManual()
    {
        if(GameObject.FindWithTag("Manual").transform.localScale.x==1)
        {
            GameObject.FindWithTag("Manual").transform.localScale=Vector3.zero;
        }
        else
        {
            GameObject.FindWithTag("Manual").transform.localScale=Vector3.one;
        }
    }


}
