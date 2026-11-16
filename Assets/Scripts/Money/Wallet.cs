
using System;

public class Wallet
{
    
    public int currentMoney {get; private set;}=0;

    private int MaxMoney=9999;

    public event Action<int> MoneyChange;


    private void NotifyChange()
    {
        MoneyChange?.Invoke(currentMoney);
        
    }
    public bool SpendMoney(int money)
    {   
        if(money>currentMoney||money<=0)return false;

        currentMoney-=money;
        NotifyChange();

        return true; 
    }

    public bool GainMoney(int money)
    {   
        if(currentMoney>=MaxMoney||money<=0)return false;

        int moneyToFill=MaxMoney-currentMoney;

        currentMoney+=Math.Min(money,moneyToFill);
        
        NotifyChange();

        return true; 

        
    }

}