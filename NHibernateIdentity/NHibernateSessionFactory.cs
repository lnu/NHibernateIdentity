// <copyright file="NHibernatSessionFactory.cs" company="Translation Centre for the Bodies of the European Union">
//  Copyright (c) 2013 All Rights Reserved
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Engine;
using NHibernate.Event;
using NHibernate.Type;
using NHibernate.AspNet.Identity.Helpers;
using WebApplication1.Models;
using NHibernate.Tool.hbm2ddl;

namespace WebApplication1
{
    /// <summary>
    /// NHibernate session factory
    /// </summary>
    public static class NHibernateSessionFactory
    {
        private static readonly ISessionFactory _sessionFactory;

        private static readonly Configuration _configuration;

        static NHibernateSessionFactory()
        {
            _configuration = new Configuration();

            _configuration
                .Configure()
                .SetNamingStrategy(DefaultNamingStrategy.Instance)
                .SetProperty(NHibernate.Cfg.Environment.UseProxyValidator, "true");
            //.EventListeners.FlushEventListeners = new IFlushEventListener[] { new FixedDefaultFlushEventListener() };

            // get mappings
            var myEntities = new[] { typeof(ApplicationUser) };

            _configuration.AddDeserializedMapping(MappingHelper.GetIdentityMappings(myEntities), null);

            _sessionFactory = _configuration.BuildSessionFactory();

            SchemaExport se = new SchemaExport(_configuration);
            se.Drop(false, true);
            se.Create(false, true);
            //se.Execute(false, true, false);
        }

        public static Configuration Configuration
        {
            get
            {
                return _configuration;
            }
        }

        public static ISessionFactory SessionFactory
        {
            get
            {
                return _sessionFactory;
            }
        }
    }
}