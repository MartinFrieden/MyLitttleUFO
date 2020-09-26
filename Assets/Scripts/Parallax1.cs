using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax1 : MonoBehaviour
{
    [Header("Set in Inspector")]
    public GameObject poi; // Корабль игрока
    public GameObject[] panels; // Прокручиваемые панели переднего плана
    public float scrollspeed = -30f;
    // motionMult определяет степень реакции панелей на перемещение корабля игрока
    public float motionMult = 0.25f;
    private float panelHt; // Высота каждой панели
    private float depth;


    // Start is called before the first frame update
    void Start()
    {
        panelHt = panels[0].transform.localScale.x*21f;
        depth = panels[0].transform.position.z;

        panels[0].transform.position = new Vector3(0,-6f,depth);
        panels[1].transform.position = new Vector3(panelHt, -6f, depth);
    }

    // Update is called once per frame
    void Update()
    {
        float tY = -6f;
        float tX = 0;

        tX = Time.time * scrollspeed % panelHt + (panelHt * 0.5f);

        // Сместить панель panels[0]
        panels[0].transform.position = new Vector3(tX, tY, depth);

        // Сместить панель panels[1], чтобы создать эффект непрерывности
        // звездного поля
        if (tX >= 0)
        {
            panels[1].transform.position = new Vector3(tX-panelHt, tY, depth);
        }
        else
        {
            panels[1].transform.position = new Vector3(tX+panelHt, tY, depth);
        }
    }
}
