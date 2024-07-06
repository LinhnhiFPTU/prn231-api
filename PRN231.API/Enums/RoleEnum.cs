using System.ComponentModel;

namespace PRN231.API.Enums;

public enum RoleEnum
{
    [Description("ADMIN")] ADMIN,
    [Description("STAFF")] STAFF,
    [Description("CUSTOMER")] CUSTOMER
}