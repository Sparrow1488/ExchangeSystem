﻿using Exchange.System.Packages.Default;
using Exchange.System.Protection;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Exchange.Server.Routers
{
    public interface IRouter
    {
        Task<IPackage> IssueRequestAsync(TcpClient client);
        EncryptType GetPackageEncryptType();
    }
}
