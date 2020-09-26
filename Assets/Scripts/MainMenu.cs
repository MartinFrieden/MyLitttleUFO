using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Set in Inspector")]
    public RectTransform mainMenu;
    public RectTransform levelMenu;
    public RectTransform noCoins;
    public Button lvl;
    public int costLvl;
    public Text totalCashText;
    public Text distLvl_1_Dist;
    public Text distLvl_2_Dist;
    public Text distLvl_3_Dist;

    public Button lvl2;
    public Button lvl3;
    public Button unlock2;
    public Button unlock3;

    // Имя сцены, содержащей саму игру.
    public string sceneToLoad;

    // Компонент пользовательского интерфейса,
    // содержащий текст "Loading...".
    public RectTransform loadingOverlay;

    // Выполняет загрузку сцены в фоновом режиме. Используется
    // для управления, когда требуется переключить сцену.
    AsyncOperation sceneLoadingOperation;


    // Start is called before the first frame update
    void Start()
    {
        if (mainMenu)
            mainMenu.gameObject.SetActive(true);
        if (levelMenu)
            levelMenu.gameObject.SetActive(false);
        if(totalCashText)
            totalCashText.text = "X " + PlayerPrefs.GetInt("totalCash");
        if (distLvl_1_Dist)
            distLvl_1_Dist.text = "Goal: 200" + "\n" + "Current: " + PlayerPrefs.GetFloat("recordDist");
        if (distLvl_2_Dist)
            distLvl_2_Dist.text = "Goal: 200" + "\n" + "Current: " + PlayerPrefs.GetFloat("recordDist2");
        if (distLvl_3_Dist)
            distLvl_3_Dist.text = "Goal: 200" + "\n" + "Current: " + PlayerPrefs.GetFloat("recordDist3");
        if (PlayerPrefs.GetInt("avalibleLvl2") == 1)
        {
            lvl2.interactable = true;
            unlock2.gameObject.SetActive(false); 
        }
        if (PlayerPrefs.GetInt("avalibleLvl3") == 1)
        {
            lvl2.interactable = true;
            unlock2.enabled = false;
        }
    }

    public void LoadScene(string sceneToLoadIn)
    {
        Time.timeScale = 1.0f;
        sceneToLoad = sceneToLoadIn;
        // Скрыть заставку 'loading'
        loadingOverlay.gameObject.SetActive(false);
        // Начать загрузку сцены в фоновом режиме...
        sceneLoadingOperation =
        SceneManager.LoadSceneAsync(sceneToLoad);
        // ...но не переключаться в новую сцену,
        // пока мы не будем готовы.
        sceneLoadingOperation.allowSceneActivation = false;

        // Сделать заставку 'Loading' видимой
        loadingOverlay.gameObject.SetActive(true);
        levelMenu.gameObject.SetActive(false);
        mainMenu.gameObject.SetActive(false);
        // Сообщить операции загрузки сцены, что требуется
        // переключить сцены по окончании загрузки.
        sceneLoadingOperation.allowSceneActivation = true;
    }

    public void CloserRect(RectTransform rect)
    {
        rect.gameObject.SetActive(false);
    }

    public void OpenLevel(Button but)
    {
        int cash = PlayerPrefs.GetInt("totalCash");
        lvl = but;
        costLvl = lvl.GetComponent<LevelCost>().costLvl;

        if (cash >= costLvl )
        {
            lvl.interactable = true;
            PlayerPrefs.SetInt(lvl.GetComponent<LevelCost>().whatOpen, 1);
            PlayerPrefs.SetInt("totalCash", cash - costLvl);

            totalCashText.text = "X " + PlayerPrefs.GetInt("totalCash");
        }
        else
        {
            noCoins.gameObject.SetActive(true);
        }
    }

    public IEnumerator Waiting()
    {
        yield return new WaitForSeconds(3);
    }

    public void ActiveLevel()
    {
        if (mainMenu)
            mainMenu.gameObject.SetActive(false);
        if (levelMenu)
            levelMenu.gameObject.SetActive(true);
    }
    public void ActiveMain()
    {
        if (mainMenu)
            mainMenu.gameObject.SetActive(true);
        if (levelMenu)
            levelMenu.gameObject.SetActive(false);
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
