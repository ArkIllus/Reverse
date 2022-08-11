using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager_global : BaseManager<GameManager_global>
{
    public GameData_SO gameData_SO;
    //public SceneData_SO sceneData_SO; //no need

    public GameManager_global()
    {
        gameData_SO = ResourceManager.GetInstance().Load<GameData_SO>("GameData/GameData_SO");
    }
}
