using System.ComponentModel;

namespace PRN231.API.Enums;

public enum RoleEnum
{
    [Description("ADMIN")] ADMIN,
    [Description("STORE_MANAGER")] STORE_MANAGER,
    [Description("CASHIER")] CASHIER
}