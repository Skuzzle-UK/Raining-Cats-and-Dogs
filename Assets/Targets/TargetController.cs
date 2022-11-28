using System.Collections;
using UnityEngine;
using UnityEngine.SocialPlatforms;

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
    [SerializeField]
    private bool _eraticMovement = false;
    [SerializeField]
    private float _eraticDampner = 2.2f;
    [SerializeField]
    private bool _faceTravelDirection = false;
    [SerializeField]
    private float _rotateDampening = 10f;
    [SerializeField]
    private bool _specialTarget;
    [SerializeField]
    private AudioClip _specialTargetHitAudio;
    private AudioSource _audioSource;

    private Rigidbody2D _rbody;
    private PlayerController _playerController;
    private Quaternion desiredRotQ;
    private Vector2 newVelocity = new Vector2();

    private CircleCollider2D _collider;
    private CapsuleCollider2D _collider2;

    private void Awake()
    {
        try
        {
            _collider = GetComponent<CircleCollider2D>();
        }
        catch
        {
            try
            {
                _collider2 = GetComponent<CapsuleCollider2D>();
            }
            catch { }
        }
        _audioSource = GetComponent<AudioSource>();
        _playerController = FindObjectOfType<PlayerController>();
        _rbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        newVelocity = velocity;
        _rbody.velocity = velocity;
        StartCoroutine(GetNewVelocity());
    }

    IEnumerator GetNewVelocity()
    {
        yield return new WaitForSeconds(0.2f);
        if (_eraticMovement)
        {
            newVelocity.x = Random.Range(-velocity.x + -velocity.y, velocity.x + velocity.y) / _eraticDampner;
            newVelocity.y = Random.Range(-velocity.x + -velocity.y, velocity.x + velocity.y) / _eraticDampner;
            newVelocity += _rbody.velocity / (_eraticDampner / 2.5f);

        }
        else
        {
            newVelocity = velocity;
        }
        StartCoroutine(GetNewVelocity());
    }

    private void Update()
    {
        _rbody.velocity = newVelocity;
        if (_faceTravelDirection)
        {
            Quaternion toRotation = Quaternion.LookRotation(Vector3.forward, _rbody.velocity.normalized);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, _rotateDampening * Time.deltaTime);
        }
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

    public void TargetHit()
    {
        try
        {
            _collider.enabled = false;
        }
        catch
        {
            try
            {
                _collider2.enabled = false;
            }
            catch { }
        }

        _audioSource.Stop();
        _audioSource.PlayOneShot(_specialTargetHitAudio);
        
        RewardPlayer();
        
        if (_ammoGain > 0)
        {
            _playerController.AddAmmo(_weapon, _ammoGain);
        }
        
        SpawnPointController spc = GetComponentInParent<SpawnPointController>();
        
        spc.spawnedTargets.Remove(this.gameObject);
        
        var children = gameObject.GetComponentInChildren<Transform>();
        foreach (Transform child in children)
        {
            child.gameObject.active = false;
        }
        
        StartCoroutine(DestroyTarget());
    }

    IEnumerator DestroyTarget()
    {
        while (_audioSource.isPlaying)
        {
            yield return new WaitForEndOfFrame();
        }
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
