using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class KeycardDoor : MonoBehaviour, IInteractable
{
    protected PlayerInputHandler PInput => PlayerInputHandler.Instance;
    
    [SerializeField] private Animator animator;
    [SerializeField] private DialogueManager.DialogueTarget target;
    private bool inputting = false;
    private RaycastHit? cardHit;
    private bool reachedPoint = false;

    [SerializeField] private Transform collectibleTransform;
    [SerializeField] private ECollectible requiredCollectible;

    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;
    private float maxDistance;

    [SerializeField] private AudioSource keycardAccessTrue;

    void Awake()
    {
        collectibleTransform.gameObject.SetActive(false);

        maxDistance = Vector3.Distance(startPoint.position, endPoint.position);
    }

    void Update()
    {
        cardHit = null;

        if(!inputting || reachedPoint || !GameManager.Instance.ContainsCollectible(requiredCollectible)) return;

        collectibleTransform.gameObject.SetActive(true);
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        List<RaycastHit> raycastHits = Physics.RaycastAll(ray, 100f).ToList();
        if(Input.GetMouseButton(0) && raycastHits.Count != 0)
        {
            foreach(RaycastHit hit in raycastHits)
            {
                if(hit.transform == collectibleTransform) cardHit = hit;
            }
        }

        if(cardHit == null) return;

        float dist = Vector3.Distance(endPoint.position, cardHit.Value.point) / maxDistance;
        Debug.Log(dist);
        
        if(dist <= 0.25f)
        {
            reachedPoint = true;
            keycardAccessTrue.Play();

            animator.SetBool("IsOpen", true);
            animator.SetTrigger("OpenDoor");
            return;
        }
        
        collectibleTransform.position = Vector3.Lerp(endPoint.position, startPoint.position, Vector3.Distance(endPoint.position, cardHit.Value.point) / maxDistance);
    }

    public void OnEnter()
    {
        animator.ResetTrigger("Close");
        animator.SetTrigger("Open");

        PlayerUI.Instance.ShowPrompt();
    }

    public void OnExit()
    {
        animator.ResetTrigger("Open");
        animator.SetTrigger("Close");

        PlayerUI.Instance.HidePrompt();
    }

    public void OnInteract()
    {
        inputting = !inputting;

        //This is temporary, will change it for a handler that just inmovilizes the player
        if (inputting)
        {
            DialogueManager.OnDialogueStarted?.Invoke(target);
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            DialogueManager.OnDialogueEnded?.Invoke();
            Cursor.lockState = CursorLockMode.Locked;
            collectibleTransform.gameObject.SetActive(false);
        }
    }
}