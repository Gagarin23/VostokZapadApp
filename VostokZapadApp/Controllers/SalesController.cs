﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VostokZapadApp.Domain.Core;
using VostokZapadApp.Domain.Core.DataBase;
using VostokZapadApp.Domain.Core.InputOutputData;
using VostokZapadApp.Services.Interfaces;

namespace VostokZapadApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesController : ControllerBase
    {
        private readonly ISalesService _salesService;
        private readonly IValidateService _validateService;

        public SalesController(ISalesService salesService, IValidateService validateService)
        {
            _salesService = salesService;
            _validateService = validateService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Sales>>> GetAll()
        {
            return await _salesService.GetAllAsync();
        }

        [HttpGet]
        public async Task<ActionResult<List<Sales>>> GetByDate(DateTime min, DateTime max)
        {
            return await _salesService.GetByDateAsync(min, max);
        }

        [HttpGet]
        public async Task<ActionResult<Sales>> GetByCustomer(string customerName)
        {
            if(string.IsNullOrWhiteSpace(customerName))
                return BadRequest();

            return await _salesService.GetByCustomerAsync(customerName);
        }

        [HttpGet]
        public async Task<ActionResult<Sales>> GetById(int documentId)
        {
            if(documentId == 0)
                return BadRequest();

            return await _salesService.GetByDocIdAsync(documentId);
        }

        [HttpPost]
        public async Task<ActionResult> AddCustomer(string customerName)
        {
            if(string.IsNullOrWhiteSpace(customerName))
                return BadRequest();


        }

        [HttpPost]
        public async Task<ActionResult> AddOrder(DateTime date, int documentId, decimal sum, string customerName) //По хорошему тут должена быть своя абстракция на входные данные.
        {
            if(date == DateTime.MinValue || documentId == 0 || sum <= 0 || string.IsNullOrWhiteSpace(customerName))
                return BadRequest();


        } 
    }
}
