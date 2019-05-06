using Models.Entities.DbModels.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Services
{
    public interface IRepoService<TEntity> where TEntity : class, IBaseEntity
    {
        IEnumerable<TEntity> GetAll(Func<TEntity, bool> linqExpression = null, params Expression<Func<TEntity, object>>[] includeExpression);
        IEnumerable<TEntity> GetAll(params Expression<Func<TEntity, object>>[] includeExpressions);
        IEnumerable<TEntity> GetAllThenInclude(Func<TEntity, bool> linqExpression = null, params IncludeBuilderResult[] includeExpressions);
        IEnumerable<TEntity> GetAllThenInclude(params IncludeBuilderResult[] includeExpressions);
        Task<TEntity> Create(TEntity entity);
        Task<TEntity> Update(TEntity entity);
        Task<TEntity> Upsert(TEntity entity);
        Task<IEnumerable<TEntity>> UpdateAll(IEnumerable<TEntity> entities);
        Task<TEntity> Delete(TEntity entity);
        Task<bool> Exists(Func<TEntity, bool> linqExpression = null);
    }

    public class IncludeBuilder<TEntity> where TEntity : IBaseEntity
    {
        public static IncludeBuilder<TEntity, Y> Include<Y>(Expression<Func<TEntity, Y>> initialInclude) where Y : IBaseEntity
        {
            return new IncludeBuilder<TEntity, Y>(initialInclude);
        }

        public static IncludeBuilder<TEntity, Y> Include<Y>(Expression<Func<TEntity, ICollection<Y>>> initialInclude) where Y : IBaseEntity
        {
            return new IncludeBuilder<TEntity, Y>(initialInclude);
        }
    }

    public class IncludeBuilder<TEntity, YEntity> where TEntity : IBaseEntity where YEntity : IBaseEntity
    {
        private string IncludeExpression = "";

        public IncludeBuilder(Expression<Func<TEntity, YEntity>> initialInclude)
        {
            IncludeExpression = initialInclude.Body.Type.Name;
        }

        public IncludeBuilder(Expression<Func<TEntity, ICollection<YEntity>>> initialInclude)
        {
            IncludeExpression = initialInclude.Body.Type.GenericTypeArguments[0].Name;
        }

        private IncludeBuilder(string previousExpression, string addition)
        {
            IncludeExpression = new StringBuilder().Append(previousExpression).Append(".").Append(addition).ToString();
        }

        public IncludeBuilder<YEntity, Z> ThenInclude<Z>(Expression<Func<YEntity, Z>> nextExpression) where Z : IBaseEntity
        {
            var includeName = nextExpression.Body.Type.Name;
            return new IncludeBuilder<YEntity, Z>(IncludeExpression, includeName);
        }

        public IncludeBuilder<YEntity, Z> ThenInclude<Z>(Expression<Func<YEntity, ICollection<Z>>> nextExpression) where Z : IBaseEntity
        {
            var includeName = nextExpression.Body.Type.GenericTypeArguments[0].Name;
            return new IncludeBuilder<YEntity, Z>(IncludeExpression, includeName);
        }

        public IncludeBuilderResult Done()
        {
            return new IncludeBuilderResult(IncludeExpression);
        }
    }

    public class IncludeBuilderResult
    {
        private string IncludeExpression = "";

        public IncludeBuilderResult(string includeExpression)
        {
            IncludeExpression = includeExpression;
        }

        public string GetInclude()
        {
            return IncludeExpression;
        }
    }
}
