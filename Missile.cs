using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

/// <summary>
/// The class for enemy missiles
/// </summary>
public class Missile : MonoBehaviour
{    
    public float duration = 120f;
    public float speed = 600f;           // Missile speed
    public float endPhaseSpeed;
    public float rotationSpeed = 5f;    // Rotation speed to face the target
    public float proportionalGain = 0.1f; // Proportional gain for guidance
    public GameObject target,explosion;
    Rigidbody rb;
    bool closeRange;
    public int HP;

    void Awake()
    {
        // speed = 600;
        endPhaseSpeed = speed*2.75f;
        rb = GetComponent<Rigidbody>();
        HP = (int)(rb.mass*140);
        StartCoroutine(CanExplode());
        StartCoroutine(OutOfFuel());
    }
    void Start()
    {
        Vector3 relativePosition = target.transform.position - transform.position;
        if(relativePosition.magnitude<1400f)
        {
            closeRange = true;
            proportionalGain*=3.5f;
        }
    }
    // Awake protection for the missile to avoid collisions with vessels of the same side
    IEnumerator CanExplode()
    {
        yield return new WaitForSeconds(0.75f);
        GetComponent<CapsuleCollider>().isTrigger=false;
    }
    // Update is called once per frame
    void Update()
    {
        if(HP<1)
        {
            Detonate();
        }
        if(target!=null&&!target.CompareTag("Player"))
        {
            target = GameObject.FindGameObjectsWithTag("Player")[^1];
        }
        // proportional navigation
        if (target != null)
        {
            // Calculate the relative position and velocity
            Vector3 relativePosition = target.transform.position - transform.position;
            Vector3 relativeVelocity = target.GetComponent<Rigidbody>().velocity - rb.velocity;

            // two stage acceleration
            if(!closeRange&&relativePosition.magnitude<850f)
            {
                speed = endPhaseSpeed;
            }

            // Calculate the proportional navigation guidance command
            Vector3 guidanceCommand = proportionalGain * Vector3.Cross(relativePosition, relativeVelocity.normalized);

            // Apply rotation based on the guidance command
            rb.angularVelocity = guidanceCommand * rotationSpeed;

            // Move the missile forward
            rb.velocity = transform.forward * speed;
        }
        else
        {
            Detonate();
        }
    }
    // Detonate the missile
    public void Detonate()
    {
        Instantiate(explosion, transform.position, new Quaternion(-90,0,0,0));
        Destroy(gameObject);
    }
    // Make the missile stop after a certain amount of time
    IEnumerator OutOfFuel()
    {
        yield return new WaitForSeconds(duration);
        target = null;
    }
    // Enter when the missile collides with another object
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject);
        if(collision.gameObject.name.IndexOf("Turret")!=-1&&collision.gameObject.GetComponentInParent<Carrier>()==null)
        {
            if(collision.gameObject.GetComponent<Turret>()!=null&&collision.gameObject.GetComponent<Turret>().b!=null)
            {
                collision.gameObject.GetComponent<Turret>().HP-=HP*12;
            }
        }
        else if(collision.gameObject.GetComponent<ShieldF>()!=null)
        {
            collision.gameObject.GetComponent<ShieldF>().HP-=HP*3;
        }
        else if(collision.gameObject.GetComponentInParent<Test>()!=null)
        {
            if(collision.gameObject.GetComponentInParent<Test>().beHit>0)
                collision.gameObject.GetComponentInParent<Test>().beHit+=HP*(int)(collision.gameObject.GetComponentInParent<Test>().beHit/5000f+1f);
            else
                collision.gameObject.GetComponentInParent<Test>().beHit+=HP*2;
            Test.yourTotalCasualty+=Random.Range(2, 6);
        }
        else if(collision.gameObject.GetComponentInParent<FriendlyVessel>() != null)
        {
            if(collision.gameObject.GetComponentInParent<FriendlyVessel>().beHit>0)
                collision.gameObject.GetComponentInParent<FriendlyVessel>().beHit+=HP*(int)(collision.gameObject.GetComponentInParent<FriendlyVessel>().beHit/5000f+1f);
            else
                collision.gameObject.GetComponentInParent<FriendlyVessel>().beHit+=HP*2;
            Test.yourTotalCasualty+=Random.Range(2, 6);
        }
        
        
        Detonate();
    }
}
