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

/*    [SerializeField]
    private UI_Manager uI_Manager_ = null;*/

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

    public void MouseLock(bool _lock, bool _visible)
    {
        if (_lock)
            // 마우스를 윈도우 정중앙 고정 후 보이지 않게 함
            Cursor.lockState = CursorLockMode.Locked;
        else
        {
            // 마우스를 창 밖으로 벗어나지 못하게 함
            Cursor.lockState = CursorLockMode.Confined;
            VisibleMouse(_visible);
        }
    }
    private void VisibleMouse(bool _visible)
    {
        Cursor.visible = _visible;
    }

}
