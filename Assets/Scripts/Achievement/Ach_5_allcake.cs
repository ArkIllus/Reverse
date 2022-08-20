using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Ach_5_allcake", menuName = "AchievementData/Ach_5_allcake")]
public class Ach_5_allcake : Achievement_SO
{
    public List<E_cake> cakes = new List<E_cake>();
    public List<bool> cakesGet = new List<bool>();

    public void UpdateMe(E_cake e_Cake)
    {
        for (int i = 0; i < cakes.Count; i++)
        {
            if (e_Cake == cakes[i] && cakesGet[i] == false)
            {
                cakesGet[i] = true;
                return;
            }
        }
    }
}
