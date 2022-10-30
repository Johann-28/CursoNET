namespace BankConsole;

public class Client : User, IPerson
{

    public char TaxRegime { get; set; }

    public Client(){}

    public Client(int id, string name, string email, decimal balance, char TaxRegime) : base(id, name, email, balance)
    {
        this.TaxRegime = TaxRegime;
        SetBalance(balance);
    }

    public override void SetBalance(decimal amount)
    {
        base.SetBalance(amount);

        if(TaxRegime.Equals('M'))
            Balance += (amount * 0.02m);
    }

    public override string ShowData()
    {
        return base.ShowData() + $", Régimen fiscal {this.TaxRegime}";

    }

    public string GetName()
    {
        return Name;
    }

    public string GetCountry()
    {
        throw new NotImplementedException();
    }
}