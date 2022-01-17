using UnityEngine;

/// <summary>
/// La classe qui gere les mouvement du joueur.
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    //Player RigidBody
    private Rigidbody2D rb;
    public Fall fall;
    
    //normal movement
    public float speed = 10f;
    public Vector3 inputMovement;
    [SerializeField] private float deadZone = 0.3f;
    
    public Vector3 lastDirection;

    //dash related
    public float dashSpeed = 40f;
    public bool dashing = false;
    private float dashInternalCd;
    public float dashInternalCdMax = 0.1f;
    private float dashCd;
    public float dashCdMax = 1f;

    //Animations
    [SerializeField] private Animator animPlayer;
    public int playerDirection = 0;
    
    //fog of war
    [SerializeField] private FogOfWar fogOfWar;
    
    //Particules
    //[SerializeField] private ParticleSystem dust;
        
    //Inputs
    [HideInInspector] public float horizontalAxis;
    private float previousHorizontalAxis;
    [HideInInspector] public float verticalAxis;
    private float previousVerticalAxis;
    [HideInInspector] public bool dash;
    [HideInInspector] public float shootingAxis;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        dashInternalCd = 0;
        dashCd = 0;

    }

    private void Update()
    {
        inputMovement.x = horizontalAxis;
        inputMovement.y = verticalAxis;
        
        if(!InputManager.canInput) return;
        InputManager.canMove = !dashing;

        if (Mathf.Abs(horizontalAxis) > 0.3f || Mathf.Abs(verticalAxis) > 0.3f)
        {
            lastDirection = inputMovement;
            animPlayer.SetFloat("Move X",lastDirection.x);
            animPlayer.SetFloat("Move Y",lastDirection.y);
        }
        inputMovement.Normalize();
        
        if (dash)
        {
            if (dashCd <= 0)
            {
                dashCd = dashCdMax;
                dashInternalCd = 0;
                dashing = true;

                animPlayer.SetBool("IsDashing", true);
            }
        }
    }

    void FixedUpdate()
    {
        dashCd -= Time.fixedDeltaTime;

        if (dashing)
        {
            if (dashInternalCd>dashInternalCdMax)
            {
                rb.velocity = Vector2.zero;
                dashing = false;
                if(LifeManager.Instance != null) LifeManager.Instance.canTakeDamge = true;
                animPlayer.SetBool("IsDashing", false);
            }
            else
            {
                if(LifeManager.Instance != null) LifeManager.Instance.canTakeDamge = false;
                animPlayer.SetBool("IsDashing", true);
                animPlayer.SetBool("IsWalking", false);
                dashInternalCd += Time.fixedDeltaTime;
                rb.velocity = inputMovement * dashSpeed;
            }
        }
        else
        {
            MovePlayer();
        }
    }

    void MovePlayer()
    {
        rb.velocity = Vector2.zero;
        
        if(!(shootingAxis <= 0)) return;
        if(!InputManager.canInput) return;
        
        if (Mathf.Abs(inputMovement.x) > deadZone || Mathf.Abs(inputMovement.y) > deadZone)
        {
            if (GetComponent<Combat2>().isAttacking == false && GetComponent<SprayAttack>().isSpraying == false)
            {
                animPlayer.SetBool("IsWalking",true); 
                rb.velocity = inputMovement * speed;
                
                
                
                PlayCorrectDustAnim();
                
                previousHorizontalAxis = horizontalAxis;
                previousVerticalAxis = verticalAxis;
            }
            
        }
        else
        {
            animPlayer.SetBool("IsWalking",false);
        }
        
        if(fogOfWar != null) fogOfWar.UpdateMapFog(transform.position);
    }

    void PlayCorrectDustAnim()
    {
        //var currentSign = Mathf.Sign(horizontalAxis);
        //var previousSign = Mathf.Sign(previousHorizontalAxis);
        //droite : Mathf.Sign(horizontalAxis) = 1
        //gauche : Mathf.Sign(horizontalAxis) = -1
        //haut : Mathf.Sign(verticalAxis) = 1
        //bas : Mathf.Sign(verticalAxis) = -1
        
        if(ObjectPooler.Instance == null) return;

        var currentHAxis = Mathf.Sign(horizontalAxis);
        var currentVAxis = Mathf.Sign(verticalAxis);
        var prevHAxis = Mathf.Sign(previousHorizontalAxis);
        var prevVAxis = Mathf.Sign(previousVerticalAxis);
        
        
        if (prevHAxis != currentHAxis)
        {
            PlayDustH(prevHAxis > 0);
        }
        
        if (prevVAxis != currentVAxis)
        {
            PlayDustV(prevVAxis > 0);
        }
    }

    void PlayDustH(bool rightSide)
    {
        GameObject dustObj = ObjectPooler.Instance.SpawnFromPool("Dust", transform.position + new Vector3(0, -0.5f, 0),
            Quaternion.Euler(new Vector3(-90f,0,0)));
        if (dustObj == null) return;
        ParticleSystem ps = dustObj.GetComponent<ParticleSystem>();
        if(ps == null) return;
        var vol = ps.velocityOverLifetime;
        vol.x = rightSide ? new ParticleSystem.MinMaxCurve(0.15f) : new ParticleSystem.MinMaxCurve(-0.15f);
        
        if(!ps.isPlaying) ps.Play();
    }
    
    void PlayDustV(bool topSide)
    {
        GameObject dustObj = ObjectPooler.Instance.SpawnFromPool("Dust", transform.position + new Vector3(0, -0.5f, 0),
            Quaternion.identity);
        if (dustObj == null) return;
        ParticleSystem ps = dustObj.GetComponent<ParticleSystem>();
        if(ps == null) return;
        var vol = ps.velocityOverLifetime;
        vol.x = topSide ? new ParticleSystem.MinMaxCurve(0.15f) : new ParticleSystem.MinMaxCurve(-0.15f);
        
        if(!ps.isPlaying) ps.Play();
    }
}