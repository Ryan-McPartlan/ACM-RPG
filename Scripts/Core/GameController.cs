using UnityEngine;

public class GameController : MonoBehaviour {

    public static GameController gameController;

    public bool paused;
    [SerializeField]
    GameObject pauseUI;
    [SerializeField]
    GameObject resourceBars;

    void Awake()
    {
        if(gameController == null)
        {
            gameController = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    //Flip visibility of resource bars and pause menue. Called when we push the pause button. Also set active ability to 0 and timescale to 0
    public void Pause()
    {
        paused = !paused;
        pauseUI.SetActive(paused); 
        resourceBars.SetActive(!paused); 
        PlayerController.playerController.ActiveAbility = -1; //When we pause, we clear active abilities
        if (paused)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
}
