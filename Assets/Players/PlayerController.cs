using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float _movementSpeed = 10;
    [SerializeField]
    private float _fireInitialPower = 3;
    [SerializeField]
    private float _firePowerIncSpeed = 1;
    [SerializeField]
    private float _maxFirePower = 6;
    [SerializeField]
    private GameObject _primaryWeapon;
    [SerializeField]
    private GameObject _secondaryWeapon;
    [SerializeField]
    private int _primaryAmmo = -1;
    [SerializeField]
    private int _secondaryAmmo = 3;
    [SerializeField]
    private float _xMinLimit = -10;
    [SerializeField]
    private float _xMaxLimit = 10;
    [SerializeField]
    private float _yMinLimit = -4;
    [SerializeField]
    private float _yMaxLimit = 4;
    private Rigidbody2D _rbody;
    private Vector2 _moveInput;
    private PlayerInput _playerInput;
    private bool _firePrimary;
    private bool _fireSecondary;
    private float _firePrimaryPower;
    private float _fireSecondaryPower;
    private WeaponChargeUiController _primaryWeaponChargeUI;
    private WeaponChargeUiController _secondaryWeaponChargeUI;
    private Animator _animator;


    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _playerInput = GetComponent<PlayerInput>();
        _rbody = GetComponent<Rigidbody2D>();
        _playerInput.SwitchCurrentActionMap("UpTop");
    }

    private void Start()
    {
        _primaryWeaponChargeUI = GameObject.Find("UI/Weapons/PrimaryCharge").GetComponent<WeaponChargeUiController>();
        _secondaryWeaponChargeUI = GameObject.Find("UI/Weapons/SecondaryCharge").GetComponent<WeaponChargeUiController>();
        _primaryWeaponChargeUI.SetRange(_fireInitialPower, _maxFirePower);
        _secondaryWeaponChargeUI.SetRange(_fireInitialPower, _maxFirePower);
    }

    private void FixedUpdate()
    {
        _rbody.velocity = _moveInput * _movementSpeed;
    }

    private void Update()
    {
        CheckPositionLimits();
        WeaponsCheck(ref _primaryAmmo, ref _firePrimary, ref _firePrimaryPower, ref _primaryWeapon, ref _primaryWeaponChargeUI);
        WeaponsCheck(ref _secondaryAmmo, ref _fireSecondary, ref _fireSecondaryPower, ref _secondaryWeapon, ref _secondaryWeaponChargeUI);
    }

    void OnMovement(InputValue value)
    {
        _moveInput = value.Get<Vector2>();
    }

    void OnFirePrimary(InputValue value)
    {
        if (value.Get<float>() > 0.05f)
        {
            if (_primaryAmmo != 0)
            {
                _animator.Play("PlayerCharge");
            }
            _firePrimary = true;
        }
        else
        {
            _firePrimary = false;
        }
    }

    void OnFireSecondary(InputValue value)
    {
        if (value.Get<float>() > 0.05f)
        {
            if (_secondaryAmmo != 0)
            {
                _animator.Play("PlayerCharge");
            }
            _fireSecondary = true;
        }
        else
        {
            _fireSecondary = false;
        }
    }

    //Checks that player is within positional limits and corrects if not
    private void CheckPositionLimits()
    {
        if (this.transform.position.x < _xMinLimit)
        {
            transform.position = new Vector3(_xMinLimit, this.transform.position.y, this.transform.position.z);
        }
        if (this.transform.position.x > _xMaxLimit)
        {
            transform.position = new Vector3(_xMaxLimit, this.transform.position.y, this.transform.position.z);
        }
        if (this.transform.position.y < _yMinLimit)
        {
            transform.position = new Vector3(this.transform.position.x, _yMinLimit, this.transform.position.z);
        }
        if (this.transform.position.y > _yMaxLimit)
        {
            transform.position = new Vector3(this.transform.position.x, _yMaxLimit, this.transform.position.z);
        }
    }

    //Check for if weapon(projectile) is being charged or released
    private void WeaponsCheck(ref int ammo, ref bool weaponIsFiring, ref float power, ref GameObject weapon, ref WeaponChargeUiController ui)
    {
        if (ammo > 0 || ammo == -1)
        {
            if (weaponIsFiring)
            {
                if (power <= _fireInitialPower)
                {
                    power = _fireInitialPower;
                }
                power += Time.deltaTime * _firePowerIncSpeed;
                if (power > _maxFirePower)
                {
                    power = _maxFirePower;
                }
                ui.SetValue(power);
            }
            else
            {
                if (power > _fireInitialPower)
                {
                    ReleaseWeapon(weapon, power);
                    power = _fireInitialPower;
                    if (ammo > 0)
                    {
                        ammo -= 1;
                    }
                    ui.SetValue(_fireInitialPower);
                }
            }
        }
        else
        {
            OutOfAmmo(ref weapon);
        }
    }


    //@TODO Out of ammo on secondary weapon sequence
    private void OutOfAmmo(ref GameObject weapon)
    {
    }

    public void AddAmmo(AmmoSource weapon, int ammo)
    {
        switch (weapon)
        {
            case AmmoSource.Primary:
                _primaryAmmo += ammo;
                break;
            case AmmoSource.Secondary:
                _secondaryAmmo += ammo;
                break;
        }
    }

    public int? PrimaryAmmo
    {
        get { return _primaryAmmo; }
    }

    public int? SecondaryAmmo
    {
        get { return _secondaryAmmo; }
    }

    private void ReleaseWeapon(GameObject weapon, float power)
    {
        _animator.Play("PlayerThrow");
        GameObject obj = Instantiate(weapon);
        WeaponController weaponController = obj.GetComponent<WeaponController>();
        weaponController.power = power;
        obj.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z - weaponController.startHeightAboveLauncher);
    }
}
