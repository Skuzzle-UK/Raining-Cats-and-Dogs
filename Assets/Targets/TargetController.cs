using UnityEngine;

public class TargetController : MonoBehaviour
{

    [SerializeField]
    private int _pointsValue = 10;
    [SerializeField]
    private float _timeAdder = 5;
    [SerializeField]
    private AmmoSource _weapon = AmmoSource.Secondary;
    [SerializeField]
    private int _ammoGain = 1;
    [SerializeField]
    public Vector2 velocity;
    [SerializeField]
    private float _minX = -13f;
    [SerializeField]
    private float _maxX = 13f;
    [SerializeField]
    private float _minY = -8f;
    [SerializeField]
    private float _maxY = 8f;

    private Rigidbody2D _rbody;
    private PlayerController _playerController;

    private void Awake()
    {
        _playerController = FindObjectOfType<PlayerController>();
        _rbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        _rbody.velocity = velocity;
    }

    private void Update()
    {
        _rbody.velocity = velocity;
    }

    private void FixedUpdate()
    {
        if (OutOfPlay())
        {
            SpawnPointController spc = GetComponentInParent<SpawnPointController>();
            spc.spawnedTargets.Remove(this.gameObject);
            Destroy(this.gameObject);
        }
    }

    private bool OutOfPlay()
    {
        if (this.transform.position.x < _minX || this.transform.position.x > _maxX || this.transform.position.y < _minY || this.transform.position.y > _maxY)
        {
            return true;
        }
        return false;
    }

    //@TODO Sequence for destroying target
    public void TargetHit()
    {
        GameManager.Instance.AddScore(_pointsValue);
        GameManager.Instance.AddTime(_timeAdder);
        if (_ammoGain > 0)
        {
            _playerController.AddAmmo(_weapon, _ammoGain);
        }
        SpawnPointController spc = GetComponentInParent<SpawnPointController>();
        spc.spawnedTargets.Remove(this.gameObject);
        Destroy(this.gameObject);
    }
}
