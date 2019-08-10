using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ShoppingList.API.Repositories;
using ShoppingList.API.Services;

namespace ShoppingList.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingListController : ControllerBase
    {
        private readonly MssqlDatabaseRepository _databaseRepository;

        public ShoppingListController(IConfiguration config)
        {
            try
            {
                _databaseRepository = new MssqlDatabaseRepository(config);
            }
            catch (Exception exception)
            {
                exception.ToLogError();
            }
        }

        [HttpGet]
        public ActionResult<IEnumerable<ShoppingItem>> Get()
        {
            return new ActionResult<IEnumerable<ShoppingItem>>(_databaseRepository.GetShoppingItems());
        }
        [Route("boughtItems")]
        [HttpGet]
        public ActionResult<IEnumerable<ShoppingItem>> GetBoughtItems()
        {
            return new ActionResult<IEnumerable<ShoppingItem>>(_databaseRepository.GetBoughtItems());
        }
        [Route("add")]
        [HttpPost]
        public void AddItem([FromBody] ShoppingItem shoppingItem)
        {
            try
            {
                shoppingItem.CreationDate = DateTime.Now;
                _databaseRepository.AddShoppingItem(shoppingItem);
            }
            catch (Exception exception)
            {
                exception.ToLogError();
            }
        }
        [Route("checkOff")]
        [HttpPut]
        public void CheckOff(int id)
        {
            try
            {
                _databaseRepository.CheckOffShoppingItem(id);
            }
            catch (Exception exception)
            {
                exception.ToLogError();
            }
        }
    }
}
