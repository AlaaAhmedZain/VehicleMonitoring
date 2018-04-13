using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using VehicleMonitoring.Common.Core.Repository;

namespace VehicleMonitoring.Common.Repository
{
    public class RepositoryFactory : IRepositoryFactory
    {
        #region Data Members 
        /// <summary>
        /// Get the dictionary of repository factory functions.
        /// </summary>
        /// <remarks>
        /// A dictionary key is a System.Type, typically a repository type.
        /// A value is a repository factory function
        /// that takes a <see cref="DbContext"/> argument and returns
        /// a repository object. Caller must know how to cast it.
        /// </remarks>
        private readonly IDictionary<Type, Func<DbContext, object>> _repositoryFactories;

        #endregion
        #region CTOR
        /// <summary>
        /// Constructor that initializes with runtime repository factories
        /// </summary>
        public RepositoryFactory()
        {
            _repositoryFactories = GetFactories();
        }

        /// <summary>
        /// Constructor that initializes with an arbitrary collection of factories
        /// </summary>
        /// <param name="factories">
        /// The repository factory functions for this instance. 
        /// </param>
        /// <remarks>
        /// This ctor is primarily useful for testing this class
        /// </remarks>
        public RepositoryFactory(IDictionary<Type, Func<DbContext, object>> factories)
        {
            _repositoryFactories = factories;
        }
        #endregion
        #region Public Operations
        public Func<DbContext, object> GetRepositoryFactory<T>()
        {

            Func<DbContext, object> factory;
            _repositoryFactories.TryGetValue(typeof(T), out factory);
            return factory;
        }
        public Func<DbContext, object> GetRepositoryFactoryForEntityType<T>() where T : class
        {
            return GetRepositoryFactory<T>() ?? DefaultEntityRepositoryFactory<T>();
        }
        #endregion
        #region Helper Methods
        /// <summary>
        /// Default factory for a <see cref="IRepository{T}"/> where T is an entity.
        /// </summary>
        /// <typeparam name="T">Type of the repository's root entity</typeparam>
        private Func<DbContext, object> DefaultEntityRepositoryFactory<T>() where T : class
        {
            return dbContext => new EFRepository<T>(dbContext);
        }
        /// <summary>
        /// Return the runtime code Crossvertise repository factory functions,
        /// each one is a factory for a repository of a particular type.
        /// </summary>
        private IDictionary<Type, Func<DbContext, object>> GetFactories()
        {
            return new Dictionary<Type, Func<DbContext, object>>
            {
                //Instances for customized Repositories. 
            };
        }
        #endregion
    }
}
