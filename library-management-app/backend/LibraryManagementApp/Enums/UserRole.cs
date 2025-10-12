using System.Text.Json.Serialization;

namespace LibraryManagementApp.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum UserRole
    {
        User_Admin,
        User_Standard,
        User_ReadOnly,
        Member_Client
    }
}