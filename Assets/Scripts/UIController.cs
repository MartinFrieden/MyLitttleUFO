using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : Singleton<UIController>
{
    public int cashOnLevel = 0;
    public int totalCash;
    public Text cashText;

    public Text totalCashText;
    public Text cashForLevel;
    

    public float recordDist;
    public float lvlDist;
    public Text recordDistText;
    public Text lvlDistText;
    public float distAddTime = 1f;

    public string currentLvl;


    void Start()
    {
        totalCashText.text = "X " + PlayerPrefs.GetInt("totalCash");
        recordDistText.text = "Record: " + PlayerPrefs.GetFloat(currentLvl);
        Invoke("AddDistance", 1f / distAddTime);
    }

    void Update()
    {
        
    }

    public void AddCoin()
    {    
        totalCash = PlayerPrefs.GetInt("totalCash");
        cashOnLevel += 1;
        PlayerPrefs.SetInt("totalCash", totalCash+1);
        cashText.text = "X " + cashOnLevel;
        totalCashText.text = "X " + PlayerPrefs.GetInt("totalCash");
        cashForLevel.text = "+ " + cashOnLevel;
    }

    public void AddDistance()
    {
        recordDist = PlayerPrefs.GetFloat(currentLvl);
        lvlDist += 1;
        if(lvlDist > recordDist)
        {
            PlayerPrefs.SetFloat(currentLvl, lvlDist);
            recordDistText.text = "Record: " + PlayerPrefs.GetFloat(currentLvl);
        }
        lvlDistText.text = "Current: " + lvlDist;
        Invoke("AddDistance", 1f / distAddTime);
    }




}
