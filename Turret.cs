using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Generic turret controller
/// </summary>
public class Turret : MonoBehaviour
{
    Vector3 position;
    Quaternion rotation;
    public Beam b;
    public BeamE bE;
    public int HP,iHP;
    // Start is called before the first frame update
    void Start()
    {
        position = transform.localPosition;
        rotation = transform .localRotation;
        float pxs = 0;
        if(GetComponentInChildren<Beam>()!=null)
        {
            b=GetComponentInChildren<Beam>();
            b.turret=gameObject;
            b.transform.parent=transform.parent.parent.parent;
            if(GetComponentInParent<FriendlyVessel>()!=null)
            {
                pxs = GetComponentInParent<FriendlyVessel>().xScale;
            }
            else
            {
                pxs = GetComponentInParent<Test>().xScale;
            }
        }
        else if(GetComponentInChildren<BeamE>()!=null)
        {
            bE=GetComponentInChildren<BeamE>();
            bE.turret=gameObject;
            bE.transform.parent=transform.parent.parent.parent;
            pxs = GetComponentInParent<Carrier>().xScale;
        }
        iHP=HP=400*(int)transform.localScale.x*(int)pxs;
        InvokeRepeating(nameof(IncrementHP),0f,5f);
    }
    void IncrementHP()
    {
        HP+=10;
        if(HP>iHP) HP = iHP;
    }
    void Update()
    {
        transform.localPosition = position;
        transform.localRotation = rotation;
        if(HP<1)
        {
            if(b!=null)
            {
                b.GetComponent<ParticleSystem>().Stop();
                Destroy(b);
                Test.yourTotalCasualty+=Random.Range(20, 50);
                if(GetComponentInParent<FriendlyVessel>()!=null)
                {
                    Instantiate(GetComponentInParent<FriendlyVessel>().fireball, transform.position, Quaternion.identity);
                }
                else
                {
                    Instantiate(GetComponentInParent<Test>().fireball, transform.position, Quaternion.identity);
                }
                Destroy(this);
            }
            else if(bE!=null)
            {
                bE.GetComponent<ParticleSystem>().Stop();
                Destroy(bE);
                Test.enemyTotalCasualty+=Random.Range(20, 50);
                Instantiate(GetComponentInParent<Carrier>().fireball, transform.position, Quaternion.identity);
                Destroy(this);
            }
        }
    }
}
