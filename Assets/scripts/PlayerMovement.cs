using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] public float playerSpeed;
    private SpriteRenderer _renderer;
    private Animator _animator;
    void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }
    void FixedUpdate()
    {
        float x = Input.GetAxis("Horizontal") * playerSpeed * Time.deltaTime;
        float y = Input.GetAxis("Vertical") * playerSpeed * Time.deltaTime;
        var step = Math.Abs(x) >= 0.01 * playerSpeed ? new Vector3(x, 0, 0) : new Vector3(0, y, 0);
        gameObject.transform.position += step;
        if (Math.Abs(x) > 0 || Math.Abs(y) > 0)
        {
            _animator.SetBool("moving",true);
            float z = step.x < 0 ? 180 :0;
            z += step.y > 0 ? 90 : step.y < 0 ? -90 : 0;
            _renderer.transform.rotation=Quaternion.Euler(0,0,z);
        }
        else
        {
            _animator.SetBool("moving",false);
        }
    }
}
