using System;
using UnityEngine;

public class PlayerHop : MonoBehaviour
{
    private Rigidbody _rb;
    private Vector3 _startPos;
    private bool _hopping;
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        PlayerHopManager.Current.HopEvent += Hop;
        _startPos = transform.position;
        _hopping = false;
    }

    private void Update()
    {
        if (transform.position.y < _startPos.y)
        {
            transform.position = _startPos;
            _rb.velocity = new Vector3(0, 0, 0);
            _hopping = false;
        }
    }

    private void FixedUpdate()
    {
        _rb.AddForce(Vector3.down * ((PlayerMovement.Current.jumpingGravity + 5.5f) * _rb.mass));
    }
    
    private void Hop()
    {
        if (_hopping == false)
        {
            _hopping = true;
            _rb.AddForce(transform.up * PlayerMovement.Current.maxJumpForce, ForceMode.Impulse);
        }
    }
}