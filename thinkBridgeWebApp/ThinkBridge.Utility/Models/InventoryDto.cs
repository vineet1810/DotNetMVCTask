using System.Collections.Generic;
using static ThinkBridge.UtilityAndModels.Enums;

namespace ThinkBridge.UtilityAndModels.Models
{
    public class InventoryDto
    {
        public InventoryModel inventory { get; set; }
        public List<InventoryModel> inventories { get; set; }
        public ErrorCode ErrCode { get; set; }
        public long RecordCount { get; set; }
    }
}
