using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeWeapon : MonoBehaviour
{
    public GameObject playerWeapon;


    public void EquipWeapon(Mesh weaponMesh, Material[] weaponMaterials)
    {
        playerWeapon.GetComponent<MeshFilter>().mesh = weaponMesh;
        playerWeapon.GetComponent<MeshRenderer>().materials = weaponMaterials;

    }
}


