namespace Jiwebapi.Catalog.Domain.Common;
using System.ComponentModel.DataAnnotations;

public class StorageItem
{
    public static string S = ":";

    public static string Name = "storageitem";
        
    [Required]
    public string Id { get; set; } = $"{Name}{S}{Guid.NewGuid().ToString().Replace("-", "").ToUpper()}";
        
    [Required]
    public string Value { get; set; } = string.Empty;

    public string GetId()
    {
        if (string.IsNullOrEmpty(this.Id))
        {
            return string.Empty;
        }

        var arr = this.Id.Split(S);
        if (arr.Length > 1)
        {   
            return arr[1];
        }

        return string.Empty;
    }
}