using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Authorization;

namespace FilmUpMovie.Authorization
{
    // Centralized Constants Class
    public static class Constants
    {
        // Cinema Roles
        public const string CinemaAdministratorsRole = "CinemaAdministrator";
        public const string CinemaManagerRole = "CinemaManager";

        // Movie Roles
        public const string MovieAdministratorsRole = "MovieAdministrator";
        public const string MovieManagersRole = "MovieManager";

        // ShowRoom Roles
        public const string ShowRoomAdministratorsRole = "ShowRoomAdministrator";
        public const string ShowRoomManagersRole = "ShowRoomManager";

        // ShowTime Roles
        public const string ShowTimeAdministratorsRole = "ShowTimeAdministrator";
        public const string ShowTimeManagersRole = "ShowTimeManager";

        // HR Roles
        public const string AdminRole = "Administrator";
        public const string ManagerRole = "Manager";

        // Food & Beverage Roles
        public const string FoodAdministratorsRole = "FoodAdministrators";
        public const string FoodManagerRole = "FoodManager";
        public const string FoodOwnerRole = "FoodOwner";

        public const string BeverageAdministratorsRole = "BeverageAdministrators";
        public const string BeverageManagerRole = "BeverageManager";

        public const string ComboAdministratorsRole = "ComboAdministrators";
        public const string ComboManagerRole = "ComboManager";


        // Operation Names
        public const string CreateOperationName = "Create";
        public const string ReadOperationName = "Read";
        public const string UpdateOperationName = "Update";
        public const string DeleteOperationName = "Delete";
        public const string ApproveOperationName = "Approve";
        public const string RejectOperationName = "Reject";
    }

    // Operations related to Authorization
    public class Operations
    {
        public static OperationAuthorizationRequirement Create =
            new OperationAuthorizationRequirement { Name = Constants.CreateOperationName };
        public static OperationAuthorizationRequirement Read =
            new OperationAuthorizationRequirement { Name = Constants.ReadOperationName };
        public static OperationAuthorizationRequirement Update =
            new OperationAuthorizationRequirement { Name = Constants.UpdateOperationName };
        public static OperationAuthorizationRequirement Delete =
            new OperationAuthorizationRequirement { Name = Constants.DeleteOperationName };
        public static OperationAuthorizationRequirement Approve =
            new OperationAuthorizationRequirement { Name = Constants.ApproveOperationName };
        public static OperationAuthorizationRequirement Reject =
            new OperationAuthorizationRequirement { Name = Constants.RejectOperationName };
    }
}
