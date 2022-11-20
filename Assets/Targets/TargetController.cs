using UnityEngine;

public class TargetController : MonoBehaviour
{

    [SerializeField]
    private int _pointsValue = 10;
    [SerializeField]
    private float _timeAdder = 5;
    [SerializeField]
    private float _outOfPlaytimeReduction = 1;
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
    [SerializeField]
    private GameObject _rewardObject;

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
            GameManager.Instance.ReduceTime(_outOfPlaytimeReduction);
            return true;
        }
        return false;
    }

    //@TODO Sequence for destroying target
    public void TargetHit()
    {
        RewardPlayer();
        if (_ammoGain > 0)
        {
            _playerController.AddAmmo(_weapon, _ammoGain);
        }
        SpawnPointController spc = GetComponentInParent<SpawnPointController>();
        spc.spawnedTargets.Remove(this.gameObject);
        Destroy(this.gameObject);
    }

    private void RewardPlayer()
    {
        GameManager.Instance.AddScore(_pointsValue);
        GameManager.Instance.AddTime(_timeAdder);

        if (_pointsValue != 0)
        {
            var rewardPointsObj = Instantiate(_rewardObject);
            RewardController rewardPoints = rewardPointsObj.GetComponent<RewardController>();
            rewardPoints.transform.position = this.transform.position;
            rewardPoints.value = _pointsValue;
            rewardPoints.rewardType = RewardController.RewardType.Score;
        }
        if (_timeAdder != 0)
        {
            var rewardPointsObj = Instantiate(_rewardObject);
            RewardController rewardPoints = rewardPointsObj.GetComponent<RewardController>();
            rewardPoints.transform.position = this.transform.position;
            rewardPoints.value = _timeAdder;
            rewardPoints.rewardType = RewardController.RewardType.Time;
        }
        if (_ammoGain != 0)
        {
            var rewardPointsObj = Instantiate(_rewardObject);
            RewardController rewardPoints = rewardPointsObj.GetComponent<RewardController>();
            rewardPoints.transform.position = this.transform.position;
            rewardPoints.value = _ammoGain;
            rewardPoints.rewardType = RewardController.RewardType.SecondaryWeapon;
        }
    }
}
