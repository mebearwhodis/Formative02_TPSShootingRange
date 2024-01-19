using System;
using UnityEngine;

public class Arrow : MonoBehaviour
{
     private Rigidbody _rb;
     private BoxCollider _bc;

     
     // Start is called before the first frame update
     void Start()
     {
         _rb = GetComponent<Rigidbody>();
         _bc = GetComponent<BoxCollider>();
         Destroy(gameObject,5);
     }
    
     private void Update()
     {
         transform.rotation = Quaternion.LookRotation(_rb.velocity);
     }

     private void OnCollisionEnter(Collision collision)
     {
         Debug.Log("CollidedEnter");
         //transform.position = collision.contacts[0].point;
         //transform.SetParent(collision.transform);
         
         _rb.isKinematic = true;
         //_rb.velocity = Vector3.zero;
         //_rb.angularVelocity = Vector3.zero;
         
         if (collision.gameObject.layer == LayerMask.NameToLayer("Targets"))
         {
             Debug.Log("With Dummy");
             Animator animator = collision.gameObject.GetComponent<Animator>();
             if (animator != null)
             {
                 animator.SetTrigger("DummyHit");
             }
             // Deactivate the collider of the target
             collision.gameObject.GetComponent<Collider>().enabled = false;
         }

       
     }
}