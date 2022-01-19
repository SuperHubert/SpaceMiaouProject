using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static bool canInput = true;
    public static bool canMove = true;
    public bool isMoving;
    public GameObject playerObj;

    [SerializeField] private bool showKeycodes = false;
    
    private SprayAttack sprayAttack;
    private Combat2 combat;
    private DisplayInteracion displayInteraction;
    private PlayerMovement playerMovement;
    [SerializeField] private MapManager mapManager;
    [SerializeField] private ShopInteraction shopInteraction;
    [SerializeField] private UIManager uiManager;
    
    void Start()
    {
        sprayAttack = playerObj.GetComponent<SprayAttack>();
        combat = playerObj.GetComponent<Combat2>();
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

        if(uiManager != null) uiManager.pauseInput = (Input.GetKeyDown(KeyCode.JoystickButton6) || Input.GetKeyDown(KeyCode.Escape));
        
        if(uiManager != null && uiManager.pauseUI.activeSelf) return;
        
        
        if (Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.F))
        {
            if (DialogueManager.Instance.dialogueCanvas.activeSelf)
            {
                DialogueManager.Instance.DisplayNextSentence();
            }
            else
            {
                displayInteraction.interact = true;
            }
        }
        else
        {
            displayInteraction.interact = false;
        }
        
        if(!canInput) return;
        
        if (shopInteraction != null)
        {
            if (!shopInteraction.shopUI.activeSelf)
            {
                
                if (sprayAttack.sprayAttackAxis == 0 && Input.GetKey(KeyCode.J))
                {
                    sprayAttack.sprayAttackAxis = 1;
                }
                else
                {
                    sprayAttack.sprayAttackAxis = 0;
                }
                if(sprayAttack.sprayAttackAxis == 0) sprayAttack.sprayAttackAxis = Input.GetAxisRaw("SprayAttack");
                playerMovement.shootingAxis = Input.GetAxisRaw("SprayAttack");
                
                combat.baseAttack = Input.GetButtonDown("BaseAttack") || Input.GetKeyDown(KeyCode.K);
                combat.specialAttack = Input.GetButtonDown("SpecialAttack") || Input.GetKeyDown(KeyCode.L);
            }
            
            shopInteraction.closeShopInput = Input.GetKeyDown(KeyCode.JoystickButton1);
        }
        

        if (canMove)
        {
            playerMovement.horizontalAxis = Input.GetAxisRaw("Horizontal");
            playerMovement.verticalAxis = Input.GetAxisRaw("Vertical");
            playerMovement.dash = Input.GetAxisRaw("Dash") > 0 || Input.GetAxisRaw("Dash2") > 0 || Input.GetKeyDown(KeyCode.LeftShift);

            isMoving = Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0;
        }
        
        if (mapManager != null)
        {
            mapManager.mapExitInput = Input.GetButtonDown("Cancel") || Input.GetKeyDown(KeyCode.JoystickButton1);
            mapManager.mapInput = Input.GetButtonDown("DisplayMap") || Input.GetKeyDown(KeyCode.JoystickButton7) || Input.GetKeyDown(KeyCode.M);
        }
        
    }
}
