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
    }

    private void Update()
    {
        transform.rotation = Quaternion.LookRotation(_rb.velocity);
    }

    private void OnCollisionEnter(Collision other)
    {
        _rb.isKinematic = true;
        _bc.isTrigger = true;
        
    }
}
