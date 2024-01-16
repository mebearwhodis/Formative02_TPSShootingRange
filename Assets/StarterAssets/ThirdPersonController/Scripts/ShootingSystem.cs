using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using StarterAssets;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class ShootingSystem : MonoBehaviour
{
    [SerializeField] private GameObject _shootingTarget;
    [SerializeField] private GameObject _shootingOrigin;
    [SerializeField] private float _maxShootingDistance = 300f;
    [SerializeField] private GameObject _impact;
    [SerializeField] private GameObject _crosshairPrefab;
    [SerializeField] private LayerMask _aimLayers;
    [SerializeField] private Transform _spine;


    [SerializeField] private CinemachineFreeLook _baseCamera;
    [SerializeField] private CinemachineFreeLook _aimCamera;
    private readonly int _activePriority = 100;
    private readonly int _inactivePriority = 10;

    private StarterAssetsInputs _input;
    private Animator _animator;
    private Camera _mainCamera;
    private GameObject _crosshair;

    private Ray _ray;
    private Vector3 _spineOffset;
    private float _turnSpeed = 15f;

    [SerializeField] private Rig _weaponAimRigLayer;
    [SerializeField] private MultiParentConstraint _pullStringConstraint;
    private float _aimDuration = 0.2f;

    void Start()
    {
        _input = GetComponent<StarterAssetsInputs>();
        _mainCamera = Camera.main;
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Aim();
        
        // _shootingOrigin.SetActive(true);
        // _shootingTarget.SetActive(true);

        //Vector3 shootPoint = _mainCamera.ScreenToViewportPoint(new Vector3(0.5f, 0.5f, _maxShootingDistance));

        //Ray ray = new Ray(_shootingOrigin.transform.position, shootPoint - _shootingOrigin.transform.position);

        // Debug.DrawRay(ray.origin, ray.direction * _maxShootingDistance, Color.magenta, 0.5f);
        // if (Physics.Raycast(ray, out RaycastHit hitInfo, _maxShootingDistance))
        // {
        //     _shootingTarget.transform.position = hitInfo.point;
        //     //hitInfo.collider.gameObject.CompareTag("Target");
        //     if (_input.isShot)
        //         Instantiate(_impact, hitInfo.point, Quaternion.identity);
        // }
    }

    private void FixedUpdate()
    {
        float YAxisCamera = _mainCamera.transform.rotation.eulerAngles.y;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, YAxisCamera, 0), _turnSpeed * Time.fixedDeltaTime);
    }

    private void LateUpdate()
    {
        if (_input.isAiming)
            RotateCharacter();
    }

    private void DisplayCrosshair(Vector3 crosshairPos)
    {
        if (!_crosshair)
            _crosshair = Instantiate(_crosshairPrefab) as GameObject;

        _crosshair.transform.position = crosshairPos;
        _crosshair.transform.LookAt(Camera.main.transform);
    }

    private void DestroyCrosshair()
    {
        if (_crosshair)
            Destroy(_crosshair);
    }
    
    // ReSharper disable Unity.PerformanceAnalysis
    private void Aim()
    {
        //Switch to/from AimCamera
        _aimCamera.Priority = _input.isAiming ? _activePriority : _inactivePriority;
        _baseCamera.Priority = _input.isAiming ? _inactivePriority : _activePriority;
        
        if (_input.isAiming)
        {
            _weaponAimRigLayer.weight += Time.deltaTime / _aimDuration;
            _pullStringConstraint.weight = _animator.GetCurrentAnimatorStateInfo(1).IsName("TakeAim") ? 1 : 0;
            
            _animator.SetBool("Aiming", true);
            Vector3 cameraPosition = _mainCamera.transform.position;
            Vector3 cameraDirection = _mainCamera.transform.forward;

            _ray = new Ray(cameraPosition, cameraDirection);
            if (Physics.Raycast(_ray, out RaycastHit hitInfo, _maxShootingDistance, _aimLayers))
            {
                Debug.DrawRay(_ray.origin, hitInfo.point, Color.green);
                DisplayCrosshair(hitInfo.point);
            }
            else
            {
                DestroyCrosshair();
            }
        }
        else
        {
            _weaponAimRigLayer.weight -= Time.deltaTime / _aimDuration;
            
            _animator.SetBool("Aiming", false);
            _shootingTarget.SetActive(false);
            _shootingOrigin.SetActive(false);
            DestroyCrosshair();
        }
    }

    void RotateCharacter()
    {
        _spine.LookAt(_ray.GetPoint(50));
        _spine.Rotate(_spineOffset);
    }
}