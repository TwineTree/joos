using Abp.Domain.Entities;
using Abp.EntityFramework;
using Abp.EntityFramework.Repositories;

namespace Joos.EntityFramework.Repositories
{
    public abstract class JoosRepositoryBase<TEntity, TPrimaryKey> : EfRepositoryBase<JoosDbContext, TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {
        protected JoosRepositoryBase(IDbContextProvider<JoosDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        //add common methods for all repositories
    }

    public abstract class JoosRepositoryBase<TEntity> : JoosRepositoryBase<TEntity, int>
        where TEntity : class, IEntity<int>
    {
        protected JoosRepositoryBase(IDbContextProvider<JoosDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        //do not add any method here, add to the class above (since this inherits it)
    }
}
