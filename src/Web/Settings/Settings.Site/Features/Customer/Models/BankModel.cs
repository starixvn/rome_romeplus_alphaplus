namespace AlphaPlus.Settings.Site.Features.Customer.Models;
public class BankModel
{
    public BankModel(int id, string code, string name)
    {
        Id = id;
        Code = code;
        Name = name;
    }
    public int Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
}


