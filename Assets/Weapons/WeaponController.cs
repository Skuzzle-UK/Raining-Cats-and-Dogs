using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public float power = 5f;
    public float startHeightAboveLauncher = 2f;
    [SerializeField]
    private float _powerMultiplier = 1f;
    [SerializeField]
    private float _powerFade = 2f;
    [SerializeField]
    private float _fallRate = 1f;
    [SerializeField]
    private float _fallRateAdder = 1.5f;
    [SerializeField]
    private float _shrinkDivider = 12f;
    private Vector2 _move;
    private Rigidbody2D _rbody;
    private bool _hitFloor = false;
    private bool _hitTarget = false;

    private void Awake()
    {
        power = power * _powerMultiplier;
        _move = new Vector2(0, power);
        _rbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (!_hitFloor && !_hitTarget)
        {
            MoveY();
            Fall();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (this.transform.position.z > collision.transform.position.z - 1 && this.transform.position.z < collision.transform.position.z + 1 && collision.transform.tag == "Target")
        {
            HitTarget(collision);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (this.transform.position.z > collision.transform.position.z - 1 && this.transform.position.z < collision.transform.position.z + 1 && collision.transform.tag == "Target")
        {
            HitTarget(collision);
        }
    }

    private void Fall()
    {
        _fallRate += _fallRateAdder * Time.fixedDeltaTime;
        float fallRate = _fallRate * Time.deltaTime;
        if (transform.position.z + fallRate <= 0)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + fallRate);
            transform.localScale = new Vector3(transform.localScale.x - fallRate / _shrinkDivider, transform.localScale.y - fallRate / _shrinkDivider, transform.localScale.z);
        }
        else
        {
            if (!_hitFloor)
            {
                HitFloor();
            }
        }
    }

    //@TODO Sequence for hitting target
    private void HitTarget(Collider2D collision)
    {
        _hitTarget = true;
        TargetController target = collision.gameObject.transform.GetComponent<TargetController>();
        target.TargetHit();
        Destroy(this.gameObject);
    }

    //@TODO Sequence for missing target and hitting the floor
    private void HitFloor()
    {
        _hitFloor = true;
        Destroy(this.gameObject);
    }

    private void MoveY()
    {
        if (power > 0)
        {
            power -= _powerFade * Time.fixedDeltaTime;
            _move.y = power;
            _move.x = 0;
        }
        else
        {
            _move.y = 0;
            _move.x = 0;
        }
        _rbody.velocity = _move;
    }
}
