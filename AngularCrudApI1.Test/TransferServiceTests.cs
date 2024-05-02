using AngularCrudApI1.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
//https://www.udemy.com/course/building-restful-web-apis-with-aspnet-core/learn/lecture/18384226#overview
namespace AngularCrudApI1.Test
{
    [TestClass]
    public class TransferServiceTests
    {
        #region Descriptions How to Mock Service
        /*Sometimes its ok but not right bcoz we need ignore the dependeccy of WireTransferValidator class our focous should to do
           TransferService calss to Test...So here a Mock is an object which
           we will try to impersonate a dependency of a  class ...The idea is that a Moq object allow us to create an object
           and which we have totall control.. from the point of view of unit test is taht Mocks are important bcoz they help us 
           to focous on test on a specific class without having the trouble to deal with the dependency 
           install Moq from Nuget for mocking the dependancy and used MsTest unit text project for unit testing*/
        #endregion

        [TestMethod]
        public void WireTransferWithInsufficientFundsThrowAnError()
        {
            #region Preparation
            Account origin = new Account() { Funds = 0 };
            Account destination = new Account() { Funds = 0 };
            decimal amountToTransfer = 5m;
            string erroMessage = "custom error message";
        
            //var service = new TransferService(new WireTransferValidator());

            var mockValidatorTransfer = new Mock<IValidateWireTransfer>();

            //We will set up ile this based on our result required...here its a negetive scenarion testmethod so 
            mockValidatorTransfer.Setup(x => x.Validate(origin, destination, amountToTransfer))
                .Returns(new OperationResult(false, erroMessage));

            // we are creating an instance of this  interface (IValidateWireTransfer) and we ar epassing it into out transerfer service class
            var service = new TransferService(mockValidatorTransfer.Object);

            Exception exceptionExcepted = null;

            #endregion

            #region Testing
            try
            {
                service.WireTransfer(origin, destination, amountToTransfer);
            }
            catch (Exception ex)
            {
                exceptionExcepted = ex;
            }
            #endregion

            #region Verification
            if (exceptionExcepted == null)
            {
                Assert.Fail("An exception was excepted");
            }
            Assert.IsTrue(exceptionExcepted is ApplicationException);
            Assert.AreEqual(erroMessage, exceptionExcepted.Message);
            #endregion
        }

        [TestMethod]
        public void WireTransferCorrectlyEditFunds()
        {

            #region Preparation
            Account origin = new Account() { Funds = 10 };    // origin= 10-7=3
            Account destination = new Account() { Funds = 5 };// destination=7+5=12
            decimal amountToTransfer = 7m;

            var mockValidatorTransfer = new Mock<IValidateWireTransfer>();
            mockValidatorTransfer.Setup(x => x.Validate(origin, destination, amountToTransfer))
                .Returns(new OperationResult(true, ""));

            var service = new TransferService(mockValidatorTransfer.Object);
            #endregion

            #region Testing
            service.WireTransfer(origin, destination, amountToTransfer);
            #endregion

            #region Verification
            Assert.AreEqual(3, origin.Funds);
            Assert.AreEqual(12, destination.Funds);
            #endregion 
        }

        [TestMethod]
        public void TestMethod2()
        {
            #region Preparation
            #endregion

            #region Testing
            #endregion

            #region Verification
            //Assert.Fail("I am forcefully failing this Test");
            #endregion
        }
    }
}
