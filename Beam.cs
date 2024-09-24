using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// distance : 7500
/// <summary>
/// Beam for friendly units
/// </summary>
public class Beam : MonoBehaviour
{
    public GameObject enemy;
    ParticleSystem ps;
    public int priorityCode;
    public GameObject turret;
    public int fireOnET;
    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        // fireOnET = Random.Range(0,2);
        priorityCode = 0;
        enemy = GameObject.FindWithTag("Enemy");
    }
    void PrioritizeOnMissile()
    {
        RaycastHit hit;
        if(GameObject.FindWithTag("Missile") != null &&Physics.Raycast(transform.position,GameObject.FindWithTag("Missile").transform.position-transform.position,out hit,Mathf.Infinity)&&hit.transform.gameObject.CompareTag("Missile"))
        {
            transform.LookAt(GameObject.FindWithTag("Missile").transform);
            if(!ps.isPlaying)
                ps.Play();
                        
            
        }
        else if(enemy != null &&Physics.Raycast(transform.position,enemy.transform.position-transform.position,out hit,Mathf.Infinity)&&(hit.transform.gameObject.CompareTag("Enemy")||hit.transform.gameObject.GetComponent<Shield>()!=null)&&hit.distance<2000f)
        {
            if(enemy.GetComponentInChildren<BeamE>()!=null&&enemy.GetComponentInChildren<BeamE>().enabled&&fireOnET==0)
                transform.LookAt(enemy.GetComponentInChildren<BeamE>().turret.transform);
            else 
                transform.LookAt(enemy.transform);            
            if(!ps.isPlaying)
                ps.Play();
                      
        }
        else
        {
            if(ps.isPlaying)
                ps.Stop();
        }
    }
    void EqualPriority()
    {
        RaycastHit hit;
        if(GameObject.FindWithTag("Missile") != null &&Physics.Raycast(transform.position,GameObject.FindWithTag("Missile").transform.position-transform.position,out hit,Mathf.Infinity)&&hit.transform.gameObject.CompareTag("Missile")&&(enemy==null||(GameObject.FindWithTag("Missile").transform.position-transform.position).magnitude<(enemy.transform.position-transform.position).magnitude))
        {
            transform.LookAt(GameObject.FindWithTag("Missile").transform);
            if(!ps.isPlaying)
                ps.Play();
                        
            
        }
        else if(enemy != null &&Physics.Raycast(transform.position,enemy.transform.position-transform.position,out hit,Mathf.Infinity)&&(hit.transform.gameObject.CompareTag("Enemy")||hit.transform.gameObject.GetComponent<Shield>()!=null)&&hit.distance<2000f)
        {
            if(enemy.GetComponentInChildren<BeamE>()!=null&&enemy.GetComponentInChildren<BeamE>().enabled&&fireOnET==0)
                transform.LookAt(enemy.GetComponentInChildren<BeamE>().turret.transform);
            else 
                transform.LookAt(enemy.transform);            
            if(!ps.isPlaying)
                ps.Play();
                      
        }
        else
        {
            if(ps.isPlaying)
                ps.Stop();
        }
    }
    void PrioritizeOnShip()
    {
        RaycastHit hit;
        if(enemy != null &&Physics.Raycast(transform.position,enemy.transform.position-transform.position,out hit,Mathf.Infinity)&&(hit.transform.gameObject.CompareTag("Enemy")||hit.transform.gameObject.GetComponent<Shield>()!=null)&&hit.distance<2000f)
        {
            if(enemy.GetComponentInChildren<BeamE>()!=null&&enemy.GetComponentInChildren<BeamE>().enabled&&fireOnET==0)
                transform.LookAt(enemy.GetComponentInChildren<BeamE>().turret.transform);
            else 
                transform.LookAt(enemy.transform);            
            if(!ps.isPlaying)
                ps.Play();
                      
        }
        else if(GameObject.FindWithTag("Missile") != null &&Physics.Raycast(transform.position,GameObject.FindWithTag("Missile").transform.position-transform.position,out hit,Mathf.Infinity)&&hit.transform.gameObject.CompareTag("Missile"))
        {
            transform.LookAt(GameObject.FindWithTag("Missile").transform);
            if(!ps.isPlaying)
                ps.Play();
        }
        else
        {
            if(ps.isPlaying)
                ps.Stop();
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(enemy!=null&&!enemy.CompareTag("Enemy"))
        {
            enemy=GameObject.FindWithTag("Enemy");
        }
        switch (priorityCode)
        {
            case 0:
                EqualPriority();
                break;
            case 1:
                PrioritizeOnMissile();
                break;
            case 2:
                PrioritizeOnShip();
                break;
            default:
                break;
        }
    }
    void OnParticleCollision(GameObject other)
    {
        if(other.GetComponent<Missile>()!=null)
        {
            other.GetComponent<Missile>().HP-=1;
        }
        if(other.GetComponent<Turret>()!=null&& other.GetComponentInParent<Carrier>()!=null)
        {
            // Debug.Log("Haha");
            if(other.GetComponent<Turret>().bE!=null)
            {
                other.GetComponent<Turret>().HP-=1;

            }
        }
        else if(other.GetComponent<Shield>()!=null)
        {
            other.GetComponent<Shield>().HP-=1;
        }
        else if(other.GetComponentInParent<Carrier>()!=null)
        {
            other.GetComponentInParent<Carrier>().beHit+=1;
        }
        
    }
}
