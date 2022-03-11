using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    //Variables of behavoir 
    public float _movementSpeed;
    public float _movementTime;
    public float _rotationAmount;

    public Vector3 _newPosition;
    public Quaternion _newRotation;


    //Zoom & Zoom Out
    public Transform _cameraTransform;
    public Vector3 _zoomAmount;
    public Vector3 _newZoom;

    void Start()
    {
        _newPosition = transform.position;
        _newRotation = transform.rotation;
        _newZoom = _cameraTransform.localPosition;
    }

    void FixedUpdate()
    {
        HandleMovementInput();
    }

    //Movements Inputs || *Cambiar inputs por commands*
    void HandleMovementInput()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            _newPosition += (transform.forward * _movementSpeed);
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            _newPosition += (transform.forward * -_movementSpeed);
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            _newPosition += (transform.right * _movementSpeed);
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            _newPosition += (transform.right * -_movementSpeed);
        }
        if (Input.GetKey(KeyCode.Q))
        {
            _newRotation *= Quaternion.Euler(Vector3.up * _rotationAmount);
        }
        if (Input.GetKey(KeyCode.E))
        {
            _newRotation *= Quaternion.Euler(Vector3.up * -_rotationAmount);
        }

        if(Input.GetAxis("Mouse ScrollWheel") > 0f || Input.GetKey(KeyCode.R))
        {
            _newZoom += _zoomAmount;            
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0f || Input.GetKey(KeyCode.F))
        {
            _newZoom -= _zoomAmount;
        }

        transform.position = Vector3.Lerp(transform.position, _newPosition, Time.deltaTime * _movementTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, _newRotation, Time.deltaTime * _movementTime);
        _cameraTransform.localPosition = Vector3.Lerp(_cameraTransform.localPosition, _newZoom, Time.deltaTime * _movementTime);

    }

}
