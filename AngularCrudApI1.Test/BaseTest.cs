using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AngularCrudApI1.Test
{
    public class BaseTest
    {
        #region Descriptions Regarding How to Mock The Dbcontext using In-Memory database and all

        /*Your BaseTest class contains a method named BuildContext that is used to build an instance of your MyAngularDataContext class,
         which inherits from DbContext. This method is specifically designed for unit testing purposes, as it creates
         an in-memory database context for testing without actually interacting with a physical database.

         * MyAngularDataContext is my context file which inherited from DbContext in AngularCrud Project
         ..now BaseTest have the power to acces all Dbset which all are present in MyAngularDataContext class..

         The BuildContext method is a protected method within your BaseTest class. 
         Being protected means it can be accessed by derived classes (subclasses)
         but not directly from outside the class hierarchy.
        
         The method takes a string parameter named databasename. This parameter is used as the name for the in-memory database 
        that will be created. Each time you call this method with a different databasename, a new in-memory database 
        instance will be created.
        
         Inside the BuildContext method, you're using DbContextOptionsBuilder<T> to configure options for your MyAngularDataContext.
        In this case, you're using the UseInMemoryDatabase method to specify that you want to use an in-memory database with the
        provided databasename.
        
         After configuring the options, you create an instance of your MyAngularDataContext class by passing the configured 
        options (options) to its constructor. This creates a new instance of your database context with the specified in-memory database.
        
         Finally, the method returns the created dbContext, which can be used in your unit tests to perform database operations 
        against the in-memory database.
        
         Overall, this approach is commonly used in unit testing scenarios to isolate tests from external 
        dependencies, such as a physical database. It allows you to create a fresh instance of your database 
        context with a clean in-memory database for each test method, ensuring test independence and reproducibility..
        */

        #endregion
        protected MyAngularDataContext BuildContext(string databasename)
        {
            /* for every Testmethod we are passing fresh db name  */
            var options = new DbContextOptionsBuilder<MyAngularDataContext>()
                .UseInMemoryDatabase(databasename).Options;

            var dbContext = new MyAngularDataContext(options);
            return dbContext;
        }
    }
}
