using Jflutter.Entities;

namespace Jflutter.Services.DataAccess;

public static class FakePeopleInfo
{
    public static ICollection<UserInfo> UserInfoDB = new List<UserInfo>()
    {
        new()
        {
            Firstname = "Ali",
            Lastname = "Ahmed",
            PersonalCode = 11111111,
        },
        new()
        {
            Firstname = "Fatemeh",
            Lastname = "Salehi",
            PersonalCode = 22222222
        },
        new()
        {
            Firstname = "Sara",
            Lastname = "Momeni",
            PersonalCode = 33333333,
            G1 = 19,
            G2 = 17,
            G3 = 18,
            G4 = 19,
            LabAttendence = 4,
            ClassAttendence = 0
        },
        new()
        {
            Firstname = "Mohamad",
            Lastname = "Soltan",
            PersonalCode = 44444444,
            G1 = 13,
            G2 = 15,
            G3 = 20,
            G4 = 15.5,
            LabAttendence = 5,
            ClassAttendence = 0
        },
        new()
        {
            Firstname = "Yaser",
            Lastname = "Naser",
            PersonalCode = 55555555,
            G1 = 10,
            G2 = 10,
            G3 = 12,
            G4 = 12.5,
            LabAttendence = 2,
            ClassAttendence = 0
        },
    };
}