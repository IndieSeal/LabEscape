using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : Singleton<PlayerUI>
{
    [SerializeField] private GameObject promptTxt;
    
    [Header("Crosshairs")]
    [SerializeField] private Image crosshairImg;
    [SerializeField] private Sprite usualCrosshair;
    [SerializeField] private Sprite hoverCrosshair;
    private Color usualColor;

    protected override void Awake()
    {
        base.Awake();
        usualColor = crosshairImg.color;
    }

    void OnEnable()
    {
        DialogueManager.OnDialogueStarted += HidePrompt;
        DialogueManager.OnDialogueEnded += ShowPrompt;

        CameraHandler.StartHoverOverInteractable += CrosshairHover;
        CameraHandler.EndHoverOverInteractable += CrosshairUsual;
    }

    void OnDisable()
    {
        DialogueManager.OnDialogueStarted -= HidePrompt;
        DialogueManager.OnDialogueEnded -= ShowPrompt;

        CameraHandler.StartHoverOverInteractable -= CrosshairHover;
        CameraHandler.EndHoverOverInteractable -= CrosshairUsual;
    }

    public void ShowPrompt() => promptTxt.SetActive(true);
    public void HidePrompt(DialogueManager.DialogueTarget t = null) => promptTxt.SetActive(false);

    private void CrosshairHover()
    {
        crosshairImg.sprite = hoverCrosshair;
        crosshairImg.color = Color.white;
    }

    private void CrosshairUsual()
    {
        crosshairImg.sprite = usualCrosshair;
        crosshairImg.color = usualColor;
    }
}