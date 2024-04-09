using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EfeitoRagdol : MonoBehaviour
{
    private Rigidbody myRigid;
    private List<Collider> colls = new List<Collider>();
    private List<Rigidbody> rigs = new List<Rigidbody>();

    // Start is called before the first frame update
    void Start()
    {
        myRigid = GetComponent<Rigidbody>();    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init()
    {
        Rigidbody[] rigids = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rigid in rigids)
        {
            if (rigid == myRigid)
            {
                continue;
            }

            rigs.Add(rigid);
            rigid.isKinematic = true;

            Collider coll = rigid.gameObject.GetComponent<Collider>();
            coll.enabled = false;
            colls.Add(coll);
        }
    }
}
