using System.Collections;
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
    [SerializeField] private Renderer redLight;
    [SerializeField] private Renderer greenLight;
    [SerializeField] private Material normalLightMat;
    private Material greenLightMat;

    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;
    private float maxDistance;

    [SerializeField] private AudioSource keycardAccessTrue;

    void Awake()
    {
        collectibleTransform.gameObject.SetActive(false);

        maxDistance = Vector3.Distance(startPoint.position, endPoint.position);

        greenLightMat = greenLight.material;
        greenLight.material = normalLightMat;
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
        if(dist <= 0.25f)
        {
            collectibleTransform.position = endPoint.position;
            
            reachedPoint = true;
            keycardAccessTrue.Play();

            StartCoroutine(DoorOpenCoroutine());
            return;
        }
        
        collectibleTransform.position = Vector3.Lerp(endPoint.position, startPoint.position, dist);
    }

    private IEnumerator DoorOpenCoroutine()
    {
        greenLight.material = greenLightMat;
        redLight.material = normalLightMat;
        
        yield return new WaitForSeconds(0.3f);

        animator.SetBool("IsOpen", true);
        animator.SetTrigger("OpenDoor");

        yield return new WaitForSeconds(0.2f);

        StopInteraction();
    }

    public void OnEnter()
    {
        if(reachedPoint) return;
        
        animator.ResetTrigger("Close");
        animator.SetTrigger("Open");

        PlayerUI.Instance.ShowPrompt();
    }

    public void OnExit()
    {
        if(reachedPoint) return;

        animator.ResetTrigger("Open");
        animator.SetTrigger("Close");

        PlayerUI.Instance.HidePrompt();
    }

    public void OnInteract()
    {
        if(reachedPoint) return;
        
        inputting = !inputting;

        //This is temporary, will change it for a handler that just inmovilizes the player
        if (inputting)
        {
            DialogueManager.OnDialogueStarted?.Invoke(target);
            Cursor.lockState = CursorLockMode.None;
        }
        else StopInteraction();
    }

    private void StopInteraction()
    {
        DialogueManager.OnDialogueEnded?.Invoke();
        Cursor.lockState = CursorLockMode.Locked;
        collectibleTransform.gameObject.SetActive(false);

        PlayerUI.Instance.HidePrompt();
    }
}