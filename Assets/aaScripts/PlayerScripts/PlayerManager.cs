using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;
    public EquipmentContent equipmentContent;
    [SerializeField] GameObject PlayerSprite;

    public CharacterStats characterStats;
    public BaseStats baseStats;
    [SerializeField] PlayerPosition playerPosition;
    public UnityEvent interactionEvents;

    public Vector2 PlayerMovement = Vector2.zero;

    public bool PlayerCanMove = true;

    public GameObject RadialEmotesMenu;
    public GameObject RadialToolsMenu;

    public enum QuestStates { DONE, DOING, TODO } // THE QUEST IS CURRENTLY DONE, BEING DONE OR NEVER STARTED
    public Dictionary<QuestInfo, QuestStates> QuestProgression; // THIS DICTIONARY STORES A QUEST AND ITS CURRENT STATE


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        PlayerSprite.GetComponent<SpriteRenderer>().sprite = characterStats.sprite;
        // wtf is this transform.position = playerPosition.Position;
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (PlayerCanMove)
        {
            Debug.Log("checking movement method. PlayerCanMove: " + PlayerCanMove);
            PlayerMovement = context.ReadValue<Vector2>();
        }
    }
    public void Attack(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            // If the player is in Attack state, means that the attack is not over yet, so do not start another attack until the current fininshes
            if (StateController.Instance.currentState != StateController.Instance.mouseAttackState)
                StateController.Instance.ChangeState(StateController.Instance.mouseAttackState);
        }

    }

    public void ToggleEmotesRadialMenu(InputAction.CallbackContext context)
    {
        if (context.performed)
            RadialEmotesMenu.SetActive(true);

        if (context.canceled)
            RadialEmotesMenu.SetActive(false);
    }
    public void ToggleToolsRadialMenu(InputAction.CallbackContext context)
    {
        if (context.performed)
            RadialToolsMenu.SetActive(true);

        if (context.canceled)
            RadialToolsMenu.SetActive(false);
    }

    public void addQuest(QuestInfo quest, QuestStates questState)
    {
        this.QuestProgression.Add(quest, questState);
    }


    // MOVEMENT

    public void Dash(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            AbilityManager.Instance.ActivateAbility("Dash", characterStats);
        }
    }

    private void FixedUpdate()
    {

    }
    // END MOVEMENT
    public void Interact()
    {
        if (interactionEvents != null)
        {
            interactionEvents.Invoke();
        }
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.transform.tag == "Mision")
        {
            Quest q = collision.GetComponent<Quest>();
            q.showQuest();
        }

        if (collision.transform.tag == "Item")
        {
            collision.GetComponent<ItemScript>().TakeItem();
        }

        if (collision.transform.tag == "DungeonTrigger")
        {
            SceneManager.LoadScene("Dungeon");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.tag == "Mision")
        {
            Quest q = collision.GetComponent<Quest>();
            q.closeQuest();
        }
    }

    public int getAttackDmg()
    {
        int attackDmg = 0;
            attackDmg = characterStats.dmg + Equipment.Instance.getEquipmentDmg();
        print("ATTACK DMG: " + attackDmg);
        return attackDmg;
    }

    public void levelUp()
    {
        characterStats.level++;
    }






}
