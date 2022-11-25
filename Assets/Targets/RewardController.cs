using TMPro;
using UnityEngine;

public class RewardController : MonoBehaviour
{
    public enum RewardType
    {
        Score,
        Time,
        PrimaryWeapon,
        SecondaryWeapon
    }

    public float value;
    public RewardType rewardType;
    [SerializeField]
    private float _speed = 5;
    [SerializeField]
    private Color _scoreColour;
    [SerializeField]
    private Color _timeColour;
    [SerializeField]
    private Color _primaryWeaponColour;
    [SerializeField]
    private Color _secondaryWeaponColour;
    [SerializeField]
    private float _fadeDelay;
    [SerializeField]
    private float _fadeRate;
    [SerializeField]
    private Vector2 ScorePosition;
    [SerializeField]
    private Vector2 TimePosition;
    [SerializeField]
    private Vector2 PrimaryPosition;
    [SerializeField]
    private Vector2 SecondaryPosition;

    private Rigidbody2D _rbody;
    private TextMeshPro _tmp;

    private void Awake()
    {
        _tmp = GetComponent<TextMeshPro>();
    }

    private void Start()
    {
        SetValue();
        SetColour();
        SetPosition();
    }

    private void Update()
    {
        SetPosition();
        if (_fadeDelay > 0)
        {
            _fadeDelay -= Time.deltaTime;
            return;
        }
        _tmp.color = new Color(_tmp.color.r, _tmp.color.g, _tmp.color.b, _tmp.color.a - (_fadeRate * Time.deltaTime));
        if (_tmp.color.a <= 0.05)
        {
            Destroy(this.gameObject);
        }
    }

    private void SetValue()
    {
        _tmp.text = value.ToString();
    }

    private void SetPosition()
    {
        switch (rewardType)
        {
            case RewardType.Score:
                transform.position = Vector2.Lerp(transform.position, ScorePosition, Time.deltaTime * _speed);
                break;
            case RewardType.Time:
                transform.position = Vector2.Lerp(transform.position, TimePosition, Time.deltaTime * _speed);
                break;
            case RewardType.PrimaryWeapon:
                transform.position = Vector2.Lerp(transform.position, PrimaryPosition, Time.deltaTime * _speed);
                break;
            case RewardType.SecondaryWeapon:
                transform.position = Vector2.Lerp(transform.position, SecondaryPosition, Time.deltaTime * _speed);
                break;
        }
    }

    private void SetColour()
    {
        switch (rewardType)
        {
            case RewardType.Score:
                _tmp.color = _scoreColour;
                break;
            case RewardType.Time:
                _tmp.color = _timeColour;
                break;
            case RewardType.PrimaryWeapon:
                _tmp.color = _primaryWeaponColour;
                break;
            case RewardType.SecondaryWeapon:
                _tmp.color = _secondaryWeaponColour;
                break;
        }
    }
}
