using System;
using System.Configuration;
using System.Data.Common;
using System.Data.SqlServerCe;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using ChatApp.Application.WebApi.Controllers;
using ChatApp.Domain.Model;
using ChatApp.Persistence.EF;

namespace ChatApp.Application.WebApi
{
    public class CompositeRoot : IDisposable
    {
        private static readonly int UsersLimit 
            = ReadIntValueFromAppSettings("UsersLimit");

        private static readonly int ChatMessages 
            = ReadIntValueFromAppSettings("ChatMessages");

        private static readonly DbConnection DbConnection =
            new SqlCeConnection(@"Data Source=|DataDirectory|\ChatRoomDb.sdf");

        public static readonly CompositeRoot Instance
           = new CompositeRoot();

        private static readonly ChatRoom ChatRoom =
            new ChatRoom(UsersLimit);

        private CompositeRoot() { }

        public void ComposeApp()
        {
            ComposeControllerFactory();
        }

        private void ComposeControllerFactory()
        {
            GlobalConfiguration.Configuration.Services.Replace(
             typeof(IHttpControllerActivator), new MyControllerFactory());
        }

        private class MyControllerFactory : IHttpControllerActivator
        {
            public IHttpController Create(
                HttpRequestMessage request,
                HttpControllerDescriptor controllerDescriptor,
                Type controllerType)
            {
                if (controllerType == typeof(ChatRoomController))
                    return ResolveChatRoomController();

                return new DefaultHttpControllerActivator()
                    .Create(request, controllerDescriptor, controllerType);
            }

        }
        private static IHttpController ResolveChatRoomController()
        {
            return new ChatRoomController(ChatRoom,
                ResolveChatMessageRepository(),
                ResolveMessagesQuery());
        }

        private static EfGetLastMessagesQuery ResolveMessagesQuery()
        {
            return new EfGetLastMessagesQuery(ChatMessages, DbContextFactory);
        }

        private static IChatMessageRepository ResolveChatMessageRepository()
        {
            return new EfChatMessageRepository(DbContextFactory);
        }

        private static ChatRoomContext DbContextFactory()
        {
            return new ChatRoomContext(DbConnection);
        }

        private static int ReadIntValueFromAppSettings(string key)
        {
            return int.Parse(ConfigurationManager.AppSettings[key]);
        }

        public void Dispose()
        {
            DbConnection.Dispose();
        }
    }
}