using UnityEngine;
using UnityEngine.UI;

namespace Tsiunas.Mechanics
{
    public class UIMoneyController : MonoBehaviour
    {
        public Text textAmountMoney;

        private void Awake()
        {
            GameManager.Instance.moneyAmountChange += MoneyAmountChange;
        }

        private void OnDestroy()
        {
            if(GameManager.Instance != null)
                GameManager.Instance.moneyAmountChange -= MoneyAmountChange;
        }

        void MoneyAmountChange(int currentMoneyAmount)
        {
            textAmountMoney.text = currentMoneyAmount.ToString();
        }
    }
}
