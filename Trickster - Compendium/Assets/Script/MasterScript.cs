﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public enum rarity
{
    basic = 0,
    common = 1,
    uncommun = 2,
    rare = 3

}

public enum type
{
    attack = 0,
    skill = 1,
    power = 2
}

public class carte
{

    public string name;
    public int cost;
    public rarity rarity;

    public int cost_U;

    public type type;
    public Sprite sprite;
    public Sprite sprite_U;

    public carte(string name_, int cost_, int cost_U_, rarity rarity_, type type_)
    {
        name = name_;
        cost = cost_;
        cost_U = cost_U_;
        rarity = rarity_;
        type = type_;
    }

    public void Spriting()
    {
        sprite = Resources.Load<Sprite>(name);
        sprite_U = Resources.Load<Sprite>(name + "_U"); // A changer en name + "_U"
    }

}


public class MasterScript : MonoBehaviour
{

    string[] parts;
    string[] parts2;

    public List<carte> cartes = new List<carte>();
    public List<carte> cartesDisplay = new List<carte>();

    public GameObject canvas;

    public Image[] allSpots = new Image[8];
    public bool attackOn = true;
    public bool skillOn = true;
    public bool powerOn = true;
    public bool upgradeOn = false;
    public Slider slide;
    void Initialize()
    {
        string path = Application.dataPath + "/test.csv";
        parts = File.ReadAllLines(path);

        allSpots = canvas.GetComponentsInChildren<Image>();
    }

    void CreateAllCards()
    {
        for (int a = 1; a < parts.Length; a++)
        {
            parts2 = parts[a].Split(';');
            carte c = new carte(parts2[0], int.Parse(parts2[3]), int.Parse(parts2[4]), (rarity)int.Parse(parts2[2]), (type)int.Parse(parts2[1]));
            cartes.Add(c);
        }

        for (int i = 0; i < cartes.Count; i++)
        {
            cartes[i].Spriting();
        }
    }

    public Vector3[] posini;
    void Start()
    {
        Initialize();
        CreateAllCards();
        RecalculateDisplay();
        posini = new Vector3[ui.Length];
        for (int i = 0; i < ui.Length; i++)
        {
            posini[i] = ui[i].transform.position;
        }
    }

    public float vSliderValue = 0.0f;

    void OnGUI()
    {
        vSliderValue = GUI.VerticalSlider(new Rect(100, 100, 100, 800), vSliderValue, 0.0f, 45.0f);
    }
    public GameObject[] ui;
    void Update()
    {
        for (int i = 0; i < ui.Length; i++)
        {
            ui[i].transform.position = new Vector3(posini[i].x, posini[i].y + vSliderValue, posini[i].z);
        }
    }

    public void RecalculateDisplay()
    {
        cartesDisplay.Clear();


        if (attackOn)
        {
            for (int i = 0; i < cartes.Count; i++)
            {
                if (cartes[i].type == type.attack)
                {
                    cartesDisplay.Add(cartes[i]);
                }
            }
        }
        if (skillOn)
        {
            for (int i = 0; i < cartes.Count; i++)
            {
                if (cartes[i].type == type.skill)
                {
                    cartesDisplay.Add(cartes[i]);
                }
            }
        }
        if (powerOn)
        {
            for (int i = 0; i < cartes.Count; i++)
            {
                if (cartes[i].type == type.power)
                {
                    cartesDisplay.Add(cartes[i]);
                }
            }
        }

        DisplayChanges();
    }

    public void ChangeAttack()
    {
        if (attackOn)
            attackOn = false;
        else
            attackOn = true;

        RecalculateDisplay();
    }

    public void ChangeSkill()
    {
        if (skillOn)
            skillOn = false;
        else
            skillOn = true;

        RecalculateDisplay();
    }

    public void ChangePower()
    {
        if (powerOn)
            powerOn = false;
        else
            powerOn = true;

        RecalculateDisplay();
    }

    public void ChangeUpgrade()
    {
        if (upgradeOn)
            upgradeOn = false;
        else
            upgradeOn = true;

        RecalculateDisplay();
    }

    public void DisplayChanges()
    {
        for (int i = 0; i < allSpots.Length; i++)
        {
            if (i < cartesDisplay.Count)
            {
                allSpots[i].enabled = true;
                if (!upgradeOn)
                    allSpots[i].sprite = cartesDisplay[i].sprite;
                else
                    allSpots[i].sprite = cartesDisplay[i].sprite_U;
            }
            else
            {
                allSpots[i].sprite = null;
                allSpots[i].enabled = false;
            }
        }
    }
}
