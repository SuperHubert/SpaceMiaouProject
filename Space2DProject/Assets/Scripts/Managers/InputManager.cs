using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static bool canInput = true;
    public static bool canMove = true;
    public bool isMoving;
    public GameObject playerObj;

    [SerializeField] private bool showKeycodes = false;
    
    private SprayAttack sprayAttack;
    private Combat combat;
    private DisplayInteracion displayInteraction;
    private PlayerMovement playerMovement;
    [SerializeField] private MapManager mapManager;
    [SerializeField] private ShopInteraction shopInteraction;
    
    void Start()
    {
        sprayAttack = playerObj.GetComponent<SprayAttack>();
        combat = playerObj.GetComponent<Combat>();
        displayInteraction = playerObj.GetComponent<DisplayInteracion>();
        playerMovement = playerObj.GetComponent<PlayerMovement>();
    }
    
    void Update()
    {
        if (showKeycodes)
        {
            foreach(KeyCode kcode in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKey(kcode))
                    Debug.Log("KeyCode down: " + kcode);
            }
        }
        
        if(!canInput) return;
        
        if (shopInteraction != null)
        {
            if (!shopInteraction.shopUI.activeSelf)
            {
                sprayAttack.sprayAttackAxis = Input.GetAxisRaw("SprayAttack");
                playerMovement.shootingAxis = Input.GetAxisRaw("SprayAttack");

                combat.rightAttack = Input.GetButtonDown("RightAttack");
                combat.leftAttack = Input.GetButtonDown("LeftAttack");
                combat.uptAttack = Input.GetButtonDown("UpAttack");
                combat.downAttack = Input.GetButtonDown("DownAttack");
                combat.specialAttack = Input.GetButtonDown("SpecialAttack");
            }
            
            shopInteraction.closeShopInput = Input.GetKeyDown(KeyCode.JoystickButton1);
        }
        
        displayInteraction.interact = Input.GetButtonDown("Fire1");

        if (canMove)
        {
            playerMovement.horizontalAxis = Input.GetAxisRaw("Horizontal");
            playerMovement.verticalAxis = Input.GetAxisRaw("Vertical");
            playerMovement.dash = Input.GetMouseButtonDown(1) || Input.GetAxisRaw("Dash") > 0;

            isMoving = Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0;
        }
        

        
        if (mapManager != null)
        {
            mapManager.mapExitInput = Input.GetButtonDown("Cancel") || Input.GetKeyDown(KeyCode.JoystickButton1);
            mapManager.mapInput = Input.GetButtonDown("DisplayMap") || Input.GetKeyDown(KeyCode.JoystickButton7);
        }
        
    }
}
