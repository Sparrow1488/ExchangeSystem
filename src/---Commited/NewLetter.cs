﻿using Exchange.Server.Models;
using Exchange.Server.Protocols;
using Exchange.Server.Protocols.Selectors;
using Exchange.System.Entities;
using Exchange.System.Enums;
using Exchange.System.Packages.Default;
using Exchange.System.Protection;
using System;
using System.Net.Sockets;

namespace Exchange.Server.Controllers
{
    public class NewLetter : Message
    {
        public override RequestType RequestType => RequestType.NewMessage;
        protected override Protocol Protocol { get; set; }
        protected override IProtocolSelector ProtocolSelector { get; set; }
        private Package _clientRequestObject;

        public void ProcessRequest(TcpClient connectedClient, Package package, EncryptType encryptType)
        {
            AssignValues(connectedClient, package, encryptType);

            var userLetter = _clientRequestObject.RequestObject as Letter;
            if(userLetter != null)
            {
                userLetter = Validation(userLetter);
                LetterModel letterModel = new LetterModel();
                bool wasAddSuccess = letterModel.Add(userLetter);
                PrepareResponsePackage(wasAddSuccess);
            }
            else
                PrepareResponsePackage(false);
            SendResponse();
        }
        private void AssignValues(TcpClient client, IPackage package, EncryptType encryptType)
        {
            base.Client = client;
            _clientRequestObject = (Package)package;
            EncryptType = encryptType;
        }
        private void PrepareResponsePackage(bool addMessageSuccess)
        {
            string errorMessage = string.Empty;
            object responseObject;
            if (addMessageSuccess)
            {
                responseObject = "Сообщение доставлено";
                Response = new ResponsePackage(responseObject, ResponseStatus.Ok);
            }
            else
            {
                errorMessage = "Неизвестная ошибка";
                Response = new ResponsePackage("", ResponseStatus.Exception, errorMessage);
            }
        }
        private Letter Validation(Letter letter)
        {
            letter.DateCreate = DateTime.Now;
            return letter;
        }
        
    }
}
