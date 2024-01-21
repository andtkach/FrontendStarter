namespace Jiwebapi.Catalog.Application.Error
{
    public static class ErrorCodes
    {
        public static string GenericError => "ERR-000";
        
        public static string ApplicationError => "ERR-001";
        public static string ValidationError => "ERR-001";
        public static string ParameterEmpty => "EMPTY-002";
        public static string Unauthorized => "ERR-403";
        public static string CategoryNotFound => "CAT-001";
        public static string CategoriesNotFound => "CAT-002";
    }
}