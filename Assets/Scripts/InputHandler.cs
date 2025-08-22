using UnityEngine;

public class InputHandler : MonoBehaviour
{
    [SerializeField] private InputSystem_Actions playerInputActions;
    [SerializeField] private UIController UIController;
    [SerializeField] private AimSlowdownManager aimSlowdownManager;

    private bool isAiming = false;
    private bool wasAiming = false;

    //Instantiate and enabled Player Input Actions
    void Start()
    {
        playerInputActions = new InputSystem_Actions();
        playerInputActions.Enable();
    }

    void Update()
    {
        var input = playerInputActions.Player;

        if (input.Pause.WasPressedThisFrame())
        {
            HandlePause();
        }

        //Don't process aim inputs if the game is paused
        //if (TimeScaleManagerOLD.Instance.IsPaused)
        //    return;

        isAiming = input.Aim.IsPressed();
        HandleAimingSlowdown();
    }

    private void HandleAimingSlowdown()
    {
        //Player just started aiming
        if (isAiming && !wasAiming)             
        {
            aimSlowdownManager.EnableAimingSlowMo();
        }
        //Player just stopped aiming
        else if (!isAiming && wasAiming)        
        {
            aimSlowdownManager.DisableAimingSlowMo();
        }

        wasAiming = isAiming;
    }

    //Handles pause inputs based on whether the game is already paused or not
    private void HandlePause()
    {
        //if (!TimeScaleManagerOLD.Instance.IsPaused)
        //{
        //    UIController.ShowPauseText(true);
        //    TimeScaleManagerOLD.Instance.PauseTime();
        //}
        //else if (TimeScaleManagerOLD.Instance.IsPaused)
        //{
        //    UIController.ShowPauseText(false);
        //    TimeScaleManagerOLD.Instance.UnpauseTime();
        //}
    }
}
