using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// The class for shield 
/// used by enemies
/// </summary>
public class Shield : MonoBehaviour
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
        if(collision.gameObject.GetComponentInParent<FriendlyVessel>()!=null&&collision.gameObject.GetComponentInParent<FriendlyVessel>().enabled)
        {
            collision.gameObject.GetComponentInParent<FriendlyVessel>().beHit+=3001;
            Instantiate(collision.gameObject.GetComponentInParent<FriendlyVessel>().fireball,collision.transform.position,Quaternion.identity);
            HP-=3001;
        }
        else if(collision.gameObject.GetComponentInParent<Test>()!=null&&collision.gameObject.GetComponentInParent<Test>().enabled)
        {
            collision.gameObject.GetComponentInParent<Test>().beHit+=3001;
            Instantiate(collision.gameObject.GetComponentInParent<Test>().fireball,collision.transform.position,Quaternion.identity);
            HP-=3001;
        }
        else if(collision.gameObject.GetComponent<ShieldF>()!=null)
        {
            collision.gameObject.GetComponent<ShieldF>().HP-=3001;
            HP-=3001;
        }
    }

}
