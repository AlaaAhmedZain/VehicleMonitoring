using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace VehicleMonitoring.Common.Core.Repository
{
    /// <summary>
    /// Responsible for handle repositories instances.
    /// </summary>
    public interface IRepositoryFactory
    {
        #region Methods
        /// <summary>
        /// Get the repository factory function for the type.
        /// </summary>
        /// <typeparam name="T">Type serving as the repository factory lookup key.</typeparam>
        /// <returns>The repository function if found, else null.</returns>
        Func<DbContext, object> GetRepositoryFactory<T>();
        /// <summary>
        /// Get the factory for <see cref="IRepository{T}"/> where T is an entity type.
        /// </summary>
        /// <typeparam name="T">The root type of the repository, typically an entity type.</typeparam>
        /// <returns>
        /// A factory that creates the <see cref="IRepository{T}"/>, given an EF <see cref="DbContext"/>.
        /// </returns>
        Func<DbContext, object> GetRepositoryFactoryForEntityType<T>() where T : class;
        #endregion
    }
}
