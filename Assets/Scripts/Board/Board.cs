//#define DEBUG
using NaughtyAttributes;

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using static Board;
using static UnityEngine.GraphicsBuffer;
using static Util;



public class Board : MonoBehaviour
{

    public class Slot
    {
        public Slot(int x,  int y)
        {
            this.x = x; this.y = y;
        }

        public bool locked = false;
        public GameObject obj = null;
        public int x = 0;
        public int y = 0;
        public bool IsFilled() { return (obj != null); }
        public void Set(GameObject obj)
        {
            this.obj = obj;
        }
        public void Clear()
        {
            obj = null;
        }

        public GameObject Get()
        {
            return obj;
        }

    }

    InputManager inputManager;
    public Inventory playerInventory;

    [HorizontalLine(2f, EColor.Red)]
    [BoxGroup("Board size")]
    [MinValue(0), MaxValue(40)]
    [SerializeField] public Vector2Int boardSize = new Vector2Int(7, 6);
    [BoxGroup("Board size")]
    [MinValue(0), MaxValue(100)]
    [SerializeField] Vector2Int slotSize = new Vector2Int(22, 21);
    [BoxGroup("Board size")]
    [SerializeField] Vector2Int slotCentreOffset = new Vector2Int(1, 0);

    [HorizontalLine(2f, EColor.Blue)]

    [SerializeField, Required, BoxGroup("Objects")] GameObject tokenContainer;
    [SerializeField, Required, BoxGroup("Objects")] GameObject testToken;
    [SerializeField, Required, BoxGroup("Objects")] GameObject enemyToken;
    [SerializeField, Required, BoxGroup("Objects")] Transform slotOrigin;

    [HorizontalLine(color: EColor.Gray)]


    private Team team1 = new Team();
    private Team team2 = new Team();
    public Slot[,] slots;
    int slotCount;

    bool wait = false;
    void StopWait()
    {
        wait = false;
    }
    void Wait(int seconds = 2)
    {
        wait = true;
        Invoke("StopWait", seconds);
    }
    void EnemyPlaceToken()
    {
        int x = Random.Range(0, boardSize.x);
        while (!TryPlaceToken(x, enemyToken,team2))
        {
            x = Random.Range(0, boardSize.x);
        }
    }

    void Start()
    {
        inputManager = FindFirstObjectByType<InputManager>();
        slotCount = boardSize.x * boardSize.y;
        slots = new Slot[boardSize.x, boardSize.y];

        // Replace with LINQ when you have spare time
        for (int x = 0; x < boardSize.x; x++)
        {
            for (int y = 0; y < boardSize.y; y++)
            {
                slots[x,y] = new Slot(x,y);
            }
        }
    }

    private void Update()
    {

        if (Input.GetMouseButtonDown(0) && !wait)
        {
            Slot slot = GetSlotAtMousePosition();
            if (slot == null) return;
            TryPlaceToken(slot.x, playerInventory.selectedToken.gameObject,team1);
            Invoke("EnemyPlaceToken", 1);
            Wait();
        }
        if (Input.GetMouseButtonDown(1))
        {
            Slot slot = GetSlotAtMousePosition();
            if (slot == null) return;
            RemoveToken(slot);
        }
    }

    [Button]
    private void CheckTokens()
    {
        List<Slot> connectedSlots = new List<Slot>();
        for (int y = boardSize.y - 1; y >= 0; y--)
        {
            for (int x = 0; x < boardSize.x; x++)
            {
                Slot slot = slots[x, y];
                if (slot.obj != null) { 
                    if (slot.obj.GetComponent<Token>().ActivationCheck(out connectedSlots)) { 
                        RemoveTokens(connectedSlots);
                    }

                }
            }
        }
    }


    public bool CheckIfTokensAreMoving()
    {
        foreach (Slot slot in slots)
        {
            if (slot.obj.GetComponent<Token>().moving) return true;
        }
        return false;
    }


    public void TryPlaceTokenInSlot(int x, int y, GameObject token)
    {
        GameObject t = Instantiate(token,tokenContainer.transform);
        slots[x, y].Set(t);
        Vector2 pos = GetWorldPosition(x, y);
        slots[x, y].obj.transform.position = new Vector3(pos.x, pos.y,transform.position.z);
    }

    public bool TryPlaceToken(int x, GameObject token,Team team)
    {
        // target = 0;
        int target_y = 0;   
        if (slots[x, 0].Get() != null) return false;
        for (int y = 1; y < boardSize.y; y++) 
        {
            if (slots[x, y].Get() == null) target_y = y;
            else break;
        }

        GameObject t = Instantiate(token, tokenContainer.transform);
        slots[x, target_y].Set(t);
        Vector2 pos = GetWorldPosition(x, 0);
        slots[x, target_y].obj.transform.position = new Vector3(pos.x, pos.y+40, transform.position.z);
        slots[x, target_y].obj.GetComponent<Token>().SetTargetPosition(GetWorldPosition(x,target_y));
        slots[x, target_y].obj.GetComponent<Token>().board = this;
        slots[x, target_y].obj.GetComponent<Token>().slot = slots[x,target_y];
        slots[x, target_y].obj.GetComponent<Token>().team = team;
        Invoke("CheckTokens",.5f);
        return true;
    }

    public Vector2 GetWorldPosition(int x, int y)
    {
        return new Vector2(slotOrigin.position.x + x*slotSize.x + slotCentreOffset.x, slotOrigin.position.y + -y*slotSize.y + slotCentreOffset.y);
    }

    public Slot GetSlot(int x, int y)
    {
        return slots[x, y];
    }

    #region Token Removal
    public void RemoveToken(int x, int y)
    {
        Slot slot = slots[x, y];
        if (slot.IsFilled())
        {
            Destroy(slot.obj);
            slot.Clear();
            UpdateColumn(x);
        }
    }
    public void RemoveToken(Slot slot)
    {
        if (slot.IsFilled())
        {
            Destroy(slot.obj);
            slot.Clear();
            UpdateColumn(slot.x);
        }
    }


    public void RemoveTokens(List<Slot> tokensToRemove)
    {
        foreach (Slot slot in tokensToRemove)
        {
            if (slot.IsFilled())
            {
                Destroy(slot.obj);
                slot.Clear();
            }
        }
        UpdateAllSlots();
        tokensToRemove.Clear();
    }
    #endregion

    #region Update Slots
    public void UpdateAllSlots()
    {
        for (int y = boardSize.y - 1; y >= 0; y--)
        {
            for (int x = 0; x < boardSize.x; x++)
            {
                UpdateSlot(slots[x, y]);
            }
        }

    }

    public void UpdateColumn(int x)
    {
        for (int y = boardSize.y - 1; y >= 0; y--)
        {
            UpdateSlot(slots[x, y]);
        }
    }
    

    public void UpdateSlot(Slot slot)
    {
        int target_y = -1;
        if (slot.y + 1 >= boardSize.y) return;
        if (slots[slot.x, slot.y + 1].Get() != null) return;
        target_y = slot.y+1;
        
        for (int y = slot.y+2; y < boardSize.y; y++)
        {
            if (slots[slot.x, y].Get() == null) target_y = y;
            else break;
        }
        if (target_y == -1 || target_y == slot.y) return; 
        MoveToken((slot.x, slot.y), (slot.x, target_y));
    }
    #endregion

    #region Move Tokens
    public void MoveToken((int x, int y) fromSlot, (int x, int y) toSlot)
    {
        Slot slot = slots[fromSlot.x, fromSlot.y];
        GameObject token = slot.Get();
        if (token == null) return;
        //if (toSlot.y > boardSize.y) { RemoveToken(fromSlot.x, fromSlot.y); return; }
        slots[toSlot.x, toSlot.y].Set(token);
        token.GetComponent<Token>().SetTargetPosition(GetWorldPosition(toSlot.x, toSlot.y));
        token.GetComponent<Token>().board = this;
        token.GetComponent<Token>().slot = slots[toSlot.x,toSlot.y];
        slot.Clear();
    }

    [Button]
    public void MoveAllTokensDown()
    {
        // Go trough each row from the bottom
        for (int y = boardSize.y - 1; y >= 0; y--)
        {
            for (int x = 0; x < boardSize.x; x++)
            {
                if (y+1 == boardSize.y) RemoveToken(x, y); 
            }
        }
    }
    #endregion

    [Button]
    public void ClearUnusedTokens()
    {
        int tokenCount = tokenContainer.transform.childCount;

        for (int i = 0; i < tokenCount; i++)
        {
            GameObject token = tokenContainer.transform.GetChild(i).gameObject;
            if (!TokenIsInBoard(token)) Destroy(token);
        }
    }

    public bool TokenIsInBoard(GameObject gameObjectToFind)
    {
        foreach (Slot slot in slots)
        {
            if (slot != null && slot.obj == gameObjectToFind)
            {
                return true;
            }
        }
        return false;
    }

    #region Get Slots 
    public bool IsSlotAtPosition((int x, int y) slotIndex, (float x, float y) position)
    {
        Vector2 worldPos = GetWorldPosition(slotIndex.x, slotIndex.y);
        return (IsValueBetween(position.x, worldPos.x - slotSize.x / 2, worldPos.x + slotSize.x / 2) &&
               (IsValueBetween(position.y, worldPos.y - slotSize.y / 2, worldPos.y + slotSize.y / 2)));
    }

    public Slot GetSlotAtMousePosition()
    {
        for (int x = 0; x < boardSize.x; x++)
        {
            for (int y = 0; y < boardSize.y; y++)
            {
                if (IsSlotAtPosition((x, y), (inputManager.mousePositionWorldSpace.x, inputManager.mousePositionWorldSpace.y)))
                    return slots[x,y];
            }
        }
        return null;
    }
    #endregion




#if DEBUG
    private void OnDrawGizmos()
    {

        for (int x = 0; x < boardSize.x; x++)
        {
            for(int y = 0; y < boardSize.y; y++)
            {
                Gizmos.color = Color.blue;
                Vector2 worldPos = GetWorldPosition(x, y);
                if (inputManager != null && IsSlotAtPosition((x,y), (inputManager.mousePositionWorldSpace.x, inputManager.mousePositionWorldSpace.y)))  { 
                    Gizmos.color = Color.red;
                }
                DrawCross(GetWorldPosition(x,y), 2);
            }
        }

    }
#endif
}

