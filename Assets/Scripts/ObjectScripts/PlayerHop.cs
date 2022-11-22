using System;
using UnityEngine;

public class PlayerHop : MonoBehaviour
{
    private Rigidbody _rb;
    private Vector3 _startPos;
    private bool _hopping;
    private int _hopIndex;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
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
        if (Math.Abs(MusicPlayer.current.GetAudioSourceTime() - SingleButtonAction.Current.GetNextTimestamp(_hopIndex)) < 0.1f)
        {
            Hop();
        }
    }

    private void FixedUpdate()
    {
        _rb.AddForce(Vector3.down * (PlayerMovement.current.jumpingGravity * _rb.mass));
    }

    private void Hop()
    {
        if (_hopping == false)
        {
            _hopping = true;
            _rb.AddForce(transform.up * PlayerMovement.current.maxJumpForce, ForceMode.Impulse);
            _hopIndex++;
        }
    }
    
}
