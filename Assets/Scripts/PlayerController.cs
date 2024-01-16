using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _gravityScale = 15f;
    [SerializeField] private float _jumpForce = 200f;
    
    private CharacterController _characterController;
    private Animator _animator;
    
    private Vector2 _inputMovement;
    private bool _inputJump;
    
    // Start is called before the first frame update
    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
  
    }

    private void FixedUpdate()
    {
        _animator.SetFloat("ForwardAxis", _inputMovement.y);
        _animator.SetFloat("StrafeAxis", _inputMovement.x);
        _animator.SetFloat("MoveSpeed", _inputMovement.magnitude * _moveSpeed);
    }

    void OnJump(InputValue value)
    {
        _inputJump = value.Get<float>() > Mathf.Epsilon;
    }
    
    //if Behavior is set to "Send Messages"
    void OnMove(InputValue value)
    {
        //Debug.Log("OnMove called : " + value.Get<Vector2>());
        _inputMovement = value.Get<Vector2>();
    }

    //if Behavior is set to "Invoke Unity Events"
    // public void OnMoveEvent(InputAction.CallbackContext context)
    // {
    //     var value = context.ReadValue<Vector2>();
    //     Debug.Log("OnMove called : " + value);
    // }


}
