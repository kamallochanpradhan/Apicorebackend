using AngularCrudApI1.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace AngularCrudApI1.Test.UnitTests
{
    #region Mockk the DbContext for UnitTesting in using MsUnitTest
    /*Here I mock the DbContext for UnitTesting in using MsUnitTest*/
    /*Here we will Mock dbcontext*/
    /* we will install package Microsoft.EntityFrameworkCore.InMemory.. why we using this bcoz insted of using real 
     * database w e use inmemory  and so it allow us to do quick operation with the databse  and 
     * also we have personnel databse for each testmethod
  
     Each instance of the context maintains its own state and tracking of entities. This ensures that changes made to the context 
     or entities in one part of the test (e.g., adding mock data) do not affect the behavior or outcome of the test in another  
     part (e.g., testing the GetStudent method).
    
      Testing with a new instance of the context (context2) helps ensure that the test is isolated and does not depend on 
      any prior state or data modifications. It provides a clean slate for testing the specific functionality (GetStudent)
      without interference from other parts of the test or application.

      The test case involves creating a mock student repository context, adding some mock student data,
      and then testing the GetStudent method to verify if it retrieves the correct number of students...

      The line var databasename = Guid.NewGuid().ToString(); is generating a unique identifier (GUID) and converting it to a string.
      This unique string is typically used as a database name or identifier in scenarios like unit testing with an in-memory database 
      or temporary database creation.*/
    #endregion

    [TestClass]
    public class StudenttRepositoryTest : BaseTest
    {
        [TestMethod]
        public void GetAllStudent()
        {
            string dateOfBirthString = "1989-04-04";

            #region Preparation
            var databasename = Guid.NewGuid().ToString();
            var context = BuildContext(databasename);

            //Here we will adding some mock student data, ..here this context is tracked by above context
            context.Students.Add(new Model.Student()
            { Password = 456, StudentName = "Kamal", Email = "bimal@1989", DateOfBirth = DateTime.Parse(dateOfBirthString), Gender = "F", Address = "Kendrapara", Pin = 123456 });

            context.Students.Add(new Model.Student()
            { Password = 456, StudentName = "Bimal", Email = "das@1989", DateOfBirth = DateTime.Parse(dateOfBirthString), Gender = "F", Address = "Pattamundai", Pin = 654321 });

            context.SaveChanges();

            //so we dont want to use above context we will create new contex2  here context2 will 
            var context2 = BuildContext(databasename);

            #endregion

            #region Testing
            var stdRepository = new StudenttRepository(context2);
            var response = stdRepository.GetStudent();
            #endregion

            #region Verification
            var repo = response.Result;
            Assert.AreEqual(2, repo.Count());
            #endregion
        }

        //Pending
        [TestMethod]
        public void GetStudentByIdNotExit()
        {
            string dateOfBirthString = "1989-04-04";

            #region Preparation
            var databasename = Guid.NewGuid().ToString();
            var context = BuildContext(databasename);

            ////Here we will adding some mock student data, ..here this context is tracked by above context
            //context.Students.Add(new Model.Student()
            //{ Password = 456, StudentName = "Kamal", Email = "bimal@1989", DateOfBirth = DateTime.Parse(dateOfBirthString), Gender = "F", Address = "Kendrapara", Pin = 123456 });

            //context.Students.Add(new Model.Student()
            //{ Password = 456, StudentName = "Bimal", Email = "das@1989", DateOfBirth = DateTime.Parse(dateOfBirthString), Gender = "F", Address = "Pattamundai", Pin = 654321 });

            //context.SaveChanges();

            //so we dont want to use above context we will create new contex2  here context2 will 
            var context2 = BuildContext(databasename);

            #endregion

            #region Testing

            var stdRepository = new StudenttRepository(context2);
            var response = stdRepository.GetStudentByID(12);
             //var result = response.Result as StatusCodeResult;
           // var result = response.Result as null;

            #endregion

            #region Verification
           // var repo = response.Result;
           //Assert.AreEqual(204, result);
            #endregion
        }
    }
}
