using System.Collections;
using UnityEngine;

public class WeaponDestroyKillOffManager : MonoBehaviour
{
    [SerializeField]
    private int _secondsBeforeDestroy = 10;

    private void Awake()
    {
        StartCoroutine(RemoveFromPlay());
    }
    private IEnumerator RemoveFromPlay()
    {
        yield return new WaitForSeconds(_secondsBeforeDestroy);
        Destroy(this.gameObject);
    }
}
