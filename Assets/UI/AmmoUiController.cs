using TMPro;
using UnityEngine;

public enum AmmoSource
{
    Primary,
    Secondary,
}

public class AmmoUiController : MonoBehaviour
{
    [SerializeField]
    private AmmoSource _ammoSource;
    private TextMeshProUGUI _tmp;
    private PlayerController _playerController;

    private void Awake()
    {
        _tmp = GetComponent<TextMeshProUGUI>();
        _playerController = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (_ammoSource)
        {
            case AmmoSource.Primary:
                if (_playerController.PrimaryAmmo == -1)
                {
                    _tmp.text = "∞";
                }
                else
                {
                    _tmp.text = _playerController.PrimaryAmmo.ToString();
                }
                break;

            case AmmoSource.Secondary:
                if (_playerController.SecondaryAmmo == -1)
                {
                    _tmp.text = "∞";
                }
                else
                {
                    _tmp.text = _playerController.SecondaryAmmo.ToString();
                }
                break;
        }
    }
}
