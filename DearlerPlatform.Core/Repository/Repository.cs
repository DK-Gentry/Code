using DearlerPlatform.Core.GlobalDto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DearlerPlatform.Core.Repository
{
    public class Repository<TEntity>: IRepository<TEntity> where TEntity:class
    {
        private readonly DealerPlatformContext _context;

        public Repository(DealerPlatformContext context)
        {
            this._context = context;
        }

        /// <summary>
        /// 得到数据(返回List)
        /// </summary>
        /// <returns>具体数据类型</returns>
        public List<TEntity> GetList()
        {
            return _context.Set<TEntity>().ToList();
        }
        /// <summary>
        /// 得到数据(返回List)
        /// </summary>
        /// <returns>具体数据类型</returns>
        public List<TEntity> GetList(Func<TEntity, bool> predicate)
        {
            var dbSet = _context.Set<TEntity>();
            return dbSet.Where(predicate).ToList();
        }

        /// <summary>
        /// 得到数据(返回List)
        /// </summary>
        /// <returns>具体数据类型</returns>
        public async Task<List<TEntity>> GetListAsync()
        {
            return await GetListAsync(new PageWithSortDto());
        }

        public async Task<List<TEntity>> GetListAsync(PageWithSortDto pageWithSortDto)
        {
            int skip = (pageWithSortDto.PageIndex - 1) * pageWithSortDto.PageSize; 
            var dbSet = _context.Set<TEntity>();
            if (pageWithSortDto.orderType==PageWithSortDto.OrderType.Asc)
            {
                return await dbSet.OrderBy(m => pageWithSortDto.Sort).Skip(skip).Take(pageWithSortDto.PageSize).ToListAsync();
            }
            else
            {
                //OrderByDescending倒序排序
                return await dbSet.OrderByDescending(m => pageWithSortDto.Sort).Skip(skip).Take(pageWithSortDto.PageSize).ToListAsync();
            }
           
        }
        public IQueryable<TEntity> GetIQueryable()
        {
            var dbSet = _context.Set<TEntity>();
            return dbSet;
        }

        /// <summary>
        /// 得到数据(返回List)
        /// </summary>
        /// <returns>具体数据类型</returns>
        public async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity,bool>> predicate)
        {
            var dbSet = _context.Set<TEntity>();
            return await dbSet.Where(predicate).ToListAsync();
        }
        public async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate, PageWithSortDto pageWithSortDto)
        {
            //这里形参是表达式目录树
            //为何要用表达式目录树？
            //因为如果直接传递委托where方法会自动重载的不是返回lQueryable所以没有
            //ToListAsync()方法,所以我们这里选择使用表达式目录树这样
            //where重载的就拥有这个方法

            //表达式目录树是什么？
            //是表达式 
            int skip = (pageWithSortDto.PageIndex - 1) * pageWithSortDto.PageSize;
            var dbSet = _context.Set<TEntity>();
            if (pageWithSortDto.orderType == PageWithSortDto.OrderType.Asc)
            {
                return await dbSet.Where(predicate).OrderBy(m => pageWithSortDto.Sort).Skip(skip).Take(pageWithSortDto.PageSize).ToListAsync();
            }
            else
            {
                //OrderByDescending倒序排序
                return await dbSet.Where(predicate).OrderByDescending(m => pageWithSortDto.Sort).Skip(skip).Take(pageWithSortDto.PageSize).ToListAsync();
            }
        }
        /// <summary>
        /// 得到数据
        /// </summary>
        /// <returns>具体数据类型</returns>
        public TEntity Get(Func<TEntity, bool> predicate)
        {
            var dbSet = _context.Set<TEntity>();
            return dbSet.FirstOrDefault(predicate);
        }
        /// <summary>
        /// 得到数据
        /// </summary>
        /// <returns>具体数据类型</returns>
        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var dbSet = _context.Set<TEntity>();
            return await dbSet.FirstOrDefaultAsync(predicate);
        }
        /// <summary>
        /// 插入值
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public TEntity Insert(TEntity entity)
        {
            var dbSet = _context.Set<TEntity>();
            var res = dbSet.Add(entity).Entity;
            _context.SaveChanges();
            return res;
        }
        /// <summary>
        /// 插入值
        /// </summary>
        /// <returns>具体数据类型</returns>
        public async Task<TEntity> InsertAsync(TEntity entity)
        {
            var dbSet = _context.Set<TEntity>();
            var res = (await dbSet.AddAsync(entity)).Entity;
            _context.SaveChanges();
            return res;
        }
        /// <summary>
        /// 删除值
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public TEntity Delete(TEntity entity)
        {
            var dbSet = _context.Set<TEntity>();
            var res = dbSet.Remove(entity).Entity;
            _context.SaveChanges();
            return res;
        }
        /// <summary>
        ///  删除值
        /// </summary>
        /// <returns>具体数据类型</returns>
        public async Task<TEntity> DeleteAsync(TEntity entity)
        {
            var dbSet = _context.Set<TEntity>();
            var res = dbSet.Remove(entity).Entity;
            _context.SaveChangesAsync();
            return res;
        }
        /// <summary>
        /// 更新值
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public TEntity Update(TEntity entity)
        {
            var dbSet = _context.Set<TEntity>();
            var res = dbSet.Update(entity).Entity;
            _context.SaveChanges();
            return res;
        }
        /// <summary>
        ///  更新值
        /// </summary>
        /// <returns>具体数据类型</returns>
        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            var dbSet = _context.Set<TEntity>();
            var res = dbSet.Update(entity).Entity;
            _context.SaveChangesAsync();
            return res;
        }
    }
}
