using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using System.Threading;
using Unity.VisualScripting;
using TMPro;

/// <summary>
/// The class for the generic implementation
/// of all enemy vessels
/// </summary>
public class Carrier : MonoBehaviour
{
    public int beHit; // stores the hits this object sustained
    public GameObject missile,fireball,fire,target;
    Rigidbody body;
    public float rotationSpeed = 0.02f;  // The speed at which the ship rotates
    public float xScale;
    public GameObject shield;
    ConstantForce force;
    public float originalDrag,originalAngularDrag;
    public static GameObject flagTarget;
    string unusedTag,usedTag;
    bool canConcentrate, hDamaged;
    GameObject currentShield;
    public int damageError;
    void Awake()
    {
        hDamaged = false;
        originalAngularDrag = GetComponent<Rigidbody>().angularDrag;
        originalDrag = GetComponent<Rigidbody>().drag;
        target = GameObject.FindWithTag("Player");
        if(gameObject==GameObject.FindGameObjectsWithTag("Enemy")[0])
        {
            flagTarget = target;
        }
        beHit = 0;
        damageError = UnityEngine.Random.Range(-500, 500);
        body = GetComponent<Rigidbody>();
        force = GetComponent<ConstantForce>();
        InvokeRepeating(nameof(FindClosestFoe),3f,10f);
        FindClosestFoe();
        xScale = transform.localScale.x;
        rotationSpeed = 1f/xScale * Random.Range(0.85f,1.15f);
        if(xScale>=8f)
        {
            canConcentrate = true;
            unusedTag = "UnusedLB";
            usedTag = "UsedLB";
            StartCoroutine(HoldFiring(CanFireC,nameof(Launch),0.9f));
        }
        else if(xScale>=5f)
        {
            canConcentrate = true;
            unusedTag = "UnusedLC";
            usedTag = "UsedLC";
            StartCoroutine(HoldFiring(CanFireC,nameof(Launch),1.65f));
        }
        else if(xScale>=3f)
        {
            canConcentrate = true;
            unusedTag = "UnusedLC";
            usedTag = "UsedLC";
            StartCoroutine(HoldFiring(CanFireF,nameof(Launch),1.025f));
        }
        else
        {
            canConcentrate = false;
            unusedTag = "UnusedLF";
            usedTag = "UsedLF";
            StartCoroutine(HoldFiring(CanFireF,nameof(Launch),0.075f));
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
            currentShield = Instantiate(shield,transform.position,transform.rotation);
            currentShield.GetComponent<Shield>().HP=(int)xScale*2000;
            currentShield.GetComponent<Shield>().p=gameObject;
            currentShield.transform.localScale=new Vector3(xScale*35,xScale*35,xScale*35);
        }
        
        FireTurretOn();
        
    }
    IEnumerator PrepareSalvo()
    {
        yield return new WaitForSeconds(Random.Range(10,15));
        StartCoroutine(StopSalvo());

        InvokeRepeating("Launch",0f,0.075f);

    }
    IEnumerator StopSalvo()
    {
        yield return new WaitForSeconds(1f);
        CancelInvoke("Launch");
        InvokeRepeating("Launch",0f,2f);
    }
    
    public static void Salvo()
    {
        foreach (var item in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            var fr = item.GetComponent<Carrier>();
            if(fr!=null)
            {
                fr.Launch();
            }
        }
    }
    void FireTurretOn()
    {
        int i = UnityEngine.Random.Range(0, 2);
        foreach (var item in GetComponentsInChildren<BeamE>())
        {
            if(i==0)
                item.fireOnET = 0;
            else
                item.fireOnET = 1;
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
        target = GameObject.FindWithTag("Player");
        foreach (var item in GameObject.FindGameObjectsWithTag("Player"))
        {
            if((item.transform.position-transform.position).magnitude<(target.transform.position-transform.position).magnitude)
            {
                target = item;
            }
        }
        foreach (var item in GetComponentsInChildren<BeamE>())
        {
            item.enemy=target;
        }
        if(gameObject==GameObject.FindGameObjectsWithTag("Enemy")[0])
        {
            flagTarget = target;
        }
    }
    void FixedUpdate()
    {
        
        if(gameObject==Test.target)
        {
            Test.keyValuePairs[gameObject].GetComponent<UnityEngine.UI.Image>().color=new Color(255,0,0,255);
        }
        else
        {
            Test.keyValuePairs[gameObject].GetComponent<UnityEngine.UI.Image>().color=new Color(0,255,0,255);
        }
        
        // if all friendly units are dead
        if(GameObject.FindWithTag("Player")==null)
        {
            CancelInvoke(nameof(Launch));
            CancelInvoke(nameof(FindClosestFoe));
        }
        if(beHit>(6000+damageError)*xScale)
        {
            Instantiate(fireball,transform.position,transform.rotation);
            foreach (var item in GetComponentsInChildren<Light>())
            {
                item.enabled = false;
            }
            foreach (var item in GetComponentsInChildren<BeamE>())
            {
                // item.CancelInvoke(nameof(item.ChooseEnemy));
                item.enabled = false;
            }
            foreach (ParticleSystem p in GetComponentsInChildren<ParticleSystem>())
            {
                p.Stop();
            }
            GameObject f = Instantiate(fire,transform);
            f.transform.localPosition = Vector3.zero;
            gameObject.tag = "Untagged";
            Destroy(Test.keyValuePairs[gameObject]);
            GetComponent<ConstantForce>().relativeForce = Vector3.zero;
            CancelInvoke(nameof(Launch));
            CancelInvoke(nameof(FindClosestFoe));
            CancelInvoke(nameof(IncrementHP));
            StopAllCoroutines();
            GameManager.ENum-=1;
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
            if(name.IndexOf("anker") >= 0)
            {
                Test.TTELost+=1;
            }
            else if(xScale>7)
            {
                Test.BBELost+=1;
            }
            else if(xScale>4)
            {
                Test.CAELost+=1;
            }
            else if(xScale>1)
            {
                Test.CLELost+=1;
            }
            else 
            {
                Test.FFELost+=1;
            }
            Test.enemyTotalCasualty+=(int)xScale*100;
            GetComponent<Carrier>().enabled = false;
        }
        else if(beHit>2500*xScale&&!hDamaged)
        {
            hDamaged=true;
            InvokeRepeating(nameof(Launch),0f,0.35f);
        }
        else if (beHit>4000*xScale)
        {
            if(GetComponentInChildren<BeamE>()!=null&&GetComponentInChildren<BeamE>().priorityCode!=2)
            {
                for(int i=0;i<GetComponentsInChildren<BeamE>().Length/2;i++)
                {
                    GetComponentsInChildren<BeamE>()[i].priorityCode=2;
                }
            }
        }
        if(target==null)
        {
            CancelInvoke(nameof(Launch));
            CancelInvoke(nameof(FindClosestFoe));
        }
    }
    void Update()
    {
        body.drag = originalDrag * (1f+beHit/8000f);
        body.angularDrag = originalAngularDrag * (1f+beHit/10000f);
        if(GameObject.FindWithTag("Player")!=null&&GameObject.FindWithTag("Player").GetComponent<Test>()!=null&&GameObject.FindWithTag("Player").GetComponent<Test>().enabled)
            Test.keyValuePairs[gameObject].GetComponentInChildren<TextMeshProUGUI>().text = ((int)(transform.position-GameObject.FindWithTag("Player").transform.position).magnitude).ToString();
        if(target!=null)
        {
            if((transform.position-target.transform.position).magnitude<xScale*160f&&GetComponentInChildren<Turret>()!=null)
            {
                TurnBroadsideToTarget();
                // if(currentShield==null)
                if((transform.position.y-target.transform.position.y)>0&&(transform.position.y-target.transform.position.y)<130&&currentShield==null)
                {
                    body.AddRelativeTorque(Vector3.left*0.0013f*body.mass*Random.Range(0.9f,1.1f),ForceMode.Acceleration);
                }
                else if((transform.position.y-target.transform.position.y)<0&&(transform.position.y-target.transform.position.y)>-130&&currentShield==null)
                {
                    body.AddRelativeTorque(-Vector3.left*0.0013f*body.mass*Random.Range(0.9f,1.1f),ForceMode.Acceleration);
                }
                if(GetComponentInChildren<BeamE>().priorityCode!=2)
                {
                    foreach (var item in GetComponentsInChildren<BeamE>())
                    {
                        item.priorityCode = 2;
                    }
                }
            }
            else
            {
                TurnFrontToTarget();
            }
            if(target.tag.IndexOf("Player")==-1)
            {
                FindClosestFoe();
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
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * 0.35f * Time.deltaTime);
    }
    private void TurnFrontToTarget()
    {
        Vector3 directionToTarget = target.transform.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

        // Use Slerp to smoothly interpolate between current rotation and target rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
    private void TurnBackToTarget()
    {
        Vector3 directionToTarget =  transform.position - target.transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

        // Use Slerp to smoothly interpolate between current rotation and target rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
    bool CanFireC()
    {
        return (transform.position-target.transform.position).magnitude<2500f;
    }
    bool CanFireF()
    {
        return (transform.position-target.transform.position).magnitude<700f;
    }
    IEnumerator HoldFiring(System.Func<bool> func, string fireFuncName, float rate)
    {
        yield return new WaitUntil(func);
        InvokeRepeating(fireFuncName,0f,rate);
        StartCoroutine(PrepareSalvo());
    }
    void Launch()
    {
        // Debug.Log("Launching");
        GameObject tube = null;
        foreach(GameObject g in GameObject.FindGameObjectsWithTag(unusedTag))
        {
            if(g.GetComponentInParent<Carrier>()!=null&&g.GetComponentInParent<Carrier>().gameObject==gameObject)
            {
                tube = g;
                break;
            }
        }
        if(tube!=null)
        {
            tube.tag = usedTag;
            GameObject m = Instantiate(missile, tube.transform.position,new Quaternion(tube.transform.rotation.x-90,tube.transform.rotation.y,tube.transform.rotation.z,tube.transform.rotation.w));
            m.transform.Rotate(new Vector3(90,0,0));
            m.GetComponent<Rigidbody>().AddForce(100*tube.transform.forward,ForceMode.Impulse);
            // int r = Random.Range(0,3);
            // if(xScale<2)
            // {
            //     m.GetComponent<Missile>().target=GameObject.FindWithTag("Player");
            // }
            if(canConcentrate&&flagTarget!=null)
                m.GetComponent<Missile>().target=flagTarget;
            else
                m.GetComponent<Missile>().target=target;
        }
        else
        {
            CancelInvoke(nameof(Launch));
        }
    }
}
