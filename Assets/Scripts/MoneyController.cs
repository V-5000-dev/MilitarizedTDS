using UnityEngine;

public class MoneyController : MonoBehaviour
{
    public int money;
    public void AddMoney(int m)
    {
        money += m;
    }
    public void RemoveMoney(int m)
    {
        money -= m;
    }

}
