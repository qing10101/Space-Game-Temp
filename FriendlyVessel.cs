using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System;
using Unity.VisualScripting;

/// <summary>
/// The class for friendly units
/// </summary>
public class FriendlyVessel : MonoBehaviour
{
    bool outOfMissile, hDamaged;
    public static int missileOptions;
    public int beHit; // stores the hits this object sustained
    public GameObject missile,fireball,fire,target,shield,mesh;
    Rigidbody body;
    ConstantForce force;
    GameObject currentShield;
    float originalDrag, originalAngularDrag;
    public int damageError;
    public float maxThrust;
    public float xScale;
    public float rotationSpeed = 1f;  // The speed at which the ship rotates
    void Awake()
    {
        hDamaged = false;
        maxThrust=(int)transform.localScale.x*225;
        xScale = transform.localScale.x;
        originalDrag=GetComponent<Rigidbody>().drag;
        originalAngularDrag=GetComponent<Rigidbody>().angularDrag;
        missileOptions = 0;
        outOfMissile = false;
        beHit = 0;
        damageError = UnityEngine.Random.Range(-500,500);
        rotationSpeed = 1f/xScale * UnityEngine.Random.Range(0.90f,1.1f);
        body = GetComponent<Rigidbody>();
        force = GetComponent<ConstantForce>();
        target=GameObject.FindWithTag("Enemy");
        InvokeRepeating(nameof(FindFoe),3f,8f);
        FindFoe();
        if(transform.localScale.x>=8f&&name.IndexOf("anker")==-1)
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
            
            StartCoroutine(HoldFiring(CanFireC,nameof(Launch),1.025f));
        }
        else if(transform.localScale.x>=5f&&name.IndexOf("anker")==-1)
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
            StartCoroutine(HoldFiring(CanFireC,nameof(Launch),2.025f));
        }
        else if(transform.localScale.x>=3f&&name.IndexOf("anker")==-1)
        {
            if(Chooser.costumeCode[6]==1)
            {
                GameObject MT = Instantiate(GameManager.manager.CLMT,mesh.transform);
                MT.transform.localPosition = Vector3.zero;
                MT.transform.localScale = Vector3.one;
                MT.transform.localRotation = Quaternion.identity;
            }
            // transform.LookAt(target.transform);
            StartCoroutine(HoldFiring(CanFireF,nameof(Launch),0.085f));
        }
        else
        {
            // transform.LookAt(target.transform);
            StartCoroutine(HoldFiring(CanFireF,nameof(Launch),0.08f));
        }
        InvokeRepeating(nameof(IncrementHP),5f,10f);
        foreach (var item in GetComponentsInChildren<SphereCollider>())
        {
            if(item.name.IndexOf("Turret")!=-1&& item.GetComponent<SphereCollider>()!=null)
            {
                item.GetComponentInChildren<Beam>().priorityCode=1;
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
            currentShield.GetComponent<ShieldF>().HP=(int)transform.localScale.x*2000;
            currentShield.GetComponent<ShieldF>().p=gameObject;
            currentShield.transform.localScale=new Vector3(transform.localScale.x*35,transform.localScale.x*35,transform.localScale.x*35);
        }
        
    }

    void ScaleShield(ShieldF shield, int scale)
    {
        shield.HP*=scale;
    }
    IEnumerator PrepareSalvo()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(10,15));
        StartCoroutine(StopSalvo());

        InvokeRepeating("Launch",0f,0.075f);

    }
    IEnumerator StopSalvo()
    {
        yield return new WaitForSeconds(1f);
        CancelInvoke("Launch");
        InvokeRepeating("Launch",0f,2f);
    }
    void Start()
    {
        Invoke(nameof(InitializeTurret),1f);
        if(shield!=null)
        ScaleShield(currentShield.GetComponent<ShieldF>(),Test.shieldLevel+1);
    }
    void InitializeTurret()
    {
        if(name.IndexOf("anker")!=-1)
        {
            ForwardTurretControl(0);
            MiddleTurretControl(0);
            BackwardTurretControl(0);
            return;
        }
        int r=UnityEngine.Random.Range(0,2);
        ForwardTurretControl(r);
        r=UnityEngine.Random.Range(0,2);
        MiddleTurretControl(r);
        r=UnityEngine.Random.Range(0,2);
        BackwardTurretControl(r);
    }
    void FixedUpdate()
    {
        // if all enemy units are dead
        if(GameObject.FindWithTag("Enemy")==null)
        {
            CancelInvoke(nameof(Launch));
            CancelInvoke(nameof(FindFoe));
        }
        if(beHit>(6000+damageError)*xScale)
        {
            Instantiate(fireball,transform.position,transform.rotation);
            foreach (var item in GetComponentsInChildren<Light>())
            {
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
            GetComponent<ConstantForce>().relativeForce = Vector3.zero;
            CancelInvoke(nameof(Launch));
            CancelInvoke(nameof(FindFoe));
            CancelInvoke(nameof(IncrementHP));
            StopAllCoroutines();
            GameManager.YNum-=1;
            var thread = new Thread(GameManager.CleaningThread)
            {
                IsBackground = true,
                Priority = System.Threading.ThreadPriority.BelowNormal
            };
            thread.Start();
            if(xScale>7)
            {
                Test.BBYLost+=1;
            }
            else if(xScale>4)
            {
                if(name.IndexOf("anker")==-1)
                Test.CAYLost+=1;
                else
                Test.TTYLost+=1;
            }
            else if(xScale>1)
            {
                Test.CLYLost+=1;
            }
            else 
            {
                Test.FFYLost+=1;
            }
            Test.yourTotalCasualty+=(int)xScale*100;
            GetComponent<FriendlyVessel>().enabled = false;
        }
        else if(beHit>4000*xScale&&!hDamaged)
        {
            hDamaged = true;
            InvokeRepeating(nameof(Launch),0f,0.25f);
        }
        else if( beHit>2000*xScale)
        {
            if(GetComponentInChildren<Beam>()!=null&&GetComponentInChildren<Beam>().priorityCode!=2)
            {
                for(int i=0;i<GetComponentsInChildren<Beam>().Length/2;i++)
                {
                    GetComponentsInChildren<Beam>()[i].priorityCode=2;
                }
            }
        }
    }
    public void Launch()
    {
        // Debug.Log("Launching");
        GameObject tube = null;
        foreach(GameObject g in GameObject.FindGameObjectsWithTag("UnusedLC"))
        {
            if(g.GetComponentInParent<FriendlyVessel>()!=null&&g.GetComponentInParent<FriendlyVessel>().gameObject==gameObject)
            {
                tube = g;
                break;
            }
        }
        if(GameObject.FindWithTag("Enemy")==null)
        {
            CancelInvoke(nameof(Launch));
            return;
        }
        if(tube!=null)
        {
            tube.tag = "UsedLC";
            GameObject m = Instantiate(missile, tube.transform.position,new Quaternion(tube.transform.rotation.x-90,tube.transform.rotation.y,tube.transform.rotation.z,tube.transform.rotation.w));
            m.transform.Rotate(new Vector3(90,0,0));
            m.GetComponent<Rigidbody>().AddForce(100*tube.transform.forward,ForceMode.Impulse);
            if(missileOptions==0)
            m.GetComponent<MissileF>().target=target;
            else if(missileOptions==2)
            m.GetComponent<MissileF>().target=Test.target;
            else
            m.GetComponent<MissileF>().target=GameObject.FindWithTag("Enemy");
        }
        else
        {
            outOfMissile = true;
            CancelInvoke(nameof(Launch));
        }
    }
    bool CanFireC()
    {
        if(target==null) return false;
        return (transform.position-target.transform.position).magnitude<2200f;
    }
    bool CanFireF()
    {
        if(target==null) return false;
        return (transform.position-target.transform.position).magnitude<705f;
    }
    IEnumerator HoldFiring(System.Func<bool> func, string fireFuncName, float rate)
    {
        yield return new WaitUntil(func);
        InvokeRepeating(fireFuncName,0f,rate);
        if(rate>2)
        {
            StartCoroutine(PrepareSalvo());
        }
    }
    void FindFoe()
    {
        if(Test.concentrateLasers&&Test.target!=null)
        {
            target = Test.target;
            foreach (var item in GetComponentsInChildren<Beam>())
            {
                item.enemy=target;
            }
            return;
        }
        else
        {
            GameObject.FindWithTag("Enemy");
        }
        if(Test.autopilotLevel!=0)
        {
            for(int i = 0; i < GameObject.FindGameObjectsWithTag("Enemy").Length; i++)
            {
                var e = GameObject.FindGameObjectsWithTag("Enemy")[i];
                if((e.transform.position-transform.position).magnitude < 1400)
                {
                    target = e;
                    foreach (var item in GetComponentsInChildren<Beam>())
                    {
                        item.enemy=target;
                    }
                    return;
                }
            }
        }
        FindClosestFoe();
        
    }
    void FindClosestFoe()
    {
        if(Test.target!=null) target = Test.target;
        else target = GameObject.FindWithTag("Enemy");
        foreach (var item in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            if((item.transform.position-transform.position).magnitude<(target.transform.position-transform.position).magnitude)
            {
                target = item;
            }
            
        }
        foreach (var item in GetComponentsInChildren<Beam>())
        {
            item.enemy=target;
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
    // Update is called once per frame
    void Update()
    {
        body.drag = originalDrag * (1f+beHit/8000f);
        body.angularDrag = originalAngularDrag * (1f+beHit/10000f);
        if(target!=null)
        {
            if((transform.position-target.transform.position).magnitude<xScale*170f&&GetComponentInChildren<Turret>()!=null)
            {
                TurnBroadsideToTarget();
                if((transform.position.y-target.transform.position.y)>0&&(transform.position.y-target.transform.position.y)<140&&(currentShield==null||(currentShield!=null&&currentShield.GetComponent<ShieldF>().HP<6000)))
                {
                    body.AddRelativeTorque(Vector3.left*0.0012f*body.mass*UnityEngine.Random.Range(0.95f,1.05f),ForceMode.Acceleration);
                }
                else if((transform.position.y-target.transform.position.y)<0&&(transform.position.y-target.transform.position.y)>-140&&(currentShield==null||(currentShield!=null&&currentShield.GetComponent<ShieldF>().HP<6000)))
                {
                    body.AddRelativeTorque(-Vector3.left*0.0012f*body.mass*UnityEngine.Random.Range(0.95f,1.05f),ForceMode.Acceleration);
                }
                if(GetComponentInChildren<Beam>().priorityCode!=2)
                {
                    foreach (var item in GetComponentsInChildren<Beam>())
                    {
                        item.priorityCode = 2;
                    }
                }
            }
            else
            {
                TurnFrontToTarget();
                if(GetComponentInChildren<Beam>()!=null&&GetComponentInChildren<Beam>().priorityCode!=0)
                {
                    foreach (var item in GetComponentsInChildren<Beam>())
                    {
                        item.priorityCode = 0;
                    }
                }
            }
            if(target.tag.IndexOf("Enemy")==-1)
            {
                FindFoe();
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
   
    private void TurnBackToTarget()
    {
        Vector3 directionToTarget =  transform.position - target.transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

        // Use Slerp to smoothly interpolate between current rotation and target rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
    void OnCollisionEnter(Collision collision)
    {
        if(tag.IndexOf("Unta")==-1&&collision.gameObject.GetComponentInParent<Carrier>() != null&& collision.gameObject.GetComponentInParent<Carrier>().enabled&&collision.gameObject.layer!=3)
        {
            Instantiate(fireball,transform.position,transform.rotation);
            Instantiate(collision.gameObject.GetComponentInParent<Carrier>().fireball,collision.gameObject.transform.position,Quaternion.identity);
            beHit+=5001;
            collision.gameObject.GetComponentInParent<Carrier>().beHit+=5001;
            Test.enemyTotalCasualty+=UnityEngine.Random.Range(50, 80);
            Test.yourTotalCasualty+=UnityEngine.Random.Range(50, 80);
        }
    }
}