using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using StarterAssets;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class ShootingSystem : MonoBehaviour
{
    //Debug & Tweak
    [SerializeField] private float _arrowForce = 200.0f;

    //Cameras
    private Camera _mainCamera;
    
    //SFX
    [SerializeField] private AudioSource _bow_loading;
    [SerializeField] private AudioSource _bow_release;
    
    //Components & Animations
    private StarterAssetsInputs _input;
    private Animator _animator;
    private GameObject _currentArrow;
    [SerializeField] private Rig _weaponAimRigLayer;
    [SerializeField] private Rig _rightIKRigLayer;
    [SerializeField] private Transform _quiverArrow;
    private float _aimDuration = 0.2f;

    //Aiming & Shooting
    [SerializeField] private GameObject _crosshair;
    private float _turnSpeed = 15f;
    
    private Ray _ray;
    private RaycastHit _rayHitInfo;
    private Quaternion _arrowRotation;
    [SerializeField] private Transform _raycastOrigin;
    [SerializeField] private Transform _raycastDestination;
    
    [SerializeField] private GameObject _arrowObject;
    
    //Impact
    [SerializeField] private ParticleSystem _impactEffect;

    void Start()
    {
        _input = GetComponent<StarterAssetsInputs>();
        _mainCamera = Camera.main;
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        AimAndShoot();
    }

    private void FixedUpdate()
    {
        float YAxisCamera = _mainCamera.transform.rotation.eulerAngles.y;
         transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, YAxisCamera, 0),
             _turnSpeed * Time.fixedDeltaTime);
    }

    private void PickArrow()
    {
        _currentArrow = Instantiate(_arrowObject, _quiverArrow.position, _quiverArrow.rotation) as GameObject;
        _quiverArrow.gameObject.SetActive(true);
    }

    private void DisableArrow()
    {
        _quiverArrow.gameObject.SetActive(false);
    }

    private void PullString()
    {
        _rightIKRigLayer.weight += Time.deltaTime / _aimDuration;
    }
    
    //Animation Event
    private void ReleaseString()
    {
        _rightIKRigLayer.weight -= Time.deltaTime / _aimDuration;
        
        _ray.origin = _raycastOrigin.position;
        _ray.direction = _raycastDestination.position - _raycastOrigin.position;
            
        if (Physics.Raycast(_ray, out RaycastHit _rayHitInfo))
        {
                Debug.DrawLine(_ray.origin, _rayHitInfo.point, Color.red, 1.0f);
                _impactEffect.transform.position = _rayHitInfo.point;
                _impactEffect.transform.forward = _rayHitInfo.normal;
                _impactEffect.Emit(1);
                    
                if (_rayHitInfo.collider.gameObject.layer == LayerMask.NameToLayer("Targets"))
                {
                    // Play the "DummyHit" animation on the item
                    Animator animator = _rayHitInfo.collider.gameObject.GetComponent<Animator>();
                    if (animator != null)
                    {
                        animator.SetTrigger("DummyHit");
                    }
                        
                    //Deactivate the target's collider to avoid hitting it again
                    _rayHitInfo.collider.enabled = false;
                }
                    
                Vector3 normal = _rayHitInfo.normal;
                _arrowRotation = Quaternion.FromToRotation(Vector3.up, normal);
        }
        
        GameObject arrow = Instantiate(_arrowObject, _raycastOrigin.position, _arrowRotation);
        arrow.GetComponent<Rigidbody>().AddForce(_ray.direction * _arrowForce, ForceMode.VelocityChange);
        _bow_release.Play();
    }
    private void AimAndShoot()
    {
        if (_input.isAiming)
        {
            _crosshair.SetActive(true);
            _weaponAimRigLayer.weight += Time.deltaTime / _aimDuration;

            _animator.SetBool("Aiming", _input.isAiming);
            _animator.SetBool("Shooting", _input.isShooting);
            _bow_loading.Play();
        }
        else
        {
            _weaponAimRigLayer.weight -= Time.deltaTime / _aimDuration;

            _animator.SetBool("Aiming", false);
            _animator.SetBool("Shooting", false);
            _crosshair.SetActive(false);
            DisableArrow();
        }
    }
    
}