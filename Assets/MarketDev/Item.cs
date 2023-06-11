using System;
using System.Linq;

public enum ItemType
{
    Apple,
    Banana,
    Broccoli,
    Carrot,
    Coconut,
    Corn,
    Eggplant,
    Leek, 
    Milk,
    Orange,
    Pumpkin,
    Pepper,
    RiceBall,
    Salad,
    CuttingBoard,
    FryingPan,
    Pan,
    Egg,


}



public class Item : Interactable
{
    public ItemType type;

    public Cart cart;
    public Player player;

    public void Start()
    {
        player = FindObjectOfType<Player>();
        cart = FindObjectOfType<Cart>();
    }
    public override void Interact()
    {
        base.Interact();

        var gmr = GameManager.Instance;
        if (gmr.itemPool.ContainsKey(type))
        {
            if (gmr.itemPool[type] == 0) return;

            if (gmr.itemPool[type] == 1)
            {
                player.iscompletedErrand[Array.IndexOf(gmr.itemPool.Keys.ToArray(), type)] = true;

            }
            cart.AddItem(gameObject);
            gmr.itemPool[type]--;
        }

    }

}