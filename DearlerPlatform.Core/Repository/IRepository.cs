using DearlerPlatform.Core.GlobalDto;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DearlerPlatform.Core.Repository
{
    public interface IRepository<TEntity>
        where TEntity : class
    {
         List<TEntity> GetList();

        /// <summary>
        /// 得到数据(返回List)
        /// </summary>
        /// <returns>具体数据类型</returns>
         List<TEntity> GetList(Func<TEntity, bool> predicate);

         Task<List<TEntity>> GetListAsync(PageWithSortDto pageWithSortDto);

        /// <summary>
        /// 得到数据(返回List)
        /// </summary>
        /// <returns>具体数据类型</returns>
         Task<List<TEntity>> GetListAsync();

        IQueryable<TEntity> GetIQueryable();

        /// <summary>
        /// 得到数据(返回List)
        /// </summary>
        /// <returns>具体数据类型</returns>
          Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate);

          Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate, PageWithSortDto pageWithSortDto);

        /// <summary>
        /// 得到数据
        /// </summary>
        /// <returns>具体数据类型</returns>
         TEntity Get(Func<TEntity, bool> predicate);

        /// <summary>
        /// 得到数据
        /// </summary>
        /// <returns>具体数据类型</returns>
         Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 插入值
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
         TEntity Insert(TEntity entity);

        /// <summary>
        /// 插入值
        /// </summary>
        /// <returns>具体数据类型</returns>
         Task<TEntity> InsertAsync(TEntity entity);

        /// <summary>
        /// 删除值
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
         TEntity Delete(TEntity entity);

        /// <summary>
        ///  删除值
        /// </summary>
        /// <returns>具体数据类型</returns>
         Task<TEntity> DeleteAsync(TEntity entity);

        /// <summary>
        /// 更新值
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
         TEntity Update(TEntity entity);

        /// <summary>
        ///  更新值
        /// </summary>
        /// <returns>具体数据类型</returns>
         Task<TEntity> UpdateAsync(TEntity entity);
    }
}