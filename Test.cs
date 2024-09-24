using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Threading;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// The class for player
/// </summary>
public class Test : MonoBehaviour
{
    Rigidbody body;
    ConstantForce cf;
    Camera cam;
    public GameObject circle,missile,fireball,fire,shield,mesh,mobileButton;
    public static Dictionary<GameObject,GameObject> keyValuePairs; 
    Vector3 localLockScale;
    public static GameObject target;
    ShieldF sh;
    public int beHit,maxThrust,missileOptions;
    public float originalDrag,originalAngularDrag;
    public static GameObject player;
    public static bool concentrateLasers;
    int EID;
    public float xScale;
    public static int BBYLost, CAYLost, CLYLost, FFYLost, TTYLost, BBELost, CAELost, CLELost, FFELost, TTELost;
    public static int yourTotalCasualty, enemyTotalCasualty;
    TextMeshProUGUI targetText;
    bool isEngineOn,isAutoPilot;
    float rotationSpeed;
    public int damageError;
    GameObject myshield;
    public static int autopilotLevel, shieldLevel, thrustLevel;
    void Awake()
    {
        TTYLost=TTELost=BBELost=BBYLost=CAYLost=CAELost=CLYLost=CLELost=FFYLost=FFELost=yourTotalCasualty=enemyTotalCasualty = 0;
        xScale = transform.localScale.x;
        isAutoPilot = concentrateLasers = false;
        EID = 0;
        damageError = UnityEngine.Random.Range(-500,500);
        isEngineOn = true;
        rotationSpeed = 1f/xScale * UnityEngine.Random.Range(0.85f,1.15f);;
        Screen.SetResolution(3840,2160,true);
        Screen.fullScreenMode=FullScreenMode.ExclusiveFullScreen;
        originalDrag = GetComponent<Rigidbody>().drag;
        originalAngularDrag = GetComponent<Rigidbody>().angularDrag;
        player=gameObject;
        GetComponentInChildren<UnityEngine.UI.Button>().interactable = false;
        Screen.SetResolution(3840,2160,true);
        missileOptions = 0;
        target=GameObject.FindWithTag("Enemy");
        targetText = GameObject.FindWithTag("Target").GetComponent<TextMeshProUGUI>();
        // InvokeRepeating(nameof(FindClosestFoe),3f,30f);
        FindClosestFoe();
        maxThrust=(int)transform.localScale.x*225*(name.IndexOf("Oracle")!=-1?2:1);
        keyValuePairs = new Dictionary<GameObject, GameObject>();
        body = GetComponent<Rigidbody>();
        cf = GetComponent<ConstantForce>();
        cam = GetComponentInChildren<Camera>();
        autopilotLevel = PlayerPrefs.GetInt("AutopilotLevel",0);
        shieldLevel = PlayerPrefs.GetInt("ShieldLevel",0);
        thrustLevel = PlayerPrefs.GetInt("ThrustLevel",0);

        ScaleThrust(thrustLevel+1);

        if(transform.localScale.x>=8f&&name.IndexOf("Oracle")==-1)
        {
            GameObject FT;
            if(Chooser.costumeCode[0]==0)
            {
                FT = Instantiate(GameManager.manager.BBFT,mesh.transform);
            }
            else
            {
                FT = Instantiate(GameManager.manager.BBFT2,mesh.transform);
            }
            FT.transform.localPosition = Vector3.zero;
            FT.transform.localScale = Vector3.one;
            FT.transform.localRotation = Quaternion.identity;

            if(Chooser.costumeCode[1]==1)
            {
                GameObject MT = Instantiate(GameManager.manager.BBMT,mesh.transform);
                MT.transform.localPosition = Vector3.zero;
                MT.transform.localScale = Vector3.one;
                MT.transform.localRotation = Quaternion.identity;
            }

            GameObject BT;
            if(Chooser.costumeCode[2]==0)
            {
                BT = Instantiate(GameManager.manager.BBBT,mesh.transform);
            }
            else
            {
                BT = Instantiate(GameManager.manager.BBBT2,mesh.transform);
            }
            BT.transform.localPosition = Vector3.zero;
            BT.transform.localScale = Vector3.one;
            BT.transform.localRotation = Quaternion.identity;
            
        }
        else if(transform.localScale.x>=5f&&name.IndexOf("Oracle")==-1)
        {
            GameObject FT;
            if(Chooser.costumeCode[3]==0)
            {
                FT = Instantiate(GameManager.manager.CAFT,mesh.transform);
            }
            else
            {
                FT = Instantiate(GameManager.manager.CAFT2,mesh.transform);
            }
            FT.transform.localPosition = Vector3.zero;
            FT.transform.localScale = Vector3.one;
            FT.transform.localRotation = Quaternion.identity;

            if(Chooser.costumeCode[4]==1)
            {
                GameObject MT = Instantiate(GameManager.manager.CAMT,mesh.transform);
                MT.transform.localPosition = Vector3.zero;
                MT.transform.localScale = Vector3.one;
                MT.transform.localRotation = Quaternion.identity;
            }

            GameObject BT;
            if(Chooser.costumeCode[5]==0)
            {
                BT = Instantiate(GameManager.manager.CABT,mesh.transform);
            }
            else
            {
                BT = Instantiate(GameManager.manager.CABT2,mesh.transform);
            }
            BT.transform.localPosition = Vector3.zero;
            BT.transform.localScale = Vector3.one;
            BT.transform.localRotation = Quaternion.identity;
            // transform.LookAt(target.transform);
        }
        else if(transform.localScale.x>=3f&&name.IndexOf("Oracle")==-1)
        {
            if(Chooser.costumeCode[6]==1)
            {
                GameObject MT = Instantiate(GameManager.manager.CLMT,mesh.transform);
                MT.transform.localPosition = Vector3.zero;
                MT.transform.localScale = Vector3.one;
                MT.transform.localRotation = Quaternion.identity;
            }
            // transform.LookAt(target.transform);
        }
        
        InvokeRepeating(nameof(IncrementHP),5f,10f);
        foreach (var item in GetComponentsInChildren<SphereCollider>())
        {
            if(item.name.IndexOf("Turret")!=-1&& item.GetComponent<SphereCollider>()!=null)
            {
                item.gameObject.AddComponent(typeof(Turret));
                item.gameObject.AddComponent(typeof(Rigidbody));
                Rigidbody body = item.GetComponent<Rigidbody>();
                body.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
                body.useGravity = false;
            }
        }
        if(shield!=null)
        {
            myshield = Instantiate(shield,transform.position,transform.rotation);
            sh=myshield.GetComponent<ShieldF>();
            sh.HP=(int)transform.localScale.x*2000;
            sh.p=gameObject;
            myshield.transform.localScale=new Vector3(transform.localScale.x*35,transform.localScale.x*35,transform.localScale.x*35);
            ScaleShield(sh, shieldLevel+1);
        }
        
        GameObject.FindWithTag("Lose").transform.localScale=new Vector3(0,0,0);
        GameObject.FindWithTag("Win").transform.localScale=new Vector3(0,0,0);

        if(SystemInfo.deviceType != DeviceType.Handheld)
        {
            mobileButton.transform.localScale = Vector3.zero;
        }
    }
    public void ChangeTargetSubtract()
    {
        if(EID >0)
            EID -= 1;
        else
            EID = GameManager.ENum-1;
        target = GameObject.FindGameObjectsWithTag("Enemy")[EID];
    }
    public void ChangeTargetAdd()
    {
        if(EID <GameManager.ENum-1)
            EID += 1;
        else
            EID = 0;
        target = GameObject.FindGameObjectsWithTag("Enemy")[EID];
    }
    public void AddThrustMobile()
    {
        if(cf.relativeForce.magnitude<maxThrust)
        cf.relativeForce += new Vector3(0f,0f,5f*xScale);
    }
    public void SubtractThrustMobile()
    {
        if(cf.relativeForce.magnitude>5f*xScale)
        cf.relativeForce -= new Vector3(0f,0f,5f*xScale);
    }
    void ScaleThrust(int scale)
    {
        maxThrust*= scale;
        rotationSpeed*=1f+scale/2f;
        cf.force*=scale;
    }
    void ScaleShield(ShieldF shield, int scale)
    {
        shield.HP*=scale;
    }
    void FindFoe()
    {
        if(autopilotLevel!=0)
        {
            target = GameObject.FindWithTag("Enemy");
            if(autopilotLevel==2)
            {
                GameObject.FindWithTag("ConMDropdown").GetComponent<TMP_Dropdown>().value = 2;
                GameObject.FindWithTag("ConLDropdown").GetComponent<TMP_Dropdown>().value = 1;
            }
            for(int i = 0; i < GameObject.FindGameObjectsWithTag("Enemy").Length; i++)
            {
                var e = GameObject.FindGameObjectsWithTag("Enemy")[i];
                if((e.transform.position-transform.position).magnitude < 1400)
                {
                    target = e;
                    return;
                }
            }
        }
        FindClosestFoe();

    }
    void FixedUpdate()
    {
        GameObject.FindWithTag("Speed").GetComponent<TextMeshProUGUI>().text = "Speed: "+(int)(body.velocity.magnitude*10f);
        foreach (var item in GetComponentsInChildren<Beam>())
        {
            item.enemy=target;
        }
        if(target!=null&&(target.transform.position-transform.position).magnitude<2000f)
        {
            GetComponentInChildren<UnityEngine.UI.Button>().interactable = true;
        }
        GameObject.FindWithTag("Thrust").GetComponent<TextMeshProUGUI>().text = "Thrust: " + ((int)cf.relativeForce.magnitude).ToString();
        GameObject.FindWithTag("Damage").GetComponent<TextMeshProUGUI>().text = "Damage: " + beHit.ToString();
        if(sh!=null&&sh.HP>0) GameObject.FindWithTag("ShieldHP").GetComponent<TextMeshProUGUI>().text = "Shield HP: " + sh.HP.ToString();
        else GameObject.FindWithTag("ShieldHP").GetComponent<TextMeshProUGUI>().text = "Shield HP: 0";
        GameObject.FindWithTag("Turret").GetComponent<TextMeshProUGUI>().text = "Operating Turrets: " + GetComponentsInChildren<Turret>().Length.ToString();
        
        
        if(beHit>(6000+damageError)*xScale)
        {
            
            Instantiate(fireball,transform.position,transform.rotation);
            foreach (var item in GetComponentsInChildren<Light>())
            {
                if(item.type!=LightType.Directional)
                item.enabled = false;
            }
            foreach (var item in GetComponentsInChildren<Beam>())
            {
                item.enabled = false;
            }
            foreach (ParticleSystem p in GetComponentsInChildren<ParticleSystem>())
            {
                p.Stop();
            }
            GameObject f = Instantiate(fire,transform);
            f.transform.localPosition = Vector3.zero;
            gameObject.tag = "Untagged";
            CancelInvoke(nameof(IncrementHP));
            GameManager.YNum-=1;
            // foreach (var item in GetComponentsInChildren<MeshCollider>())
            // {
            //     item.gameObject.AddComponent<Rigidbody>();
            // }
            var thread = new Thread(GameManager.CleaningThread)
            {
                IsBackground = true,
                Priority = System.Threading.ThreadPriority.BelowNormal
            };
            thread.Start();
            if(xScale>7)
            {
                BBYLost+=1;
            }
            else if(xScale>4)
            {
                if(name.IndexOf("Oracle")==-1)
                CAYLost+=1;
                else
                TTYLost+=1;
            }
            else if(xScale>1)
            {
                CLYLost+=1;
            }
            else 
            {
                FFYLost+=1;
            }
            yourTotalCasualty+=(int)xScale*100;
            GameObject.FindWithTag("Warning").GetComponent<TextMeshProUGUI>().color = Color.red;
            GameObject.FindWithTag("Warning").GetComponent<TextMeshProUGUI>().text="Abandon Ship!!!";
            GetComponent<Test>().enabled = false;
        }
        else if(beHit>4000*xScale)
        {
            GameObject.FindWithTag("Warning").GetComponent<TextMeshProUGUI>().color = Color.red;
            GameObject.FindWithTag("Warning").GetComponent<TextMeshProUGUI>().text="Warning: Severe Structural Damage.\nDisengage to Avoid Destruction";
            if(isAutoPilot&&!IsInvoking(nameof(Launch)))
            {
                InvokeRepeating(nameof(Launch),0f,0.6f);
            }
        }
        else if(beHit>3000*xScale)
        {
            GameObject.FindWithTag("Warning").GetComponent<TextMeshProUGUI>().color = Color.yellow;
            GameObject.FindWithTag("Warning").GetComponent<TextMeshProUGUI>().text="Caution: Heavy Strutural Damage.\nConsider Disengage for Damage Control";
        }
        else
        {
            GameObject.FindWithTag("Warning").GetComponent<TextMeshProUGUI>().text="";
        }
    }
    void IncrementHP()
    {
        int increase = (60-(int)body.velocity.magnitude)*6;
        if(increase < 0)
        {
            increase=50;
        }
        if(beHit>increase)
        {
            beHit-=increase;
        }
        else
        {
            beHit=0;
        }
    }
    void FindClosestFoe()
    {
        target = GameObject.FindWithTag("Enemy");
        foreach (var item in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            if((item.transform.position-transform.position).magnitude<(target.transform.position-transform.position).magnitude)
            {
                target = item;
            }
        }
        
    }
    public void SetLaserConcentrate(Int32 i)
    {
        if(i==0)
        {
            concentrateLasers = false;
            GameObject.FindWithTag("ConMDropdown").GetComponent<TMP_Dropdown>().interactable = true;
        }
        else if(i==1)
        {
            concentrateLasers = true;
            GameObject.FindWithTag("ConMDropdown").GetComponent<TMP_Dropdown>().interactable = false;
        }
    }
    void Start()
    {
        
        for (int i=0; i< GameObject.FindGameObjectsWithTag("Enemy").Length;i++)
        {
            GameObject target = GameObject.FindGameObjectsWithTag("Enemy")[i];
            GameObject c=Instantiate(circle,GameObject.FindWithTag("Canvas").transform);
            Vector3 screenPos = cam.WorldToScreenPoint(target.transform.position);
            float h = Screen.height;
            float w = Screen.width;
            float x = screenPos.x - (w / 2);
            float y = screenPos.y - (h / 2);
            c.transform.localPosition=new Vector3(x,y,0);
            keyValuePairs.Add(target,c);
            localLockScale = c.transform.localScale;
        }
        Invoke(nameof(InitializeTurret),1f);
    }
    void InitializeTurret()
    {
        ForwardTurretControl(1);
        MiddleTurretControl(0);
        BackwardTurretControl(1);
    }
    
    // Update is called once per frame
    void Update()
    {
        body.drag = originalDrag * (1f+beHit/8000f);
        body.angularDrag = originalAngularDrag * (1f+beHit/10000f);
        if(target!=null&&isAutoPilot&&isEngineOn)
        {
            if((transform.position-target.transform.position).magnitude<xScale*165f&&GetComponentInChildren<Turret>()!=null)
            {
                TurnBroadsideToTarget();
                if((transform.position.y-target.transform.position.y)>0&&(transform.position.y-target.transform.position.y)<160&&myshield==null)
                {
                    body.AddRelativeTorque(Vector3.left*0.0007f*body.mass,ForceMode.Acceleration);
                }
                else if((transform.position.y-target.transform.position.y)<0&&(transform.position.y-target.transform.position.y)>-160&&myshield==null)
                {
                    body.AddRelativeTorque(-Vector3.left*0.0007f*body.mass,ForceMode.Acceleration);
                }
                if(GetComponentInChildren<Beam>().priorityCode!=2)
                {
                    GameObject.FindWithTag("LaserDropdown").GetComponent<TMP_Dropdown>().value = 2;
                }
            }
            else
            {
                TurnFrontToTarget();
                GameObject.FindWithTag("LaserDropdown").GetComponent<TMP_Dropdown>().value = 0;
            }
            if(target.tag.IndexOf("Enemy")==-1)
            {
                FindFoe();
            }
        }
        if(target!=null)
            targetText.text = "Current Target: "+target.name[..target.name.IndexOf("(")];
        if(EID > GameManager.ENum-1)
        {
            EID = 0;
        }
        // if(Input.GetKeyDown(KeyCode.Tab))
        // {
        //     GameObject.FindWithTag("MainCamera").transform.localEulerAngles+=new Vector3(0f,90f,0f);
        //     if(GameObject.FindWithTag("MainCamera").transform.localRotation.y%180>45f)
        //     {}
        //     // gameObject.GetComponentInChildren<Canvas>().transform.Rotate(GameObject.FindWithTag("MainCamera").transform.up,90f);
        // }
        if(Input.GetKeyDown(KeyCode.LeftArrow)||Input.GetKeyDown(KeyCode.UpArrow))
        {
            if(EID >0)
                EID -= 1;
            else
                EID = GameManager.ENum-1;
            target = GameObject.FindGameObjectsWithTag("Enemy")[EID];
        }
        else if(Input.GetKeyDown(KeyCode.RightArrow)||Input.GetKeyDown(KeyCode.DownArrow))
        {
            if(EID <GameManager.ENum-1)
                EID += 1;
            else
                EID = 0;
            target = GameObject.FindGameObjectsWithTag("Enemy")[EID];
        }

        foreach (GameObject t in keyValuePairs.Keys)
        {
            Vector3 screenPos = cam.WorldToScreenPoint(t.transform.position);
            float h = Screen.height;
            float w = Screen.width;
            float x = screenPos.x - (w / 2);
            float y = screenPos.y - (h / 2);
            if(keyValuePairs[t]!=null)
                if(screenPos.z>0)
                {
                    keyValuePairs[t].transform.localPosition=new Vector3(x,y,0);
                    keyValuePairs[t].transform.localScale = localLockScale;
                }
                else 
                {
                    keyValuePairs[t].transform.localScale = Vector3.zero;
                }
        }
        if(target==null||target.tag.IndexOf("Enemy")==-1)
        {
            FindFoe();
        }
        if(cf.relativeForce.magnitude>maxThrust)
        {
            cf.relativeForce.Set(0f,0f,maxThrust);
        }
        else if(cf.relativeForce.magnitude<10f)
        {
            cf.relativeForce.Set(0f,0f,10f);
        }
        if(Input.GetKeyUp(KeyCode.Mouse1))
        {
            Launch();
        }
        
        // body.AddRelativeTorque(new Vector3(Input.GetAxis("Vertical")*500f*Time.deltaTime,0f,Input.GetAxis("Horizontal")*50f*Time.deltaTime));
        if(Input.GetAxis("Mouse X")!=0)
        {
            if(Input.GetKey(KeyCode.LeftShift))
            {
                //transform.Rotate(Vector3.up,Input.GetAxis("Mouse X"));
                body.AddRelativeTorque(0f,Input.GetAxis("Mouse X"),0f,ForceMode.Acceleration);
            }
            else
            {
                //transform.Rotate(-Vector3.forward,Input.GetAxis("Mouse X"));
                body.AddRelativeTorque(0f,0f,-Input.GetAxis("Mouse X"),ForceMode.Acceleration);
            }
            
        }
        
        if(Input.GetAxis("Mouse Y")!=0)
        {
            //transform.Rotate(Vector3.left,Input.GetAxis("Mouse Y"));
            body.AddRelativeTorque(-Input.GetAxis("Mouse Y"),0f,0f,ForceMode.Acceleration);
        }

        float propel=Input.GetAxis("Mouse ScrollWheel");
        if(propel!=0&&isEngineOn)
        {
            if(propel>0&&cf.relativeForce.magnitude<=maxThrust)
            {
                cf.relativeForce += new Vector3(0f,0f,propel);
            }
            else if(propel<0&&cf.relativeForce.magnitude>=10f)
            {
                cf.relativeForce += new Vector3(0f,0f,propel);
            }
        }
    }
    public void BackwardTurretControl(Int32 i)
    {
        FireTurretOn(i,"Backward");
    }
    public void ForwardTurretControl(Int32 i)
    {
        FireTurretOn(i,"Forward");
    }
    public void MiddleTurretControl(Int32 i)
    {
        FireTurretOn(i,"Middle");
    }
    void FireTurretOn(Int32 i,string turretKeyword)
    {
        if(xScale<2) return;
        foreach (var item in GetComponentsInChildren<Beam>())
        {
            if(item.turret.gameObject.transform.parent.parent.name.IndexOf(turretKeyword) != -1)
            {
                if(i==0)
                    item.fireOnET = 0;
                else
                    item.fireOnET = 1;
            }
        }
    }
    private void TurnBroadsideToTarget()
    {
        // Calculate the direction to the target
        Vector3 directionToTarget = target.transform.position - transform.position;

        // Ignore the vertical component to only rotate around the Y-axis (left and right)
        directionToTarget.y = 0;

        // Create a rotation to look at the target
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

        // Smoothly interpolate between the current rotation and the target rotation
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * 0.3f * Time.deltaTime);
    }
    private void TurnFrontToTarget()
    {
        Vector3 directionToTarget = target.transform.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

        // Use Slerp to smoothly interpolate between current rotation and target rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
    public void Launch()
    {
        if(target==null||!enabled)
        {
            CancelInvoke(nameof(Launch));
            return;
        }
        GameObject tube = null;
        foreach(GameObject g in GameObject.FindGameObjectsWithTag("UnusedLC"))
        {
            if(g.GetComponentInParent<Test>()!=null&&g.GetComponentInParent<Test>().gameObject==gameObject)
            {
                tube = g;
                break;
            }
        }
        if(tube!=null)
        {
            tube.tag = "UsedLC";
            GameObject m = Instantiate(missile, tube.transform.position,new Quaternion(tube.transform.rotation.x-90,tube.transform.rotation.y,tube.transform.rotation.z,tube.transform.rotation.w));
            m.transform.Rotate(new Vector3(90,0,0));
            m.GetComponent<Rigidbody>().AddForce(100*tube.transform.forward,ForceMode.Impulse);
            if(target!=null&&missileOptions!=1)
            m.GetComponent<MissileF>().target=target;
            else
            m.GetComponent<MissileF>().target=GameObject.FindWithTag("Enemy");
        }
        else
        {
            Debug.Log("Run out of missile");
        }
    }
    public void Salvo()
    {
        // Carrier.Salvo();
        if(tag.IndexOf("Unta")==-1)
        Launch();
        foreach (var item in GameObject.FindGameObjectsWithTag("Player"))
        {
            var fr = item.GetComponent<FriendlyVessel>();
            if(fr!=null)
            {
                fr.Launch();
            }
        }
    }
    static void RunOnStart()
    {
        Application.wantsToQuit += CheckSceneName;
    }
    static bool CheckSceneName()
    {
        return SceneManager.GetActiveScene().name.IndexOf("Map")!=-1;
    }
    public void SetLaserPriority(Int32 priority)
    {
        foreach (Beam item in GetComponentsInChildren<Beam>())
        {
            item.priorityCode=priority;
        }
    }
    public void SetMissilePriority(Int32 priority)
    {
        missileOptions = priority;
        FriendlyVessel.missileOptions = priority;
    }
    public void EngineOnOrOff(Boolean on)
    {
        ParticleSystem[] engines = new ParticleSystem[2];
        int i = 0;
        foreach (var item in GetComponentsInChildren<ParticleSystem>())
        {
            if(item.name.IndexOf("Ignition")!=-1)
            {
                engines[i] = item;
                i++;
            }
        }
        Debug.Log("Engines"+engines[0].name);
        i--;
        if(on)
        {
            cf.relativeForce = new Vector3(0f,0f,maxThrust/2f);
            isEngineOn = true;
            while(i>=0&&engines[i]!=null)
            {
                Debug.Log(engines[i].name);
                // if(!engines[i].isPlaying)
                    engines[i].Play();
                i--;
            }
        }
        else
        {
            cf.relativeForce = Vector3.zero;
            isEngineOn = false;
            while(i>=0&&engines[i]!=null)
            {
                // if(engines[i].isPlaying)
                    engines[i].Stop();
                i--;
            }
        }
    }
    public void switchAutoPilot(Boolean on)
    {
        if (on)
        {
            isAutoPilot = true;
            InvokeRepeating(nameof(FindFoe),3f,15f);
        }
        else
        {
            isAutoPilot = false;
            CancelInvoke(nameof(FindFoe));
            CancelInvoke(nameof(Launch));
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(tag.IndexOf("Unta")==-1&&collision.gameObject.GetComponentInParent<Carrier>() != null&&collision.gameObject.GetComponentInParent<Carrier>().enabled&&collision.gameObject.layer!=3)
        {
            Instantiate(fireball,transform.position,transform.rotation);
            Instantiate(collision.gameObject.GetComponentInParent<Carrier>().fireball,collision.gameObject.transform.position,Quaternion.identity);
            beHit+=5001;
            collision.gameObject.GetComponentInParent<Carrier>().beHit+=5001;
            enemyTotalCasualty+=UnityEngine.Random.Range(50, 80);
            yourTotalCasualty+=UnityEngine.Random.Range(50, 80);
        }
    }
}
