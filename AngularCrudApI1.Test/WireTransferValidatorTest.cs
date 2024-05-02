using AngularCrudApI1.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngularCrudApI1.Test
{
    [TestClass]
    public class WireTransferValidatorTest
    {
        [TestMethod]
        public void  ValidateReturnsErrorWhenInsufficientFunds()
        {
            #region Preparation
            Account origin = new Account() { Funds = 0 };    // origin= 10-7=3
            Account destination = new Account() { Funds = 0 };// destination=7+5=12
            decimal amountToTransfer = 5m;

            //var mockValidatorTransfer = new Mock<IValidateWireTransfer>();
            //mockValidatorTransfer.Setup(x => x.Validate(origin, destination, amountToTransfer))
            //    .Returns(new OperationResult(true, ""));

            var service = new WireTransferValidator();
            #endregion

            #region Testing
            var result =service.Validate(origin, destination, amountToTransfer);
            #endregion

            #region Verification
            Assert.IsFalse(result.IsSuccessful);
            Assert.AreEqual("The origin account does not have enough funds available", result.ErrorMessage);
            #endregion
        }

        [TestMethod]
        public void ValidateReturnsSuccessfulOperation()
        {
            #region Preparation
            Account origin = new Account() { Funds = 7 };    
            Account destination = new Account() { Funds = 0 };
            decimal amountToTransfer = 5m;

            //var mockValidatorTransfer = new Mock<IValidateWireTransfer>();
            //mockValidatorTransfer.Setup(x => x.Validate(origin, destination, amountToTransfer))
            //    .Returns(new OperationResult(true, ""));

            var service = new WireTransferValidator();
            #endregion

            #region Testing
            var result = service.Validate(origin, destination, amountToTransfer);
            #endregion

            #region
            Assert.IsTrue(result.IsSuccessful);
            #endregion
        }
    }
}
