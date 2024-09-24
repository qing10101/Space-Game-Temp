using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The class for shield 
/// used by player and friends
/// </summary>
public class ShieldF : MonoBehaviour
{
    public int HP;
    int iHP;
    public GameObject p;
    // Start is called before the first frame update
    void Start()
    {
        iHP=HP;
        InvokeRepeating(nameof(Restore),1f,5f);
    }
    void Restore()
    {
        if(HP<iHP)
        {
            HP+=2;
        }
    }
    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles=p.transform.eulerAngles;
        transform.position=p.transform.position;
        
        if(HP<=0)
        {
            CancelInvoke(nameof(Restore));
            Destroy(gameObject);
        }

    }
    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponentInParent<Carrier>()!=null&&collision.gameObject.GetComponentInParent<Carrier>().enabled)
        {
            Instantiate(collision.gameObject.GetComponentInParent<Carrier>().fireball,collision.transform.position,Quaternion.identity);
            collision.gameObject.GetComponentInParent<Carrier>().beHit+=3001;
            HP-=3001;
        }
        else if(collision.gameObject.GetComponent<Shield>()!=null)
        {
            collision.gameObject.GetComponent<Shield>().HP-=3001;
            HP-=3001;
        }
    }
}
