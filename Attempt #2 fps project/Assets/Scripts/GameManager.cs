using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject menuActive = null;
    [SerializeField] private GameObject menuPause = null;
    [SerializeField] private GameObject menuWin, menuLoss = null;

    public GameObject damageFlash = null;
    public bool isPaused = false;
    public GameObject player = null;
    public playerController playerController = null;

    private float timeScaleOriginal = 1.0f;

    public static GameManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        timeScaleOriginal = Time.timeScale;
        player = GameObject.FindWithTag("Player");
        playerController = player.GetComponent<playerController>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (menuActive == null)
            {
                StatePaused();
                menuActive = menuPause;
                menuActive.SetActive(true);
            }
            else if (menuActive == menuPause)
            {
                StateUnpaused();
            }
        }
    }

    public void StatePaused()
    {
        isPaused = !isPaused;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void StateUnpaused()
    {
        isPaused = !isPaused;
        Time.timeScale = timeScaleOriginal;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        menuActive.SetActive(false);
        menuActive = null;
    }

    public void Lose()
    {
        StatePaused();
        menuActive = menuLoss;
        menuActive.SetActive(true);
    }
   
}
