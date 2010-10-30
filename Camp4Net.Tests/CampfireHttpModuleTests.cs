using System;
using System.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Camp4Net.Tests
{
    [TestClass]
    public class CampfireHttpModuleTests
    {
        [TestMethod]
        public void MailService_OnApplicationError_ShouldPostUserMessage()
        {
            // arrange
            var requestMock = new Mock<HttpRequestBase>();
            var serverMock = new Mock<HttpServerUtilityBase>();
            var contextMock = new Mock<HttpContextBase>();
            var mailServiceMock = new Mock<IMailService>();
            var cModule = new CampfireHttpModule();

            serverMock.Setup(server => server.GetLastError()).Returns(new Exception("RandomExecption"));
            requestMock.Setup(request => request.Url).Returns(new Uri("http://wellformed.uri.com"));
            contextMock.Setup(context => context.Server).Returns(serverMock.Object);
            contextMock.Setup(context => context.Request).Returns(requestMock.Object);
            
            var expectedTextToSendToCampfire = "User Unknown user experienced error!";

            mailServiceMock.Setup(service => service.PostText(expectedTextToSendToCampfire)).Verifiable("Method PostText did not send and/or argument was not the expected");
            cModule.Init(new HttpApplication());

            // act
            cModule.OnApplicationErrorWrapper(contextMock.Object, mailServiceMock.Object);

            mailServiceMock.Verify();
        }
    }
}
