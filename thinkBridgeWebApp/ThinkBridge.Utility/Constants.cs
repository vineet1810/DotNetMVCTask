using System.Net;

namespace ThinkBridge.Utility
{
    public static class Constants
    {
        public static string USP_SAVE_INVENTORY => "usp_SaveInventory";
        public static string USP_GETINVENTORY => "usp_GetInventory";
        public static string SYMBOL_AT_THE_RATE => "@";
        public static string RECORD_COUNT => "recordCount";
        public static string URL_Constant => "https://localhost:44357/";
        public static string URL_CONSTANT_SAVE_INVENTORY => URL_Constant+ "SaveInventory";
        public static string URL_CONSTANT_GET_INVENTORY => URL_Constant + "GetInventory";
        public static string I_NO_ICON_SVG => "images/No_Image_2.png";
        public static string BASE_64_STRING_MAKER => "data:image/{0};base64,{1}";
    }
}
