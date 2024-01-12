using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class ShootingSystem : MonoBehaviour
{
    [SerializeField] private GameObject _shootingTarget;
    [SerializeField] private GameObject _shootingOrigin;
    [SerializeField] private float _maxShootingDistance = 300f;
    [SerializeField] private GameObject _impact;
    
    private StarterAssetsInputs _input;
    private Camera _mainCamera;
    void Start()
    {
        _input = GetComponent<StarterAssetsInputs>();
        _mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (_input.isAiming)
        {
            _shootingOrigin.SetActive(true);
            _shootingTarget.SetActive(true);
            
            Vector3 shootPoint = _mainCamera.ScreenToViewportPoint(new Vector3(0.5f, 0.5f, _maxShootingDistance));

            //PewPew
            Ray ray = new Ray(_shootingOrigin.transform.position, shootPoint - _shootingOrigin.transform.position);
            
            Debug.DrawRay(ray.origin, ray.direction * _maxShootingDistance, Color.magenta, 0.5f);
            if (Physics.Raycast(ray, out RaycastHit hitInfo, _maxShootingDistance))
            {
                _shootingTarget.transform.position = hitInfo.point;
                //hitInfo.collider.gameObject.CompareTag("Target");
                if (_input.isShot)
                    Instantiate(_impact, hitInfo.point, Quaternion.identity);
            }
        }
        else
        {
            _shootingTarget.SetActive(false);
            _shootingOrigin.SetActive(false);
        }
    }
}
