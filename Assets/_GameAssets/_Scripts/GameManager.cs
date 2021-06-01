using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    [SerializeField]
    GameObject baseCamera;
    [SerializeField]
    bool testMode = false;

    GameObject _currentCamera;
    public GameObject CurrentCamera
    {
        get
        {
            if (_currentCamera == null)
            {
                _currentCamera =  Camera.main.gameObject;
            }
            return _currentCamera;
        }
        set
        {
            if (_currentCamera != null)
            {
                _currentCamera.SetActive(false);
            }
            _currentCamera = value;
            CurrentCamera.SetActive(true);
        }
    }

    public int blueCount;
    public int redCount;

    Level _currentLevel;
    public Level CurrentLevel
    {
        get { return _currentLevel; }
        set { _currentLevel = value; }
    }

    GameState _gameState;
    public GameState gameState
    {
        get { return _gameState; }
        set { _gameState = value; }
    }

    public float maxRadius;
    public float maxHeight;
    public float minHeight;

    public static GameManager instance = null;


    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        SetTeams();
        if (!testMode)
        {
            SetSpawners();
        }
        DontDestroyOnLoad(transform.gameObject);
    }

    private void Start()
    {
        if (!testMode)
        {
            StartMenu();
        }
    }

    void StartMenu()
    {
       SceneManager.LoadSceneAsync((int)Level.MainMenu, LoadSceneMode.Single);
    }

    public void StartGame()
    {
        StartCoroutine(StartGameCorrutine());
    }

    IEnumerator StartGameCorrutine()
    {
        yield return SceneManager.LoadSceneAsync((int)Level.GameMechanics, LoadSceneMode.Single);
        CurrentCamera = Camera.main.gameObject;
        //  yield return StartCoroutine(LoadSceneCorrutine((int)Level.Space));

    }

    public void LoadScene(int levelToLoad)
    {
        StartCoroutine(LoadSceneCorrutine(levelToLoad));
    }

    IEnumerator LoadSceneCorrutine(int levelToLoad)
    {
        yield return SceneManager.LoadSceneAsync((int)Level.GameMechanics, LoadSceneMode.Single);
        CurrentCamera = Camera.main.gameObject;
        yield return SceneManager.LoadSceneAsync(levelToLoad, LoadSceneMode.Additive);
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(levelToLoad));
        CurrentLevel = (Level)levelToLoad;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameState = gameState == GameState.InGame ? GameState.Paused : GameState.InGame;
        }

        Time.timeScale = gameState == GameState.Paused ? 0 : 1;

        ChekEndGame();
    }

    public List<SpawnerState> spawners = new List<SpawnerState>();

   public System.Array teams;

    public class SpawnerState
    {
      public Spawner spawner;
      public  float currentLife;
      public  bool isDestroyed;
      public  Team team;
      public  Level level;
    }

   void SetSpawners()
    {
        spawners.Add(new SpawnerState() { level = Level.Space, currentLife = 1,isDestroyed = false });
        spawners.Add(new SpawnerState() { level = Level.Air, currentLife = 1, isDestroyed = false });
        spawners.Add(new SpawnerState() { level = Level.Water, currentLife = 1, isDestroyed = false });
    }

    void ChekEndGame()
    {
      // if (spawners.TrueForAll(SpawnerIsDestroyed))
      // {
      //     EndGame();
      // }
    }

  public  void EndGame(bool death = false)
    {
        SpaceShip[] ships = FindObjectsOfType<SpaceShip>();

        foreach (SpaceShip ship in ships)
        {
            Destroy(ship.gameObject);
        }
        if (death)
        {
            SceneManager.LoadScene("Defeat",LoadSceneMode.Single);
        }
        else
        {
            SceneManager.LoadScene("Victory", LoadSceneMode.Single);
        }

        spawners.Clear();
         SetSpawners();
    }

    bool SpawnerIsDestroyed(SpawnerState spawner)
    {
        return spawner.isDestroyed;
    }

    void SetTeams()
    {
        teams = System.Enum.GetValues(typeof(Team));
    }

    public LayerMask SetLayers(Team _team)
    {
        LayerMask layerMaskEnemies = 0;

        foreach (Team team in teams)
        {
            if (team != _team)
            {
                layerMaskEnemies = layerMaskEnemies | 1 << (int)team;
            }
        }
        return layerMaskEnemies;
    }

    public enum Team
    {
        blue = 10,
        red = 11
    }

    public enum GameState
    {
        InGame,
        Paused
    }

    public enum Level
    {
        MainMenu = 0,
        GameMechanics = 1,
        //  InGameMenu = 2,
        Space = 2,
        Air = 3,
        Water = 4,
        GameOver = 5,
        Finish = 6
    }
}
