using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeWeapon : MonoBehaviour
{
    public Mesh[] weaponMeshes; // Array of weapon meshes
    public Material[] weaponMaterials; // Array of corresponding weapon materials
    public Vector3[] weaponRotations; // Array of rotation offsets for each weapon

    private MeshFilter weaponMeshFilter;
    private MeshRenderer weaponMeshRenderer;
    private Transform weaponTransform; // Reference to the weapon's Transform
    [SerializeField] private PlayerController playerControllerScript;

    void Start()
    {
        // Assume the weapon is a child of the character and has the necessary components
        weaponMeshFilter = GetComponentInChildren<MeshFilter>();
        weaponMeshRenderer = GetComponentInChildren<MeshRenderer>();
        weaponTransform = weaponMeshFilter.transform;

        if (playerControllerScript == null)
        {
            playerControllerScript = GetComponent<PlayerController>();

            if (playerControllerScript == null)
            {
                Debug.LogError("PlayerController script not found! Assign it in the Inspector or check the GameObject structure.");
            }
        }

        if (weaponMeshFilter == null || weaponMeshRenderer == null)
        {
            Debug.LogError("Weapon's MeshFilter or MeshRenderer component not found!");
        }
    }

    void Update()
    {
        if (playerControllerScript != null && playerControllerScript.attackDamage == 40f)
        {
            EquipWeapon(1); // Equip second weapon
        }
        else if (playerControllerScript != null && playerControllerScript.attackDamage == 65f)
        {
            EquipWeapon(2); // Equip second weapon
        }
    }

    public void EquipWeapon(int weaponIndex)
    {
        if (weaponIndex < 0 || weaponIndex >= weaponMeshes.Length || weaponIndex >= weaponMaterials.Length || weaponIndex >= weaponRotations.Length)
        {
            Debug.LogError("Invalid weapon index!");
            return;
        }

        // Update the weapon's mesh
        weaponMeshFilter.mesh = weaponMeshes[weaponIndex];

        // Update the weapon's material
        Material[] currentMaterials = weaponMeshRenderer.materials;
        if (currentMaterials.Length < 2)
        {
            currentMaterials = new Material[2];
        }
        currentMaterials[0] = weaponMaterials[weaponIndex];
        currentMaterials[1] = weaponMaterials[weaponIndex];
        weaponMeshRenderer.materials = currentMaterials;

        // Adjust the weapon's rotation
        weaponTransform.localEulerAngles = weaponRotations[weaponIndex];

        Debug.Log("Equipped weapon: " + weaponMeshes[weaponIndex].name +
                  " with material: " + weaponMaterials[weaponIndex].name +
                  " and rotation: " + weaponRotations[weaponIndex]);
    }
}



//currentMaterials[0] = weaponMaterials[weaponIndex];  //Blade material
// currentMaterials[1] = otherMaterial; // Handle material
