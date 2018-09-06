using UnityEngine;
using UnityEngine.Networking;

public class SwitchingWeapon : NetworkBehaviour {

    

    [SerializeField]
    private PlayerWeapon primaryWeapon;

    private PlayerWeapon currentWeapon;
    [SerializeField]
    private Transform weaponHolder;


    private string weaponLayerName = "Weapon";

    private void Start()
    {
        EquipWeapon(primaryWeapon);
    }

    void EquipWeapon(PlayerWeapon _weapon)
    {
        currentWeapon = _weapon;
        GameObject _weaponIns = (GameObject)Instantiate(_weapon.graphics, weaponHolder.position, weaponHolder.rotation);
        _weaponIns.transform.SetParent(weaponHolder);

        if (isLocalPlayer)
        {
            _weaponIns.layer = LayerMask.NameToLayer(weaponLayerName);
        }
    }

    public PlayerWeapon GetCurrentWeapon()
    {
        return currentWeapon;
    }
}
