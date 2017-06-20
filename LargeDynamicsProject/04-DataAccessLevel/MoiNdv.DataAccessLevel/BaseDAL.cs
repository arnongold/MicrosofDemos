using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using MoiNdv.DataModel;
using MoiNdv.Framework.Context;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MoiNdv.DataAccessLevel
{
    public abstract class BaseDAL : GlobalContext
    {
        protected GlobalContext GlobalContext { get; private set; }

        protected ServiceContext Context { get; private set; }

        public BaseDAL(GlobalContext globalContext) : base(globalContext)
        {
            GlobalContext = globalContext;
            Context = new ServiceContext(OrganizationService);
        }

        public Guid Create<T>(T entity) where T : Entity
        {
            return OrganizationService.Create(entity);
        }

        public void Update<T>(T entity) where T : Entity
        {
            OrganizationService.Update(entity);
        }

        public T Retrieve<T>(string entityName, Guid id, params string[] columns) where T : Entity
        {
            var columnSet = columns.Length > 0 ? new ColumnSet(columns) : new ColumnSet(true);
            return OrganizationService.Retrieve(entityName, id, columnSet).ToEntity<T>();
        }

        public IEnumerable<T> RetrieveMultiple<T>(QueryBase query) where T : Entity
        {
            return OrganizationService.RetrieveMultiple(query).Entities.Select(item => item.ToEntity<T>()).ToList<T>();
        }

        public ExecuteMultipleResponse ExecuteMultiple(OrganizationRequestCollection collectionRequests, bool continueOnError = true, bool returnResponses = true)
        {
            var request = new ExecuteMultipleRequest();
            request.Requests = collectionRequests;
            request.Settings = new ExecuteMultipleSettings
            {
                ContinueOnError = continueOnError,
                ReturnResponses = returnResponses
            };
            return (ExecuteMultipleResponse)OrganizationService.Execute(request);
        }

        public ExecuteMultipleResponse ExecuteMultipleRequests(OrganizationRequestCollection collectionRequests, bool continueOnError = true, bool returnResponses = true)
        {
            var request = new ExecuteMultipleRequest();

            request.Requests = collectionRequests;
            request.Settings = new ExecuteMultipleSettings
            {
                ContinueOnError = continueOnError,
                ReturnResponses = returnResponses
            };
            return (ExecuteMultipleResponse)OrganizationService.Execute(request);
        }

        public ExecuteTransactionResponse ExecuteTransactionRequests(OrganizationRequestCollection collectionRequests, bool returnResponses = true)
        {
            var request = new ExecuteTransactionRequest
            {
                Requests = collectionRequests,
                ReturnResponses = returnResponses
            };
            return (ExecuteTransactionResponse)OrganizationService.Execute(request);
        }

    }
}
