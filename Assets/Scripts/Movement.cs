using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Composites;
using UnityEngine.Rendering;
using UnityEngine.SocialPlatforms;
using UnityEngine.UIElements;
using DG.Tweening;


public class Movement : MonoBehaviour
{
    [SerializeField] private Rigidbody _playerRB;
    public float _speed, _sensitivity, _maxForce;
    [SerializeField] private Vector2 _move, _look;
    [SerializeField] private float _lookRotation, _looksides;
    [SerializeField] private GameObject _cameraFocus;
    [SerializeField] private Transform _floatinCat;
    [SerializeField] private Animator _caminhada;
    [SerializeField]
    private float floatDistance = 0.5f;
    [SerializeField]
    private float duration = 2f;

    // Start is called before the first frame update
    void Start()
    {
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;
        _floatinCat.DOLocalMoveY(_floatinCat.localPosition.y + floatDistance, duration).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
    }

    private void FixedUpdate()
    {
        Vector3 currentVelocity = _playerRB.velocity;
        Vector3 targetVelocity = new Vector3(_move.x, 0, _move.y);
        targetVelocity *= _speed;

        targetVelocity = transform.TransformDirection(targetVelocity);

        Vector3 velocityChange = (targetVelocity - currentVelocity);

        //_playerRB.velocity = velocityChange;

        Vector3.ClampMagnitude(velocityChange, _maxForce);


        _playerRB.AddForce(velocityChange, ForceMode.VelocityChange);

        _looksides += (_look.x * _sensitivity);
        Quaternion rotate = Quaternion.Euler(0, _looksides, 0);
        _playerRB.MoveRotation(rotate);

    }

    // Update is called once per frame
    void LateUpdate()
    {
        //transform.Rotate(_look.x * _sensitivity * Vector3.up);

        _lookRotation += (-_look.y * _sensitivity);
        _lookRotation = Mathf.Clamp(_lookRotation, -80, 80);
        _cameraFocus.transform.eulerAngles = new Vector3(_lookRotation, _cameraFocus.transform.eulerAngles.y, _cameraFocus.transform.eulerAngles.z);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _move = context.ReadValue<Vector2>();
        if(_move != Vector2.zero)
        {
            _caminhada.SetBool("Idle", false);
        }
        else
        {
            _caminhada.SetBool("Idle", true);
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        _look = context.ReadValue<Vector2>();
    }
}

