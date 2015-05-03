using System;
using System.Configuration;
using System.Web.Mvc;
using System.Web.Routing;
using ChatApp.Application.Commands.Base;
using ChatApp.Application.Queries.Base;
using ChatApp.Application.RestSharp;
using ChatApp.Presentation.WebMvc.Controllers;
using ChatApp.Presentation.WebMvc.Hubs;
using ChatApp.Presentation.WebMvc.Hubs.Impl;
using ChatApp.Presentation.WebMvc.Services;
using ChatApp.Utility;

namespace ChatApp.Presentation.WebMvc
{
    public class CompositeRoot : IDisposable
    {
        public static readonly string ApplicationUrl
            = ConfigurationManager.AppSettings["ApplicationUrl"];

        public static readonly Type HubUsernameProviderType =
            Type.GetType("ChatApp.Presentation.WebMvc.Hubs.Impl." +
                         ConfigurationManager.AppSettings["HubUsernameProvider"]);

        public static readonly Type AuthentifactionWithoutPasswordType =
            Type.GetType("ChatApp.Presentation.WebMvc.Services.Impl." +
                         ConfigurationManager.AppSettings["Authentification"]);

        public static readonly CompositeRoot Instance
            = new CompositeRoot();

        private CompositeRoot() { }

        public void ComposeApp()
        {
            ComposeChatHub();
            ComposeControllerFactory();
        }

        private void ComposeChatHub()
        {
            ChatHub.UsernameProvider = ResolveUsernameProvider();

            ChatHub.ControllerFactory = (chat, room) => new HubController(chat, room,
                ResolveCommandBus(),
                ResolveQueryBus());
        }

        private void ComposeControllerFactory()
        {
            ControllerBuilder.Current.SetControllerFactory(new MyControllerFactory());
        }

        private class MyControllerFactory : DefaultControllerFactory
        {
            public override IController CreateController(RequestContext requestContext, string controllerName)
            {
                if (controllerName == typeof(HomeController).Name.RemoveSubstring("Controller"))
                    return ResolveHomeController();

                return base.CreateController(requestContext, controllerName);
            }
        }

        private static IAuthenticationWithoutPassword ResolveAuthenticationWithoutPassword()
        {
            return (IAuthenticationWithoutPassword)Activator.CreateInstance(AuthentifactionWithoutPasswordType);
        }

        private static IController ResolveHomeController()
        {
            return new HomeController(ResolveQueryBus(), ResolveAuthenticationWithoutPassword());
        }

        private static IUsernameProvider ResolveUsernameProvider()
        {
            return (IUsernameProvider)Activator.CreateInstance(HubUsernameProviderType);
        }

        private static ICommandBus ResolveCommandBus()
        {
            return new RestSharpCommandBus(ApplicationUrl);
        }

        private static IQueryBus ResolveQueryBus()
        {
            return new RestSharpQueryBus(ApplicationUrl);
        }

        public void Dispose()
        {
        }
    }
}