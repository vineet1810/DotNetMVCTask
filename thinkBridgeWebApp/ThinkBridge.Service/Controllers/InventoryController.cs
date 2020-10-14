using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.Extensions.FileSystemGlobbing.Abstractions;
using Newtonsoft.Json;
using ThinkBridge.Utility;
using ThinkBridge.UtilityAndModels.Models;
using static ThinkBridge.UtilityAndModels.Enums;

namespace ThinkBridge.Service.Controllers
{
    public class InventoryController : Controller
    {
        [HttpPost]
        [Route("SaveInventory")]
        public async Task<InventoryDto> SaveInventory([FromBody]object inventoryDto)
        {
            InventoryDto inventoryDtoObj = JsonConvert.DeserializeObject<InventoryDto>(inventoryDto.ToString());
            using SqlConnection connection = new SqlConnection(Environment.GetEnvironmentVariable("DB_CONNECTION_STRING"));
            DynamicParameters parameter = new DynamicParameters();
            parameter.Add(Constants.SYMBOL_AT_THE_RATE + nameof(InventoryModel.InventoryID), inventoryDtoObj.inventory.InventoryID, DbType.Int64, ParameterDirection.Input);
            parameter.Add(Constants.SYMBOL_AT_THE_RATE + nameof(InventoryModel.ProductName), inventoryDtoObj.inventory.ProductName, DbType.String, ParameterDirection.Input);
            parameter.Add(Constants.SYMBOL_AT_THE_RATE + nameof(InventoryModel.ProductDescription), inventoryDtoObj.inventory.ProductDescription, DbType.String, ParameterDirection.Input);
            parameter.Add(Constants.SYMBOL_AT_THE_RATE + nameof(InventoryModel.ProductPrice), inventoryDtoObj.inventory.ProductPrice, DbType.Int64, ParameterDirection.Input);
            parameter.Add(Constants.SYMBOL_AT_THE_RATE + nameof(InventoryModel.ProductPhoto), inventoryDtoObj.inventory.ProductPhoto, DbType.Binary, ParameterDirection.Input);
            parameter.Add(Constants.SYMBOL_AT_THE_RATE + nameof(InventoryModel.IsActive), inventoryDtoObj.inventory.IsActive, DbType.Boolean, ParameterDirection.Input);
            parameter.Add(Constants.SYMBOL_AT_THE_RATE + nameof(InventoryModel.FileExtension), inventoryDtoObj.inventory.FileExtension, DbType.String, ParameterDirection.Input);
            parameter.Add(Constants.SYMBOL_AT_THE_RATE + nameof(inventoryDtoObj.ErrCode), inventoryDtoObj.ErrCode, DbType.Int16, direction: ParameterDirection.Output);
            await connection.ExecuteAsync(Constants.USP_SAVE_INVENTORY, parameter, commandType: CommandType.StoredProcedure).ConfigureAwait(false);
            inventoryDtoObj.ErrCode = (ErrorCode)parameter.Get<short>(Constants.SYMBOL_AT_THE_RATE + nameof(inventoryDtoObj.ErrCode));
            return inventoryDtoObj;
        }

        [Route("GetInventory")]
        public async Task<InventoryDto> GetInventory([FromQuery]long inventoryId, long recordCount)
        {
            InventoryDto inventoryDto = new InventoryDto()
            {
                inventories = new List<InventoryModel>(),
                RecordCount = recordCount,
                inventory = new InventoryModel { InventoryID = inventoryId }
            };
            using SqlConnection connection = new SqlConnection(Environment.GetEnvironmentVariable("DB_CONNECTION_STRING"));
            DynamicParameters parameter = new DynamicParameters();
            parameter.Add(Constants.SYMBOL_AT_THE_RATE + nameof(InventoryModel.InventoryID), inventoryDto.inventory.InventoryID, DbType.Int64, ParameterDirection.Input);
            parameter.Add(Constants.SYMBOL_AT_THE_RATE + nameof(InventoryDto.RecordCount), inventoryDto.RecordCount, DbType.Int64, ParameterDirection.Input);
            parameter.Add(Constants.SYMBOL_AT_THE_RATE + nameof(inventoryDto.ErrCode), inventoryDto.ErrCode, DbType.Int16, direction: ParameterDirection.Output);
            inventoryDto.inventories = (await connection.QueryAsync<InventoryModel>(Constants.USP_GETINVENTORY, parameter, commandType: CommandType.StoredProcedure).ConfigureAwait(false)).ToList();
            inventoryDto.ErrCode = (ErrorCode)parameter.Get<short>(Constants.SYMBOL_AT_THE_RATE + nameof(inventoryDto.ErrCode));
            if (recordCount == -1)
            {
                inventoryDto.inventory = inventoryDto.inventories.FirstOrDefault();
                if (inventoryDto.inventory.ProductPhoto != null)
                {
                    inventoryDto.inventory.FileBase64 = string.Format(Constants.BASE_64_STRING_MAKER, inventoryDto.inventory.FileExtension, Convert.ToBase64String(inventoryDto.inventory.ProductPhoto));
                }
            }
            foreach (var item in inventoryDto.inventories ?? Enumerable.Empty<InventoryModel>())
            {
                if (recordCount == 0)
                {
                    item.ProductName = TailTruncateString(item.ProductName, 15);
                    item.ProductDescription = TailTruncateString(item.ProductDescription, 15);
                }
                if (item.ProductPhoto != null)
                {
                    item.FileBase64 = string.Format(Constants.BASE_64_STRING_MAKER, item.FileExtension, Convert.ToBase64String(item.ProductPhoto));
                }
            }
            return inventoryDto;
        }

        private string TailTruncateString(string input, int threshold)
        {
            if (input?.Length > threshold)
            {
                return input.Substring(0, threshold) + " ...";
            }
            return input;
        }
    }
}