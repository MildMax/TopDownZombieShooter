using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    static public GameController instance;
    public GameObject player;
    public GameObject zombie;
    public AnimationClip roundAnim;
    public float spawnTime;
    public Text zombieText;
    public Text waveText;
    public GameObject[] worldPickUps;
    public AudioClip music1;
    public AudioClip music2;
    public AudioClip music3;
    [HideInInspector] public bool betweenRounds = true;
    [HideInInspector] public int zombiesStart = 12;
    [HideInInspector] public int zombiesSpawned = 0;
    [HideInInspector] public int rHasDropped = 0;
    [HideInInspector] public int sHasDropped = 0;


    //RoomPrefabs
    public GameObject[] layoutRoom1;
    public GameObject[] layoutRoom2;
    public GameObject[] layoutRoom3;
    public GameObject[] layoutRoom4;
    public GameObject[] layoutRoom5;
    public GameObject[] layoutRoom6;
    public GameObject[] layoutRoom7;
    public GameObject[] layoutRoom8;
    public GameObject[] layoutRoom9;

    //spawnpoints by room
    //keep this cause I don't wanna go back and correct the old room spawner scripts
    public Vector3[] room1;
    public Vector3[] room2;
    public Vector3[] room3;
    public Vector3[] room4;
    public Vector3[] room5;
    public Vector3[] room6;
    public Vector3[] room7;
    public Vector3[] room8;
    public Vector3[] room9;

    private List<GameObject> layoutsInUse = new List<GameObject>();

    [HideInInspector] public float waitSpawn;

    [HideInInspector] public int zombiesLeft = 0;

    private int round = 1;
    private float zombieMultiplier = 1.25f;
    private float waitSpawnMultiplier = 0.95f;
    Animator animator;
    PlayerMover playerMover;
    AstarPath graph;
    int[] pickUps;
    List<Vector3> possiblePowerUps = new List<Vector3>();
    Coroutine powerUpCoroutine;
    AudioSource music;

    private void Awake()
    {
        //ensure there's one game controller
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }

        animator = GameObject.Find("TransitionImages").GetComponent<Animator>();
        music = GetComponent<AudioSource>();

        //zombiesLeft = zombiesStart;
        player = GameObject.FindGameObjectWithTag("Player");
        playerMover = player.GetComponent<PlayerMover>();
        graph = GameObject.Find("A*").GetComponent<AstarPath>();

        StartCoroutine(SetRound());
        StartCoroutine(SpawnByRoom(room1, room2, room3, room4, room5, room6, room7, room8, room9));
        StartCoroutine(PlayMusic());

        waitSpawn = spawnTime;
        
    }

    private void FixedUpdate()
    {
        if (zombiesLeft == 0 && betweenRounds == false)
        {
            
            StartCoroutine(SetRound());
        }
        zombieText.text = "Zeds Left: " + zombiesLeft;
    }

    private IEnumerator SetRound()
    {
        if (powerUpCoroutine != null)
        {
            StopCoroutine(powerUpCoroutine);
        }

        waveText.text = "WAVE " + round;
        betweenRounds = true;
        playerMover.isMoving = true;
        playerMover.canMove = false;

        while (playerMover.isMoving == true)
        {
            yield return null;
        }

        animator.SetTrigger("RoundEnd");

        yield return new WaitForSeconds(roundAnim.length / 2);

        //reset room w/ new layout
        //ClearObjects();
        //RoomByGeneration();
        //SetWorldPickUps();

        ClearLayouts();
        RandomLayout();
        

        //increase zombies and decrease time in between spawns
        zombiesStart = (int)(zombiesStart * zombieMultiplier);
        zombiesLeft = zombiesStart;
        SetPickUps();
        

        spawnTime *= waitSpawnMultiplier;

        //set this to the time it takes inbetween rounds
        yield return new WaitForSeconds(roundAnim.length / 4);

        graph.Scan();

        yield return new WaitForSeconds(roundAnim.length / 4);

        playerMover.canMove = true;

        yield return new WaitForSeconds(2);

        zombiesSpawned = 0;
        betweenRounds = false;

        powerUpCoroutine = StartCoroutine(SpawnPowerUps());

        waveText.text = "";
        round++;
    }

    private void RandomLayout()
    {
        List<GameObject[]> layoutList = new List<GameObject[]>  { layoutRoom1, layoutRoom2, layoutRoom3, layoutRoom4, layoutRoom5,
                                                                layoutRoom6, layoutRoom7, layoutRoom8, layoutRoom9};

        for (int i = 0; i != layoutList.Count; i++)
        {
            ChooseLayout(layoutList[i]);
        }
    }

    private void ChooseLayout(GameObject[] layout)
    {
        GameObject temp = Instantiate(layout[Random.Range(0, layout.Length)], Vector3.zero, Quaternion.Euler(90f, 0f, 0f));
        layoutsInUse.Add(temp);
        //use scriptable object here
        List<Vector3> tempList = PickUpSpawnPoints.FindPoints(temp);
        //Debug.Log(tempList.Count);
        foreach(Vector3 pos in tempList)
        {
            possiblePowerUps.Add(pos);
            //Debug.Log(pos.x + " " + pos.y + " " + pos.z);
        }
    }

    private void ClearLayouts()
    {
        for (int i = 0; i != layoutsInUse.Count; i++)
        {
            Destroy(layoutsInUse[i]);
        }
        layoutsInUse.Clear();
        possiblePowerUps.Clear();
    }

    //temporary - fix so it's on a timer and spawns according to player proximity
   private IEnumerator SpawnPowerUps()
    {
        while (betweenRounds == false)
        {
            //random wait time between power ups spawning - tinker with this range
            yield return new WaitForSeconds(Random.Range(10, 21));

            List<Vector3> playerVec = FindPlayerVector();

            List<Vector3> possibleLocations = new List<Vector3>();

            //find locations near player that can also hold powerups
            foreach (Vector3 pos in playerVec)
            {
                if (possiblePowerUps.Contains(pos))
                {
                    possibleLocations.Add(pos);
                }
            }

            Instantiate(worldPickUps[Random.Range(0, worldPickUps.Length)], possibleLocations[Random.Range(0, possibleLocations.Count)], Quaternion.Euler(90f, 0f, 0f));
        }

    }

    public IEnumerator SpawnByRoom(params Vector3[][] list)
    {
        while (player != null)
        {
            if (waitSpawn >= spawnTime && betweenRounds == false
                && zombiesSpawned != zombiesStart)
            {

                SpawnZombies(list[Random.Range(0, list.Length)]);
                waitSpawn = 0f;
                yield return null;

            }
            waitSpawn += Time.deltaTime;
            yield return null;
        }
        //yield return null;
    }

    private void SpawnZombies(Vector3[] spawnPoints)
    {
        int pos = Random.Range(0, spawnPoints.Length - 1);
        GameObject zombieT = Instantiate(zombie, spawnPoints[pos], Quaternion.identity);
        //Debug.Log("x: " + spawnPoints[pos].x + " y: " + spawnPoints[pos].y + " z: " + spawnPoints[pos].z);
        //Debug.Log(gameObject.name);
        if (pickUps[zombiesSpawned] == 1 )
        {
            ZombieMover zombieM = zombieT.GetComponent<ZombieMover>();
            zombieM.pickUpStatus = Random.Range(1, 3);
        }
        ++zombiesSpawned;
    }

    private void SetPickUps()
    {
        pickUps = new int[zombiesLeft];
        int numOfPickups = zombiesLeft / 5;
        while (numOfPickups != 0)
        {
            int point = Random.Range(0, zombiesLeft);
            if(pickUps[point] != 1)
            {
                pickUps[point] = 1;
                numOfPickups -= 1;
            }
        }
    }

    private List<Vector3> FindPlayerVector()
    {
        Vector3 playerPos = player.transform.position;

        //ensure vector starts at someInt.5 to align with grid values
        playerPos = new Vector3(Mathf.Round(playerPos.x) + 0.5f, 0f, Mathf.Round(playerPos.z) + 0.5f);

        List<Vector3> playerVec = new List<Vector3>();

        //find all positions in 16x16 plane around player - tinker with dimensions 
        for(int i = -6; i != 6; ++i)
        {
            for(int j = -6; j != 6; ++j)
            {
                playerVec.Add(new Vector3(playerPos.x + j, 0f, playerPos.z + i));
            }
        }

        return playerVec;
    }

    private IEnumerator PlayMusic()
    {
        while (true)
        {
            music.clip = music1;
            music.Play();
            yield return new WaitForSeconds(music1.length + 3f);

            music.clip = music2;
            music.Play();
            yield return new WaitForSeconds(music2.length + 3f);

            music.clip = music3;
            music.Play();
            yield return new WaitForSeconds(music3.length + 3f);


        }
    }
   
}
