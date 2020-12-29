using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using Pathfinding;

public class PlayerMover : MonoBehaviour {

    public float speed;
    public Transform[] movePoints = new Transform[4];
    public Vector3[] destination;
    public AudioClip[] damageSounds;
    public AudioClip deathClip;
    public AudioClip pickupSound;
    public AudioClip oneHitKillSound;
    public AudioClip invincibilitySound;
    public AudioClip infiniteAmmoSound;
    [HideInInspector] public bool isMoving = false;
    [HideInInspector] public bool canMove = false;
    [HideInInspector] public int rifleTotalAmmo = 90;
    [HideInInspector] public int shotgunTotalAmmo = 21;
    [HideInInspector] public int health = 100;

    //pickup bools
    [HideInInspector] public bool isInvincible = false;
    [HideInInspector] public bool isInfinite = false;
    [HideInInspector] public bool isOnePunchMan = false;

    //Timers for pickups
    [HideInInspector] public float invincibilityCountDown = 0;
    [HideInInspector] public float infiniteCountDown = 0;
    [HideInInspector] public float onePunchManCountDown = 0;

    [HideInInspector] public List<int> slidersOnScreen = new List<int> { 1, 2, 3 };
    [HideInInspector] public int infiniteScreenPos = 1;
    [HideInInspector] public int oneHitKillScreenPos = 2;
    [HideInInspector] public int invincibilityScreenPos = 3;

    private float horizontal;
    private float vertical;
    private float lookLength = 100f;
    private int bgMask;
    private Vector3 previousPosition;
    private int frames = 0;
    private int oldPos;
    private bool inMoveRoutine = false;
    private bool isPlayingDamageSounds = false;
    private bool isDying = false;
    

    Rigidbody body;
    GameObject currentState;
    Animator bodyAnimator;
    Animator feetAnimator;
    ShootScript shootScript;
    Slider healthSlider;
    NavMeshAgent agent;
    AIPath path;
    AIDestinationSetterPlayer setter;
    GameObject oneHitKillSlider;
    GameObject infiniteAmmoSlider;
    GameObject invincibilitySlider;
    Slider oneHitKillSliderScript;
    Slider infiniteAmmoSliderScript;
    Slider invincibilitySliderScript;
    AudioSource playerAudio;

    void Awake () {
        agent = GetComponent<NavMeshAgent>();
        body = GetComponent<Rigidbody>();
        bgMask = LayerMask.GetMask("Background");
        currentState = gameObject.transform.Find("Handgun").gameObject;
        currentState.SetActive(true);
        GetOtherComponents();
        feetAnimator = gameObject.transform.Find("Feet").GetComponent<Animator>();
        previousPosition = transform.position;
        healthSlider = GameObject.FindGameObjectWithTag("HealthSlider").GetComponent<Slider>();
        path = GetComponent<AIPath>();
        setter = GetComponent<AIDestinationSetterPlayer>();
        oneHitKillSlider = GameObject.FindGameObjectWithTag("OneHitKillSlider");
        oneHitKillSliderScript = oneHitKillSlider.GetComponent<Slider>();
        infiniteAmmoSlider = GameObject.FindGameObjectWithTag("InfiniteAmmoSlider");
        infiniteAmmoSliderScript = infiniteAmmoSlider.GetComponent<Slider>();
        invincibilitySlider = GameObject.FindGameObjectWithTag("InvincibilitySlider");
        invincibilitySliderScript = invincibilitySlider.GetComponent<Slider>();
        playerAudio = GetComponent<AudioSource>();

        CheckCountdown();
    }

    private void FixedUpdate()
    {
        if (isMoving == true && inMoveRoutine == false && health > 0)
        {
            
            StartCoroutine(MovePlayer());
        }

        if (canMove == true && health > 0)
        {
            Move();
            Turn(); 
        }
        
    }

    private void Update()
    {
        PlayerSwitcher();
        if (frames == 10)
        {
            determineMagnitude(previousPosition);
            frames = 0;
        }
        ++frames;
        StartCoroutine(CheckHealth());
        CheckCountdown();
        SetPickupSliders();
        CheckSliderPos();
    }

    private void LateUpdate()
    {
        transform.localEulerAngles = new Vector3(90f, transform.localEulerAngles.y, transform.localEulerAngles.z);
        transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
    }

    private void Move()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        if (horizontal == 0 && vertical == 0)
        {
            bodyAnimator.SetBool("IsWalking", false);
        }
        else if (bodyAnimator.GetBool("IsWalking") == false)
        {
            bodyAnimator.SetBool("IsWalking", true);
        }

        body.velocity = new Vector3(horizontal, 0f, vertical) * speed;

        if (Input.GetButton("Run"))
        {
            body.velocity *= 1.5f;
            if(shootScript != null)
            {
                shootScript.isRunning = true;
            }
            
        }
        else if (!Input.GetButton("Run"))
        {
            if (shootScript != null)
            {
                shootScript.isRunning = false;
            }
        }
    }

    void Turn()
    {
        Ray look = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit pos;

        if (Physics.Raycast(look, out pos, lookLength, bgMask))
        {
            Vector3 playerLook = pos.point - transform.position;
            Quaternion n_rotation = Quaternion.LookRotation(playerLook);
            body.MoveRotation(n_rotation);
        }
    }

    private void PlayerSwitcher()
    {
        if (Input.GetButtonDown("Handgun") && currentState.name != "Handgun")
        {
            currentState.SetActive(false);
            currentState = gameObject.transform.Find("Handgun").gameObject;
            currentState.SetActive(true);
            GetOtherComponents();
        }

        else if (Input.GetButtonDown("Rifle") && currentState.name != "Rifle")
        {
            currentState.SetActive(false);
            currentState = gameObject.transform.Find("Rifle").gameObject;
            currentState.SetActive(true);
            GetOtherComponents();
        }

        else if (Input.GetButtonDown("Shotgun") && currentState.name != "Shotgun")
        {
            currentState.SetActive(false);
            currentState = gameObject.transform.Find("Shotgun").gameObject;
            currentState.SetActive(true);
            GetOtherComponents();
        }

        else if (Input.GetButtonDown("Knife") && currentState.name != "Knife")
        {
            currentState.SetActive(false);
            currentState = gameObject.transform.Find("Knife").gameObject;
            currentState.SetActive(true);
            GetOtherComponents();
        }

        else if (Input.GetButtonDown("Flashlight") && currentState.name != "Flashlight")
        {
            currentState.SetActive(false);
            currentState = gameObject.transform.Find("Flashlight").gameObject;
            currentState.SetActive(true);
            GetOtherComponents();
        }
    }

    private void GetOtherComponents()
    {
        bodyAnimator = currentState.GetComponent<Animator>();

        if (currentState.name == "Handgun")
        {
            shootScript = currentState.GetComponentInChildren<ShootScript>();
        }
        else if (currentState.name == "Rifle")
        {
            shootScript = currentState.GetComponentInChildren<RifleShoot>();
        }
        else if (currentState.name == "Shotgun")
        {
            shootScript = currentState.GetComponentInChildren<ShotgunShoot>();
        }
        else
        {
            shootScript = null;
        }
    }

    private void determineMagnitude(Vector3 previous)
    {
        int pos = 4;

        Vector3 findOffset = transform.position - previous;

        if (previous != transform.position && findOffset.sqrMagnitude > 0.001f)
        {
            float current = 0f;

            for (int i = 0; i != movePoints.Length; ++i)
            {

                Vector3 offset = transform.position - movePoints[i].position;
                float sqr = offset.sqrMagnitude;
                Vector3 priorOffset = previous - movePoints[i].position;
                float priorSqr = priorOffset.sqrMagnitude;

                //Debug.Log(sqr - priorSqr);

                if (current == 0 || current > sqr - priorSqr)
                {
                    current = sqr - priorSqr;
                    pos = i;
                }
            }
        }

        //Debug.Log("oldPos: " + oldPos + " " + "newPos: " + pos);

        usePos(pos);

        previousPosition = transform.position;
    }

    private void usePos(int i)
    {

        //Debug.Log("usePos is being called");
        switch (i)
        {
            case 0:
                setFalse();
                if (Input.GetButton("Run"))
                {
                    feetAnimator.SetBool("IsRunning", true);
                    break;
                }
                feetAnimator.SetBool("IsWalking", true);
                break;
            case 1:
                setFalse();
                if (Input.GetButton("Run"))
                {
                    feetAnimator.SetBool("IsRunningBackwards", true);
                    break;
                }
                feetAnimator.SetBool("IsWalkingBackwards", true);
                break;
            case 2:
                setFalse();
                if (Input.GetButton("Run"))
                {
                    feetAnimator.SetBool("FastStrafeLeft", true);
                    break;
                }
                feetAnimator.SetBool("StrafeLeft", true);
                break;
            case 3:
                setFalse();
                if (Input.GetButton("Run"))
                {
                    feetAnimator.SetBool("FastStrafeRight", true);
                    break;
                }
                feetAnimator.SetBool("StrafeRight", true);
                break;
            case 4:
                setFalse();
                break;
        }

        oldPos = i;
    }

    private void setFalse()
    {
        foreach (AnimatorControllerParameter parameter in feetAnimator.parameters)
        {
            feetAnimator.SetBool(parameter.name, false);
        }
    }

    public void TakeDamage()
    {
        if (isInvincible == false)
        {
            StartCoroutine(playDamageSounds());
            health -= 10;
            healthSlider.value = health;
        }

        if (health == 0)
        {
            GameObject fill = GameObject.FindGameObjectWithTag("HealthFill");
            fill.SetActive(false);
        }
    }

    private IEnumerator playDamageSounds()
    {
        if(isPlayingDamageSounds == false && health > 0)
        {
            isPlayingDamageSounds = true;
            playerAudio.clip = damageSounds[Random.Range(0, damageSounds.Length)];
            playerAudio.Play();
            yield return new WaitForSeconds(playerAudio.clip.length);
            isPlayingDamageSounds = false;
        }
    }

    private IEnumerator CheckHealth()
    {
        if(health <= 0 && isDying == false)
        {
            isDying = true;
            body.isKinematic = true;
            playerAudio.Stop();
            playerAudio.clip = deathClip;
            playerAudio.Play();
            yield return new WaitForSeconds(deathClip.length);
            Destroy(gameObject);
        }
    }

    private IEnumerator MovePlayer()
    {
        body.velocity = Vector3.zero;
        body.isKinematic = true;
        path.enabled = true;
        setter.enabled = true;
        inMoveRoutine = true;

        setter.target = FindDestination();

        while (transform.position.sqrMagnitude > setter.target.sqrMagnitude + 0.1f 
            || transform.position.sqrMagnitude < setter.target.sqrMagnitude - 0.1f)
        {
            yield return null;
        }

        body.isKinematic = false;
        body.velocity = Vector3.zero;
        setter.enabled = false;
        path.enabled = false;
        isMoving = false;
        inMoveRoutine = false;

    }

    //private IEnumerator MoveToPoint()
    //{
    //    agent.enabled = true;
    //    Vector3 position = FindDestination();
    //    Debug.Log(position.x + " " + position.y + " " + position.z);
    //    agent.SetDestination(position);
    //    while (transform.position != new Vector3(position.x, transform.position.y, position.z))
    //    {
    //        yield return null;
    //    }
    //    isMoving = false;
    //    agent.enabled = false;
    //}

    private Vector3 FindDestination()
    {
        Vector3 closest = destination[0];
        for (int i = 0; i != destination.Length; ++i)
        {
            if (Vector3.SqrMagnitude(transform.position - closest) > Vector3.SqrMagnitude(transform.position - destination[i]))
            {
                closest = destination[i];
            }
        }
        return closest;
    }

    private void CheckCountdown()
    {
        if (isInfinite == true)
        {
            infiniteCountDown -= Time.deltaTime;
        }
        if(isInvincible == true)
        {
            invincibilityCountDown -= Time.deltaTime;
        }
        if(isOnePunchMan == true)
        {
            onePunchManCountDown -= Time.deltaTime;
        }

        if (infiniteCountDown <= 0)
        {
            isInfinite = false;
            infiniteAmmoSlider.SetActive(false);
            if (slidersOnScreen.Contains(infiniteScreenPos))
            {
                slidersOnScreen.Remove(infiniteScreenPos);
            }
        }
        if(invincibilityCountDown <= 0)
        {
            isInvincible = false;
            invincibilitySlider.SetActive(false);
            if (slidersOnScreen.Contains(invincibilityScreenPos))
            {
                slidersOnScreen.Remove(invincibilityScreenPos);
            }
        }
        if(onePunchManCountDown <= 0)
        {
            isOnePunchMan = false;
            oneHitKillSlider.SetActive(false);
            if (slidersOnScreen.Contains(oneHitKillScreenPos))
            {
                slidersOnScreen.Remove(oneHitKillScreenPos);
            }
        }
    }

    private void SetPickupSliders()
    {
        if(isInfinite == true)
        {
            infiniteAmmoSlider.SetActive(true);
            infiniteAmmoSliderScript.value = infiniteCountDown;
        }
        if(isInvincible == true)
        {
            invincibilitySlider.SetActive(true);
            invincibilitySliderScript.value = invincibilityCountDown;
        }
        if(isOnePunchMan == true)
        {
            oneHitKillSlider.SetActive(true);
            oneHitKillSliderScript.value = onePunchManCountDown;
        }

    }

    private void CheckSliderPos()
    {

        float initPos = 345;
        float xPos = 565;
        if (slidersOnScreen.Count != 0)
        {
            for (int i = 0; i != slidersOnScreen.Count; ++i)
            {
                Debug.Log(i);
                switch (slidersOnScreen[i])
                {
                    case 1:
                        infiniteAmmoSlider.transform.position = new Vector3(xPos, initPos, 0f);
                        break;
                    case 2:
                        oneHitKillSlider.transform.position = new Vector3(xPos, initPos, 0f);
                        break;
                    case 3:
                        invincibilitySlider.transform.position = new Vector3(xPos, initPos, 0f);
                        break;

                }
                initPos -= 12.5f;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "RifleAmmo(Clone)" || other.name == "ShotgunAmmo(Clone)")
        {
            playerAudio.clip = pickupSound;
            playerAudio.Play();
        }
        else if(other.name == "OneHitKill(Clone)")
        {
            playerAudio.clip = oneHitKillSound;
            playerAudio.Play();
        }
        else if(other.name == "Invincibility(Clone)")
        {
            playerAudio.clip = invincibilitySound;
            playerAudio.Play();
        }
        else if (other.name == "InfiniteAmmo(Clone)")
        {
            playerAudio.clip = infiniteAmmoSound;
            playerAudio.Play();
        }

    }

}
