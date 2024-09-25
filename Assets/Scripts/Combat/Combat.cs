using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{
    private MovementController movementControllerScript;
    private Animator playerAnim;
    private float slashCooldown;
    // Start is called before the first frame update
    void Start()
    {
        movementControllerScript = GetComponent<MovementController>();
        playerAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        SwordSlash();
    }

    IEnumerator swordSlashCoolDown()
    {
        yield return new WaitForSeconds(2.26f);
        playerAnim.SetFloat("normal_sword_slash", 1);
    }

    void SwordSlash()
    {
        if (movementControllerScript.verticalInput == 0 && movementControllerScript.horizontalInput == 0 && Input.GetMouseButtonDown(0))
        {
            playerAnim.SetFloat("normal_sword_slash", 2f);
            Debug.Log("Pressed");
            slashCooldown -= Time.deltaTime;
            StartCoroutine(swordSlashCoolDown());
            
        }
    }


}
