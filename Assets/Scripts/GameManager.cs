using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject outGameObjectsGo_ = null;
    [SerializeField]
    private GameObject inGameObjectsGo_ = null;
    [SerializeField]
    private GameObject inGameUiGo_ = null;

    [SerializeField]
    private UI_Manager uI_Manager_ = null;

    public bool isStartGame_ { get; set; }
    public bool isInGame_ { get; set; }
    private void Awake()
    {
        inGameObjectsGo_.SetActive(false);
        isStartGame_ = false;
        isInGame_ = false;
    }
    public void ActiveOutGameObjects()
    {
        inGameObjectsGo_.SetActive(false);
        outGameObjectsGo_.SetActive(true);
        isInGame_ = false;
    }
    public void ActiveInGameObjects()
    {
        outGameObjectsGo_.SetActive(false);
        inGameObjectsGo_.SetActive(true);
        isInGame_ = true;
        isStartGame_ = true;
    }
    public void ActiveInGameUi()
    {
        outGameObjectsGo_.SetActive(false);
        inGameUiGo_.SetActive(true);
        isInGame_ = true;
    }
    public void ActiveOutGameUi()
    {
        inGameUiGo_.SetActive(false);
        outGameObjectsGo_.SetActive(true);
        isInGame_ = false;
    }
} // end of class
