namespace BankConsole;

public abstract class Person{
    public abstract string GetName();

    public string GetCountru()
    {
        return "México";
    }
}


public interface IPerson
{
    string GetName();
    string GetCountry();
}