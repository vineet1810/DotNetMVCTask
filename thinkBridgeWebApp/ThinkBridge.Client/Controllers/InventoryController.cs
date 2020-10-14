using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ThinkBridge.Utility;
using ThinkBridge.UtilityAndModels.Models;
using static ThinkBridge.UtilityAndModels.Enums;

namespace ThinkBridge.Client.Controllers
{
    public class InventoryController : Controller
    {
        public IActionResult Index()
        {
            var client = new RestClient($"{ Constants.URL_CONSTANT_GET_INVENTORY }?{nameof(InventoryModel.InventoryID)}=0&{nameof(InventoryDto.RecordCount)}=0");
            var request = new RestRequest(Method.GET) { RequestFormat = DataFormat.Json };
            IRestResponse response = client.Execute(request);
            InventoryDto inventoryDto = JsonConvert.DeserializeObject<InventoryDto>(response.Content);
            return View(inventoryDto);
        }

        [HttpPost]
        public IActionResult AddDelete(InventoryModel inventory, List<IFormFile> postedFiles)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }
            foreach (var file in postedFiles)
            {
                if (file.Length > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        file.CopyTo(ms);
                        var fileBytes = ms.ToArray();
                        inventory.FileExtension = postedFiles[0].FileName.Split('.').Last();
                        inventory.ProductPhoto = Convert.FromBase64String(Convert.ToBase64String(fileBytes));
                    }
                }
            }
            inventory.IsActive = true;
            InventoryDto inventoryDto = new InventoryDto
            {
                inventory =inventory
            };
            var client = new RestClient(Constants.URL_CONSTANT_SAVE_INVENTORY);
            var request = new RestRequest(Method.POST) { RequestFormat = DataFormat.Json };
            request.AddJsonBody(inventoryDto);
            client.Execute(request);
            return RedirectToAction("Index");
        }

        public IActionResult AddDelete(int id)
        {
            InventoryDto inventoryDto = new InventoryDto
            {
                inventory = new InventoryModel { InventoryID =id, IsActive =false}
            };
            var client = new RestClient(Constants.URL_CONSTANT_SAVE_INVENTORY);
            var request = new RestRequest(Method.POST) { RequestFormat = DataFormat.Json };
            request.AddJsonBody(inventoryDto);
            client.Execute(request);
            return RedirectToAction("Index");
        }

        public IActionResult Detail(int id)
        {
            var client = new RestClient($"{ Constants.URL_CONSTANT_GET_INVENTORY }?{nameof(InventoryModel.InventoryID)}={id}&{nameof(InventoryDto.RecordCount)}=-1");
            var request = new RestRequest(Method.GET) { RequestFormat = DataFormat.Json };
            IRestResponse response = client.Execute(request);
            InventoryDto inventoryDto = JsonConvert.DeserializeObject<InventoryDto>(response.Content);
            return View(inventoryDto);
        }
    }
}