using System;
using UnityEngine;


public class InputManager : MonoBehaviour
{
    public static bool canInput = true;
    public GameObject playerObj;

    private SprayAttack sprayAttack;
    private Combat combat;
    private DisplayInteracion displayInteraction;
    private PlayerMovement playerMovement;
    [SerializeField] private MapManager mapManager;

    void Start()
    {
        sprayAttack = playerObj.GetComponent<SprayAttack>();
        combat = playerObj.GetComponent<Combat>();
        displayInteraction = playerObj.GetComponent<DisplayInteracion>();
        playerMovement = playerObj.GetComponent<PlayerMovement>();
        Debug.Log(mapManager);
    }
    
    void Update()
    {
        /*
        foreach(KeyCode kcode in Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKey(kcode))
                Debug.Log("KeyCode down: " + kcode);
        }
        */
        
        if(!canInput) return;
        
        sprayAttack.sprayAttackAxis = Input.GetAxisRaw("SprayAttack");

        combat.rightAttack = Input.GetButtonDown("RightAttack");
        combat.leftAttack = Input.GetButtonDown("LeftAttack");
        combat.uptAttack = Input.GetButtonDown("UpAttack");
        combat.downAttack = Input.GetButtonDown("DownAttack");
        combat.specialAttack = Input.GetButtonDown("SpecialAttack");

        displayInteraction.interact = Input.GetButtonDown("Fire1");
        
        playerMovement.horizontalAxis = Input.GetAxisRaw("Horizontal");
        playerMovement.verticalAxis = Input.GetAxisRaw("Vertical");
        //playerMovement.dash = Input.GetButtonDown("Dash");
        playerMovement.dash = Input.GetMouseButtonDown(1);
        
        
        
        if(mapManager == null) return;
        //mapManager.mapInput = Input.GetKeyDown(KeyCode.M) || Input.GetKeyDown(KeyCode.JoystickButton7);
        mapManager.mapInput = Input.GetButtonDown("DisplayMap") || Input.GetKeyDown(KeyCode.JoystickButton7);
    }
}
