﻿using Exchange.Server.Exceptions.NetworkExceptions;
using Exchange.Server.Primitives;
using Exchange.Server.Protocols;
using Exchange.System.Packages;
using ExchangeSystem.Helpers;
using ExchangeSystem.Packages;
using System;
using System.Threading.Tasks;
using ResponseStatus = Exchange.System.Enums.ResponseStatus;

namespace Exchange.Server.Controllers
{
    public abstract class Controller
    {
        internal Controller() { }

        public ResponsePackage ResponsePackage { get; private set; }
        public Response Response { get; private set; }
        public RequestContext Context { get; private set; }

        public async Task ProcessRequestAsync(RequestContext context)
        {
            Context = context;
            Response = ExecuteRequestMethod<Response>();
            await SendResponseAsync();
        }

        private T ExecuteRequestMethod<T>()
            where T : Response
        {
            Response responsePack;
            try
            {
                string requestMethodName = Context.Request.Query;
                responsePack = (T)GetType().GetMethod(requestMethodName).Invoke(this, null);
            }
            catch (Exception ex)
            {
                var report = new ResponseReport(ex?.InnerException?.Message, ResponseStatus.Bad);
                responsePack = new Response<ResponseReport>(report);
            }
            return (T)responsePack ?? default;
        }

        private async Task SendResponseAsync()
        {
            Ex.ThrowIfTrue<ConnectionException>(() => !Context.Client.Connected, "Client was not connected!");
            var protocol = new NewDefaultProtocol(Context.Client);
            await protocol.SendResponseAsync(Response);
        }
    }
}
