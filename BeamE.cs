using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Beam for enemy units
/// </summary>
public class BeamE : MonoBehaviour
{
    ParticleSystem ps;
    public GameObject enemy;
    public int priorityCode;
    public GameObject turret;
    public int fireOnET;
    // Start is called before the first frame update
    void Start()
    {
        // fireOnET = Random.Range(0,2);
        enemy = null;
        ps = GetComponent<ParticleSystem>();
    }
    void PrioritizeOnMissile()
    {
        RaycastHit hit;
        if(GameObject.FindWithTag("MissileF") != null &&Physics.Raycast(transform.position,GameObject.FindWithTag("MissileF").transform.position-transform.position,out hit,Mathf.Infinity)&&hit.transform.gameObject.CompareTag("MissileF")&&(enemy==null||(GameObject.FindWithTag("MissileF").transform.position-transform.position).magnitude<(enemy.transform.position-transform.position).magnitude))
        {
            transform.LookAt(GameObject.FindWithTag("MissileF").transform);  
            if(!ps.isPlaying)
                ps.Play();
        }
        else if (enemy!=null&&Physics.Raycast(gameObject.transform.position, enemy.transform.position - gameObject.transform.position, out hit, Mathf.Infinity)&&(hit.transform.gameObject.CompareTag("Player")||hit.transform.gameObject.GetComponent<ShieldF>()!=null)&&hit.distance<2000f)
        {
            if(enemy.GetComponentInChildren<Turret>()!=null&&fireOnET==0)
                transform.LookAt(enemy.GetComponentInChildren<Turret>().transform);
            else 
                transform.LookAt(enemy.transform); 
            if(!ps.isPlaying)
                ps.Play();
        }
        else 
        {
            ps.Stop();
        }
    }
    void EqualPriority()
    {
        RaycastHit hit;
        if(GameObject.FindWithTag("MissileF") != null &&Physics.Raycast(transform.position,GameObject.FindWithTag("MissileF").transform.position-transform.position,out hit,Mathf.Infinity)&&hit.transform.gameObject.CompareTag("MissileF")&&(enemy==null||(GameObject.FindWithTag("MissileF").transform.position-transform.position).magnitude<(enemy.transform.position-transform.position).magnitude))
        {
            transform.LookAt(GameObject.FindWithTag("MissileF").transform.position);
            if(!ps.isPlaying)
                ps.Play();
        }
        else if(enemy != null &&Physics.Raycast(transform.position,enemy.transform.position-transform.position,out hit,Mathf.Infinity)&&(hit.transform.gameObject.CompareTag("Player")||hit.transform.gameObject.GetComponent<ShieldF>()!=null)&&hit.distance<2000f)
        {
            if(enemy.GetComponentInChildren<Turret>()!=null&&fireOnET==0)
                transform.LookAt(enemy.GetComponentInChildren<Turret>().transform);
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
        if (enemy!=null&&Physics.Raycast(gameObject.transform.position, enemy.transform.position - gameObject.transform.position, out hit, Mathf.Infinity)&&(hit.transform.gameObject.CompareTag("Player")||hit.transform.gameObject.GetComponent<ShieldF>()!=null)&&hit.distance<2000f)
        {
            if(enemy.GetComponentInChildren<Turret>()!=null&&fireOnET==0)
                transform.LookAt(enemy.GetComponentInChildren<Turret>().transform);
            else 
                transform.LookAt(enemy.transform); 

            if(!ps.isPlaying)
                ps.Play();
        }
        else if(GameObject.FindWithTag("MissileF") != null &&Physics.Raycast(transform.position,GameObject.FindWithTag("MissileF").transform.position-transform.position,out hit,Mathf.Infinity)&&hit.transform.gameObject.CompareTag("MissileF")&&(enemy==null||(GameObject.FindWithTag("MissileF").transform.position-transform.position).magnitude<(enemy.transform.position-transform.position).magnitude))
        {
            transform.LookAt(GameObject.FindWithTag("MissileF").transform);  
            if(!ps.isPlaying)
                ps.Play();
        }
        else 
        {
            ps.Stop();
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(enemy!=null&&!enemy.CompareTag("Player"))
        {
            enemy=GameObject.FindWithTag("Player");
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
        // Debug.Log(other.name);
        if(other.GetComponent<MissileF>()!=null)
        {
            other.GetComponent<MissileF>().HP-=1;
        }
        if(other.GetComponent<Turret>()!=null&& (other.GetComponentInParent<Test>()!=null||other.GetComponentInParent<FriendlyVessel>()!=null))
        {
            // Debug.Log("Haha!!");
            if(other.GetComponent<Turret>().b!=null)
            {
                other.GetComponent<Turret>().HP-=1;

            }
        }
        else if(other.GetComponent<ShieldF>()!=null)
        {
            other.GetComponent<ShieldF>().HP-=1;
        }
        else if(other.GetComponentInParent<Test>() != null)
        {
            other.GetComponentInParent<Test>().beHit+=1;
        }
        else if(other.GetComponentInParent<FriendlyVessel>() != null)
        {
            other.GetComponentInParent<FriendlyVessel>().beHit+=1;
        }
        
    }
}
