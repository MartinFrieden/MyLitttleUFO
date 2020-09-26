using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [Header("Set in Inspector")]
    //Массив шаблонов камней
    public GameObject[] prefabRocks;
    public GameObject prefabFire;
    // Местоположение, где должен появиться игрок.
    public GameObject startingPoint;
    // Объект-шаблон для создания нового игрока
    public GameObject playerPrefab;
    public GameObject planePrefab;
    public GameObject coinPrefab;

    public float rocksSpawnPerSecondStart;
    public float rocksDefaultPadding = 1.5f;
    public float fireSpawnPerSecondStart;
    public float fireDefaultPadding = 1.5f;
    public float planeDefaultPadding = 3f;
    public float planeSpawnPerSecond = 0.3f;

    public float rocksSpawnPerSecond;
    public float fireSpawnPerSecond;

    private BoundsCheck bndCheck;
    // Сценарий, управляющий камерой, которая должна следовать за игроком
    public CameraFollow cameraFollow;
    // 'текущий' игрок (в противоположность всем погибшим)
    Player currentPlayer;

    // Компонент пользовательского интерфейса с кнопками
    // 'перезапустить и 'продолжить'
    public RectTransform mainMenu;

    // Компонент пользовательского интерфейса с кнопками
    // 'вверх', 'вниз' и 'меню'
    public RectTransform gameplayMenu;

    // Компонент пользовательского интерфейса с экраном
    // 'вы выиграли!'
    public RectTransform gameOverMenu;

    // Значение true в этом свойстве требует игнорировать любые повреждения
    // (но показывать визуальные эффекты).
    // Объявление 'get; set;' превращает поле в свойство, что
    // необходимо для отображения в списке методов в инспекторе
    // для Unity Events
    public bool playerInvincible { get; set; }

    // Задержка перед созданием нового игрока после гибели
    public float delayAfterDeath = 1.0f;

    // Звук, проигрываемый в случае гибели игрока
    public AudioClip playerDiedSound;

    // Звук, проигрываемый в случае победы в игре
    public AudioClip gameOverSound;

    public AudioClip playSound;

    void Start()
    {
        // В момент запуска игры вызвать Reset, чтобы
        // подготовить гномика.
        //CreateNewPlayer();
        //создать нового игрока
        Reset();
        bndCheck = GetComponent<BoundsCheck>();
        Invoke("SpawnRocks", 1f / rocksSpawnPerSecondStart);
        Invoke("SpawnFire", 1f / fireSpawnPerSecondStart);
        Invoke("SpawnPlane", 1f / planeSpawnPerSecond);
    }

    public void SpawnPlane()
    {
        int coinsNum = Random.Range(4, 10);

        float yMin = -bndCheck.camHeight + 2;
        float yMax = bndCheck.camHeight - 2;

        float planePadding = planeDefaultPadding;

        Vector2 scale = new Vector2(Random.Range(15, 30), 1);
        Vector2 rockScale = Vector2.one;
        Vector2 pos = Vector2.zero;
        pos.x = bndCheck.camWidth + planePadding;
        pos.y = Random.Range(yMin, yMax);

        GameObject go = Instantiate<GameObject>(planePrefab);

        if (go.GetComponent<BoundsCheck>() != null)
        {
            planePadding = Mathf.Abs(go.GetComponent<BoundsCheck>().radius);
        }
        
        go.transform.localScale = scale;
        go.transform.position = pos;

        for (int i = 0; i < coinsNum; i++)
        {
            Vector2 posCoin = Vector2.zero;
            Vector2 scaleCoin = Vector2.one;

            scaleCoin.x /= (0.25f*scale.x);
            scaleCoin.y = 4;

            posCoin.y = pos.y + 0.4f;
            posCoin.x = (pos.x - 2) + (float)i / 2;
            if (Random.Range(0, 2) == 1)
            {
                GameObject coinGo = Instantiate<GameObject>(coinPrefab, go.transform);
                coinGo.transform.position = posCoin;
                coinGo.transform.localScale = scaleCoin;
            }
        }
        planeSpawnPerSecond += 0.009f;
        Invoke("SpawnPlane", 1f / planeSpawnPerSecond);
    }

    public void SpawnRocks()
    {
        int ndx = Random.Range(0, prefabRocks.Length);
        int scale = Random.Range(2, 7);
        GameObject go = Instantiate<GameObject>(prefabRocks[ndx]);
        float rockPadding = rocksDefaultPadding;
        if (go.GetComponent<BoundsCheck>() != null)
        {
            rockPadding = Mathf.Abs(go.GetComponent<BoundsCheck>().radius);
        }

        Vector2 pos = Vector2.zero;
        Vector2 rockScale = Vector2.one;
        float yMin = -bndCheck.camHeight+1;
        float yMax = bndCheck.camHeight;
        pos.y = Random.Range(yMin, yMax);
        pos.x = bndCheck.camWidth + rockPadding;
        rockScale.y = scale;
        rockScale.x = scale;
        go.transform.position = pos;
        go.transform.localScale = rockScale;
        rocksSpawnPerSecond += 0.005f;
        Invoke("SpawnRocks", 1f/rocksSpawnPerSecond);
    }

    public void SpawnFire()
    {
        int scale = Random.Range(1, 2);
        GameObject go = Instantiate<GameObject>(prefabFire);
        float firePadding = rocksDefaultPadding;
        if (go.GetComponent<BoundsCheck>() != null)
        {
            firePadding = Mathf.Abs(go.GetComponent<BoundsCheck>().radius);
        }
        Vector2 pos = Vector2.zero;
        Vector2 fireScale = Vector2.one;
        float xMin = -bndCheck.camWidth+6;
        float xMax = bndCheck.camWidth;
        pos.y = -4.5f;
        pos.x = Random.Range(xMin, xMax);
        fireScale.x = scale;
        fireScale.y = scale;
        go.transform.position = pos;
        go.transform.localScale = fireScale;
        fireSpawnPerSecond += 0.005f;
        Invoke("SpawnFire", 1f / fireSpawnPerSecond);
    }

    public void Reset()
    {
        // Выключает меню, включает интерфейс игры
        if (gameOverMenu)
            gameOverMenu.gameObject.SetActive(false);
        if (mainMenu)
            mainMenu.gameObject.SetActive(false);
        if (gameplayMenu)
            gameplayMenu.gameObject.SetActive(true);

        // Найти все компоненты Resettable и сбросить их в исходное состояние
        var resetObjects = FindObjectsOfType<Resettable>();
        foreach (Resettable r in resetObjects)
        {
            r.Reset();
        }

        //создать нового игрока
        CreateNewPlayer();

        DeletePlanes();

        rocksSpawnPerSecond = rocksSpawnPerSecondStart;
        fireSpawnPerSecond = fireSpawnPerSecondStart;
        planeSpawnPerSecond = 0.3f;

        UIController.instance.cashOnLevel = 0;
        UIController.instance.cashText.text = "X " + 0;
        UIController.instance.cashForLevel.text = "+ " + 0;
        UIController.instance.lvlDist = 0;
        UIController.instance.lvlDistText.text = "Current: " + 0;

    // Прервать паузу в игре
    Time.timeScale = 1.0f;
        var audio = GetComponent<AudioSource>();
        if (audio)
        {
            if (audio.isPlaying)
            {
                audio.Stop();
            }
            
            audio.Play();
        }
    }
    
    void DeletePlanes()
    {
        GameObject[] planes;
        planes = GameObject.FindGameObjectsWithTag("Plane");
        foreach (GameObject b in planes)
            Destroy(b);
    }

    void CreateNewPlayer()
    {
        //удалить текущего игрока, если меется
        RemovePlayer();

        //Создать новый объект Player и назначить его текущим
        GameObject newPlayer = (GameObject)Instantiate(playerPrefab, startingPoint.transform.position, Quaternion.identity);
        currentPlayer = newPlayer.GetComponent<Player>();

        cameraFollow.target = currentPlayer.cameraFollowTarget;
    }

    void RemovePlayer()
    {
        // Ничего не делать, если игрок неуязвим
        if (playerInvincible)
            return;
        // Запретить камере следовать за игроком
        cameraFollow.target = null;

        // Если текущий игрок существует, исключить его из игры
        if (currentPlayer != null)
        {
            // Пометить объект как исключенный из игры
            // (чтобы коллайдеры перестали сообщать о столкновениях с ним)
            currentPlayer.gameObject.tag = "Untagged";
            // Найти все объекты с тегом "Player" и удалить этот тег
            foreach (Transform child in
            currentPlayer.transform)
            {
                child.gameObject.tag = "Untagged";
            }
            // Установить признак отсутствия текущего игрока
            currentPlayer = null;
        }

    }

    //убивает игрока
    public void KillPlayer(Player.DamageType damageType)
    {
        // Если задан источник звука, проиграть звук "гибель игрока"
        var audio = GetComponent<AudioSource>();
        if (audio)
        {
            audio.PlayOneShot(this.playerDiedSound);
        }
        // Показать эффект действия ловушки
        currentPlayer.ShowDamageEffect(damageType);
        // Если гномик уязвим, сбросить игру
        // и исключить гномика из игры.
        if (playerInvincible == false)
        {
            // Сообщить игроку, что он погиб
            currentPlayer.DestroyPlayer(damageType);
            // Удалить игрока
            RemovePlayer();
            // Сбросить игру
            //StartCoroutine(ResetAfterDelay());
            StartCoroutine(TheEnd());
        }
    }

    // Вызывается в момент гибели игрока
    IEnumerator ResetAfterDelay()
    {
        // Ждать delayAfterDeath секунд, затем вызвать Reset
        yield return new WaitForSeconds(delayAfterDeath);
        Reset();
    }

    // Вызывается, когда игрок касается ловушки
    // с ножами
    public void TrapTouched()
    {
        KillPlayer(Player.DamageType.Slicing);
    }
    // Вызывается, когда игрок касается огненной ловушки
    public void FireTrapTouched()
    {
        KillPlayer(Player.DamageType.Burning);
    }

    // Вызывается в ответ на касание кнопок Menu и Resume Game.
    public void SetPaused(bool paused)
    {
        // Если игра на паузе, остановить время и включить меню
        // (и выключить интерфейс игры)
        if (paused)
        {
            Time.timeScale = 0.0f;
            mainMenu.gameObject.SetActive(true);
            gameplayMenu.gameObject.SetActive(false);
        }
        else
        {
            // Если игра не на паузе, возобновить ход времени и
            // выключить меню (и включить интерфейс игры)
            Time.timeScale = 1.0f;
            mainMenu.gameObject.SetActive(false);
            gameplayMenu.gameObject.SetActive(true);
        }
    }

    // Вызывается в ответ на касание кнопки Restart.
    public void RestartGame()
    {
        // Немедленно удалить гномика (минуя этап гибели)
        Destroy(currentPlayer.gameObject);
        currentPlayer = null;
        // Сбросить игру в исходное состояние, чтобы создать нового гномика.
        Reset();
    }

    public IEnumerator TheEnd()
    {
        yield return new WaitForSeconds(delayAfterDeath);
        // Приостановить игру
        Time.timeScale = 0.1f;
        // Выключить меню завершения игры и включить экран
        // "игра завершена"!
        if (gameOverMenu)
        {
            gameOverMenu.gameObject.SetActive(true);
            
        }
        if (gameplayMenu)
        {
            gameplayMenu.gameObject.SetActive(false);
        }
        var audio = GetComponent<AudioSource>();
        if (audio)
        {
            audio.PlayOneShot(this.gameOverSound);
        }
    }
}
