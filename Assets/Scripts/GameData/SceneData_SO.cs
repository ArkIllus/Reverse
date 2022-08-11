using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SceneData", menuName = "new SceneData_SO")]
public class SceneData_SO : ScriptableObject
{
    [Header("��������")]
    public const string initScene = "InitScene";
    public const string externalScene = "ExternalScene";
}
