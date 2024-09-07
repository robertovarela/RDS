namespace RDS.Core.Common.Extensions;

public static class NavRules
{
    public static long LoggedUserId { get; set; }
    public static bool IsAdmin { get; set; }
    public static bool IsOwner { get; set; }
        
    public static string EditUrl { get; set; } = string.Empty;
    public static string CreateUrl { get; set; } = string.Empty;
    public static string SourceUrl { get; set; } = string.Empty;
    public static string OrigenUrl { get; set; } = string.Empty;
    public static string BackUrl { get; set; } = string.Empty;
    public static string CancelUrl { get; set; } = string.Empty;
    
    public static string CancelOrBackButtonText { get; set; } = "Cancelar";
}