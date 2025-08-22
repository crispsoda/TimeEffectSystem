using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button button_S;
    [SerializeField] private Button button_M;
    [SerializeField] private Button button_L;
    [SerializeField] private Button button_XL;
    [SerializeField] private Button button_Quit;

    [Header("UI Text")]
    [SerializeField] private TMP_Text activeTimeScaleText;
    [SerializeField] private TMP_Text realTimeScaleText;
    [SerializeField] private TMP_Text pausedText;

    [SerializeField] private TimeEffectCaller caller;

    void Start()
    {
        pausedText.enabled = false;

        button_S.onClick.AddListener(() => RequestEffect("testA"));
        button_M.onClick.AddListener(() => RequestEffect("testB"));
        button_L.onClick.AddListener(() => RequestEffect("testC"));
        button_XL.onClick.AddListener(() => RequestEffect("testD"));
        button_Quit.onClick.AddListener(() => QuitGame());

        caller = GetComponent<TimeEffectCaller>();
    }

    void Update()
    {
        //activeTimeScaleText.text = $"Active Time Scale = {TimeScaleManagerOLD.Instance.ActiveTimeScale}";
        realTimeScaleText.text = $"Real Time Scale = {Time.timeScale}";
    }

    public void ShowPauseText(bool show)
    {
        pausedText.enabled = show;
    }

    private void RequestEffect(string effectName)
    {
        //if (TimeScaleManagerOLD.Instance.IsPaused)
        //    return;

        if(caller != null)
        {
            caller.AddEffect(effectName);
        }
        else
        {
            Debug.LogWarning("No instance of HitstopMaanger found!");
        }
    }

    private void QuitGame()
    {
        Application.Quit();
    }
}
