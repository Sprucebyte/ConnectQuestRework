using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using NaughtyAttributes;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine.EventSystems;

//[RequireComponent(typeof(Animator))]
//[RequireComponent(typeof(SpriteRenderer))]
public class Token : MonoBehaviour
{
    public enum State
    {
        OnBoard,
        InInventory,
        Other
    }
    public State state = State.OnBoard;

    [HorizontalLine(2f, EColor.Red)]
    [BoxGroup("Info")]
    [SerializeField] public string id = "Token ID";
    [BoxGroup("Info")]
    [SerializeField] private string displayName = "Name";
    [BoxGroup("Info")]
    [SerializeField] private string description = "Description";
    
    [HorizontalLine(2f, EColor.Blue)]
    [BoxGroup("Sprite")]
    [SerializeField] public Sprite sprite = null;
    [BoxGroup("Sprite")]
    [SerializeField] private new Animation animation = null;
    [BoxGroup("Sprite")]
    [SerializeField] private Animator animator = null;
    [BoxGroup("Sprite")]
    [SerializeField] private GameObject tokenRendererPrefab;
    private GameObject tokenRendererContainer;
    GameObject tokenRenderer;

    [HideInInspector] public Team team = null;
    [HideInInspector] public Board board = null;
    [HideInInspector] public Board.Slot slot = null;
    [HideInInspector] public bool moving;

    [HorizontalLine(2f, EColor.Green)]
    [BoxGroup("Activation Requirements")]
    [Expandable]
    [SerializeField] List<TokenActivationRequirement> activationAlternatives;
    [BoxGroup("Activation Requirements")]
    [SerializeField] private bool deleteSelfOnActivation = true;
    [BoxGroup("Activation Requirements")]
    [SerializeField] private bool deleteOthersOnActivation = true;
    Transform snapTo;
    Transform lastSnapTo;
    bool holdingToken = false;
    Vector2Int targetPosition;
    Quaternion startRotation;
    InputManager inputManager;
    public void SetTargetPosition(Vector2 pos)
    {
        targetPosition = new Vector2Int((int)pos.x, (int)pos.y);
    }

    private void Start()
    {
        if (state == State.Other) { 
            tokenRendererContainer = GameObject.Find("Token Renderer Container");
            tokenRenderer = Instantiate(tokenRendererPrefab,tokenRendererContainer.transform);
            tokenRenderer.GetComponent<SpriteRenderer>().sprite = sprite;
            tokenRenderer.GetComponent<TokenRenderer>().SetToken(this);
        }
        inputManager = FindFirstObjectByType<InputManager>();
        startRotation = transform.rotation;
    }
    private void Update()
    {
        switch (state)
        {
            case State.OnBoard: IsOnBoard(); break;
            case State.InInventory: IsInInventory(); break;
            default: Other(); break;
        }
       
    }

    TokenSnapTo GetClosestSnapTo(bool b = false)
    {
        TokenSnapTo[] snapToList = GameObject.Find("SnapTo").GetComponentsInChildren<TokenSnapTo>();
        IEnumerable<TokenSnapTo> sorted = snapToList.OrderBy(obj => (obj.transform.position - transform.position).sqrMagnitude);
        return sorted.First();
    }

    Transform GetClosestSnapTo()
    {
        Transform[] snapToList = GameObject.Find("SnapTo").GetComponentsInChildren<Transform>();
        IEnumerable<Transform> sorted = snapToList.OrderBy(obj => (obj.transform.position - transform.position).sqrMagnitude);
        return sorted.First();
    }


    void Other()
    {

        if (Input.GetMouseButtonDown(0)) holdingToken = !holdingToken; 
        if (holdingToken) {
            tokenRenderer.GetComponent<SpriteRenderer>().sortingOrder = 4;
            Vector3 currentTargetPos = inputManager.mousePositionWorldSpace;
            tokenRenderer.GetComponent<TokenRenderer>().scale = 1.2f;
            snapTo = GetClosestSnapTo();

            float dist = Vector2.Distance(inputManager.mousePositionWorldSpace, snapTo.position);
            if (dist <= 15)
            {
                //transform.position = Vector3.Lerp(snapTo.position, inputManager.mousePositionWorldSpace, (dist/30));
                //transform.position = snapTo.position;
                transform.position = new Vector3((snapTo.position.x + inputManager.mousePositionWorldSpace.x) / 2, (snapTo.position.y + inputManager.mousePositionWorldSpace.y) / 2,transform.position.z);
                lastSnapTo = snapTo;
            }
            else
            {
                transform.position = inputManager.mousePositionWorldSpace;
            }
        }
        else
        {
            tokenRenderer.GetComponent<SpriteRenderer>().sortingOrder = 2;
            tokenRenderer.GetComponent<TokenRenderer>().scale = 1f;
            if (lastSnapTo) transform.position = lastSnapTo.position;
        }

    }

    private void IsOnBoard()
    {
        moving = (transform.position.y != targetPosition.y);
        if (moving)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(targetPosition.x, targetPosition.y, transform.position.z), .1f);
        }
    }

    private void IsInInventory()
    {
        transform.position = Vector3.Lerp(transform.position, new Vector3(targetPosition.x, targetPosition.y, transform.position.z), .1f);
        /*
        mouseSpeed = Mathf.Lerp(mouseSpeed, inputManager.mouseSpeed.x, .5f);
        Vector3 newPosition = targetPosition + inputManager.mouseSpeed;
        float a = Mathf.Min(inputManager.mouseSpeed.x, 1);
        float b = Mathf.Min(inputManager.mouseSpeed.y, 1);
        newPosition = new Vector2(targetPosition.x + a, targetPosition.y + b);
        //transform.position = Vector3.Lerp(transform.position, new Vector3(newPosition.x, newPosition.y, transform.position.z), .3f);

        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, -mouseSpeed* 5));

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, startRotation, 5f * Time.deltaTime);
        */

    }

    public bool Hover(bool hover)
    {
        return hover;
    }


    public Sprite GetSprite()
    {
        return sprite;
    }

    public bool ActivationCheck(out List<Board.Slot> slots)
    {
        
        slots = new List<Board.Slot>();
        foreach (TokenActivationRequirement requirementAlternatives in activationAlternatives)
        {
            
            if (requirementAlternatives.IsMet(board, slot, out slots)) {
                if (!deleteOthersOnActivation) slots.Clear();
                if (deleteSelfOnActivation) slots.Add(slot);
                return true; 
            }
        }
        return false;
    }








}

