﻿using System.Data.Common;
using Abp.Zero.EntityFramework;
using Joos.Authorization.Roles;
using Joos.MultiTenancy;
using Joos.Users;

namespace Joos.EntityFramework
{
    public class JoosDbContext : AbpZeroDbContext<Tenant, Role, User>
    {
        //TODO: Define an IDbSet for your Entities...

        /* NOTE: 
         *   Setting "Default" to base class helps us when working migration commands on Package Manager Console.
         *   But it may cause problems when working Migrate.exe of EF. If you will apply migrations on command line, do not
         *   pass connection string name to base classes. ABP works either way.
         */
        public JoosDbContext()
            : base("Default")
        {

        }

        /* NOTE:
         *   This constructor is used by ABP to pass connection string defined in JoosDataModule.PreInitialize.
         *   Notice that, actually you will not directly create an instance of JoosDbContext since ABP automatically handles it.
         */
        public JoosDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {

        }

        //This constructor is used in tests
        public JoosDbContext(DbConnection connection)
            : base(connection, true)
        {

        }
    }
}