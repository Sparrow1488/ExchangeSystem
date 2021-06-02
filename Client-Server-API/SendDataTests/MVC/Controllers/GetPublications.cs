﻿using ExchangeServer.MVC.Models;
using ExchangeServer.Protocols;
using ExchangeServer.Protocols.Selectors;
using ExchangeSystem.Requests.Objects.Entities;
using ExchangeSystem.Requests.Packages;
using ExchangeSystem.Requests.Packages.Default;
using ExchangeSystem.SecurityData;
using System.Net.Sockets;

namespace ExchangeServer.MVC.Controllers
{
    public class GetPublications : Controller
    {
        public override RequestType RequestType { get; } = RequestType.GetPublication;

        protected override Protocol Protocol { get; set; }
        protected override IProtocolSelector ProtocolSelector { get; set; } = new ProtocolSelector();

        public override void ProcessRequest(TcpClient connectedClient, Package package, EncryptType encryptType)
        {
            Client = connectedClient;
            EncryptType = encryptType;
            var userPackage = package as Package;
            PublicationModel publicationModel = new PublicationModel();
            var publications = publicationModel.GetAllOrDefault();
            if (publications == null)
                PrepareResponse(false, new Publication[0], "Ошибка обращения к базе данных");
            else if (publications.Length == 0)
                PrepareResponse(false, new Publication[0], "Список публикаций пока пуст");
            else
                PrepareResponse(true, publications, "");

            SendResponse();
        }
        private void PrepareResponse(bool success, Publication[] posts, string errorMessage)
        {
            if (success)
                Response = new ResponsePackage(posts, ResponseStatus.Ok, "");
            else
                Response = new ResponsePackage(string.Empty, ResponseStatus.Exception, errorMessage);
        }
    }
}
