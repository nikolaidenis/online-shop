using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using Microsoft.Practices.Unity;
using OnlineShop.Api.Models;
using OnlineShop.Core;
using OnlineShop.Core.Data;
using OnlineShop.Infrastructure.Data;

namespace OnlineShop.Api.Controllers
{
    public class OperationsController : ApiController
    {
        [Dependency]
        private IUnitOfWork UnitOfWork { get; }

        public OperationsController(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        [Route("api/operation_purchase")]
        public async Task<HttpResponseMessage> PostPurchase(OperationModel model)
        {
            try
            {
                var purchase = new Operation
                {
                    IsSelled = false,
                    Quantity = 1,
                    UserID = model.UserId,
                    Amount = model.Amount,
                    Date = DateTime.Now.Date,
                    ProductId = model.ProductId
                };
                await UnitOfWork.Operations.MakePurchase(purchase);
            }
            catch (SqlException e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                    $"Could not make purcahse: {e.Message}");
            }


            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [Route("api/operation_sale")]
        public async Task<HttpResponseMessage> PostSale(OperationModel model)
        {
            try
            {
                await UnitOfWork.Operations.MakeSale(model.Id);
            }
            catch (Exception)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, $"Operation with id:{model.Id} not found!");
            }
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [Route("api/operation/{userId:int}/{pageNum:int}/{rowsCount:int}")]
        public async Task<HttpResponseMessage> GetOperations(int userId, int pageNum = 1, int rowsCount = 5)
        {
            if (userId <= 0)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "User is not exist");
            }

            var listOperations = await UnitOfWork.Operations.GetOperations(userId, pageNum, rowsCount);
            Mapper.Initialize(expression => expression.CreateMap(typeof(Operation), typeof(OperationModel)));
            var operationsModel = Mapper.Map<IEnumerable<Operation>, List<OperationModel>>(listOperations);

            return Request.CreateResponse(HttpStatusCode.OK, operationsModel);
        }

        [Route("api/operation/count_rows/{userId:int}")]
        public async Task<HttpResponseMessage> GetRows(int userId)
        {
            if (userId <= 0)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "User does not exist");
            }

            var rows = await UnitOfWork.Operations.GetOperations(userId);
            return Request.CreateResponse(HttpStatusCode.OK, rows);
        }
    }
}
