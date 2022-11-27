using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceDirectHitInAir : MonoBehaviour
{
    [SerializeField]
    private float _directHitRadius = 2f;
    private CircleCollider2D _collider;
    private float _originalSize;

    private void Awake()
    {
        _collider = GetComponent<CircleCollider2D>();
        _originalSize = _collider.radius;
        _collider.radius = _directHitRadius;
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.z > -1f)
        {
            _collider.radius = _originalSize;
        }
    }
}
