using UnityEngine;

public class TODO : MonoBehaviour {/*
    
    //Movespeed: Movespeed constant ensures that movespeed is in tile/second. Find this value, set it, and divide the "movespeed" stat by it when displaying it




    ITEMS: Add items to the game:
        -Items drop and are added to tiles, displaying their sprite
        -Items are picked up when the player touches them
        -Items can only be dragged into slots or add abilities to the item abilty list while in the inventory
        -Slots determine what items they can take, and what items dropped in them do
        -Items hold important values needed by slots
        -Items can be dragged, and will move back to place on their own
        -A tooltip will display when you touch the item
        -Items will have a function to build the tooltip

    EQUIPTMENT: An important item type
        - All will be handled the same exact way, but only the right items can be put in slots
        - Start off with placeholder slots 1-9 and armor types 1-3 and 3 slots for each type, this should test everything
        - Each adds attributes to the character. Some will add passives or abilities.
        - PlayerController manages armor, and the benefits of armor go to main player

    ABILITIES: Update the ability UI
        Assigning:
            - Make the ability assigner find abilities on the player unit, the abilities on their weapon/equiptment, the abilities on their items
            - When found, it generates new "items" for its slots. When they are removed, their corresponding items go too.
            - Create some form of UI object with a referance to each ability, which can be dragged to the 6 slots
            - Make the slots assign the ability referanced by the item to the slots
            - Pausing the game should clear active ability
        UI: 
            - Indicator for active ability
            - Indicator for cooldowns

    UI: Add a "console" for ingame messages that we can print to

    Map: 
        - Lay out tiles by hand, or make some other good way to do this. 
        - Make more tiles
    
    NPCs:
        -Make non-moving NPCs
        -NPC healthbars and names
        -Give them abilities

    Combat:
        Damage numbers appear
        Word "resist" appears when an effect is resisted
        Player gains exp, exp numbers appear

    Level Up:
        Add a level Up manager, works closely with playercontroller
        When we have exp, we can push a button to increase level.
        Level up manager calculates projected stats and displays them in the UI
        When we confirm, we lock it in.
*/

/*
    IDEAS

    Character effects:
        Root, Stun, On Fire, Speed boosted, 

    Abilities:
        Resource Builder: These abilities can be used very often, but are pure stink
        Resource Dump: These abilities consume your resource to deal more damage
        Ultimate: These abilities make you much stronger for a short time. Limited to one.
            - Pin ability: Fire a penetrating projectile that roots all enemies hit if it hits a wall

        Survival: Medium costs, mitigate damage or restore health
            - Invincible for 1 second
            - Shoot a field/tile effect that drastically slows projectile speed

        Mobility: Medium costs, help you get around
            - Speed boost
            - 

    Armor:
        Berzerker: Benefits for low HP. Get low safely, stay low longer, gain advantages while low
            - When killed, set HP to 1 and become invincible and deal 50% more damage for some time. Some CD.
            - When below 30% health, gain X% lifesteal and all of lifesteal now applies a shield
            - Gain something scaling with missing health
        Random
            - Helm of the blind monk: Reduce vision range dramatically. You deal double damage to enemies you cannot see
            - 


*/
}
