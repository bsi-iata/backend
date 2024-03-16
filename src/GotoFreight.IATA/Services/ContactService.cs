using GotoFreight.IATA.Models.Dto;

namespace GotoFreight.IATA.Services;

public class ContactService
{
    public ContactDto GetDefault()
    {
        return new ContactDto
        {
            Name = "bob",
            Phone = "13600660088",
            Email = "13600660088@gmail.com",
            Contry = "china",
            State = "guangdong",
            City = "shenzhen",
            Zip = "test",
            Address = "nan shan"
        };
    }
}