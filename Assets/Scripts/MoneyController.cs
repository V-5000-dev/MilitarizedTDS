using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MoneyController : MonoBehaviour
{
    public TextMeshProUGUI moneyText;
    public static TextMeshProUGUI moneyWarningText;
    public static int money = 2000;
    public static void AddMoney(int m)
    {
        money += m;
    }
    public static void RemoveMoney(int m)
    {
        money -= m;
        Debug.Log(money);
    }
    public static IEnumerator NotEnoughMoney()
    {
        moneyWarningText.enabled = true;
        yield return new WaitForSeconds(1f);
        moneyWarningText.enabled = false;
    }
    public void Start()
    {
        moneyWarningText = GameObject.Find("MoneyWarningText").GetComponent<TextMeshProUGUI>();
        moneyWarningText.enabled = false;
    }
    public void Update()
    {
        moneyText.text = "Money: " + money.ToString();
    } 

}
