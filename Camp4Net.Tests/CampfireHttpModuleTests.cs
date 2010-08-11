using System;
using System.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Camp4Net.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void MailService_OnApplicationError_ShouldPostUserMessage()
        {
            var requestMock = new Mock<HttpRequestBase>();
            var serverMock = new Mock<HttpServerUtilityBase>();
            var contextMock = new Mock<HttpContextBase>();
            var mailServiceMock = new Mock<IMailService>();
            var cModule = new CampfireHttpModule();

            serverMock.Setup(server => server.GetLastError()).Returns(new Exception("RandomExecption"));
            requestMock.Setup(request => request.Url).Returns(new Uri("http://wellformed.uri.com"));
            contextMock.Setup(context => context.Server).Returns(serverMock.Object);
            contextMock.Setup(context => context.Request).Returns(requestMock.Object);
            mailServiceMock.Setup(service => service.PostText("Bruker Unknown opplevde feil i kontrollstasjonen!")).Verifiable("Error handling did not send text message!");

            cModule.Init(new HttpApplication());
            cModule.OnApplicationErrorWrapper(contextMock.Object, mailServiceMock.Object);

            mailServiceMock.Verify();
        }
    }
}
