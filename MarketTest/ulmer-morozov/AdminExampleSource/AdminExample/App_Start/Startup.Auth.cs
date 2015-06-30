using System;
using System.Web.Http;
using AdminExample.DI;
using AdminExample.Providers;
using Data;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using Ninject;
using Ninject.Web.Common.OwinHost;
using Ninject.Web.WebApi.OwinHost;
using Owin;

namespace AdminExample
{
    public partial class Startup
    {
        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }

        public static string PublicClientId { get; private set; }

        // Дополнительные сведения о настройке аутентификации см. по адресу: http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Настройка контекста базы данных и диспетчера пользователей для использования одного экземпляра на запрос
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);

            // Включение использования файла cookie, в котором приложение может хранить информацию для пользователя, выполнившего вход,
            // и использование файла cookie для временного хранения информации о входах пользователя с помощью стороннего поставщика входа
            app.UseCookieAuthentication(new CookieAuthenticationOptions());
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Настройка приложения для потока обработки на основе OAuth
            PublicClientId = "self";
            OAuthOptions = new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/Token"),
                Provider = new ApplicationOAuthProvider(PublicClientId),
                AuthorizeEndpointPath = new PathString("/api/Account/ExternalLogin"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(14),
                AllowInsecureHttp = true
            };

            // Включение использования приложением маркера-носителя для аутентификации пользователей
            app.UseOAuthBearerTokens(OAuthOptions);

            //Настройка Ninject и WebApi
            var webApiConfiguration = new HttpConfiguration();
            webApiConfiguration.Routes
                .MapHttpRoute("API Default", "api/{controller}/{id}", new { id = RouteParameter.Optional });
            //GlobalFilters.Filters.Add(new RequireHttpsAttribute());

            //Только Json
            webApiConfiguration.Formatters.XmlFormatter.SupportedMediaTypes.Clear();
            //чтобы с маленьких букв начинались сериализованные данные
            webApiConfiguration.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            //Использовать нинджект
            app.UseNinjectMiddleware(GetKernel).UseNinjectWebApi(webApiConfiguration);
            //авторизация нужна для всего Web Api
            //webApiConfiguration.Filters.Add(new AuthorizeAttribute());

            // Раскомментируйте приведенные далее строки, чтобы включить вход с помощью сторонних поставщиков входа
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //    consumerKey: "",
            //    consumerSecret: "");

            //app.UseFacebookAuthentication(
            //    appId: "",
            //    appSecret: "");

            //app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions());
        }

        private static IKernel GetKernel()
        {
            var kernel = new StandardKernel();
            kernel.Load<MainModule>();
            return kernel;
        }
    }
}
