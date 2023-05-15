using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DearlerPlatform.Common.RedisModule
{
    public partial class RedisWorker
    {
        /// <summary>
        /// 设置单一的hash键值对
        /// </summary>
        /// <param name="key">主键</param>
        /// <param name="values">键值对(字典key是hash键,value是hash值)</param>
        public void SetHashMemory(string key,Dictionary<string,string> values)
        {
            var hashEntrys = new List<HashEntry>();
            foreach (var value in values)
            {
                hashEntrys.Add(new HashEntry(value.Key,value.Value));
            }
            SetHashMemory(key, hashEntrys.ToArray());
        }

        /// <summary>
        /// 将数据集合存入到redis
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="key">主键</param>
        /// <param name="entities">存到对应主键下的键值对</param>
        /// <param name="func">主键格式中:后面的内容作为数据筛选用</param>
        public void SetHashMemory<T>(string key, IEnumerable<T> entities, Func<T, IEnumerable<string>> func)
        {
            //fun的第二个参数是返回值
            //这里T是传入的一个类,我们调用时候是传递这个类中我们需要筛选的属性,然后func会自动把我们需要的数据筛选返回成数组
            //一下为m的使用方法
            //m =>
            //{
            //    //m表示我们的T就是实际的类
            //    //这里我们就是将实际类型中的属性存到list中
            //    //然后返回
            //    var list = new List<string>();
            //    list.Add(m.Id.toString());
            //    list.Add(m.UserName);
            //    return list;
            //}
            //简单写法
            //m =>
            //{
            //   //lambda中直接new的数组就代表这个数组直接被返回,这个数组中有两个元素
            //m =>new []{
            //    m.Id.toString(),m.UserName
            //}
            //}
            Type type = typeof(T);
            foreach (var entity in entities)
            {
                var valueKeys = func(entity);
                SetHashMemory($"{key}:{string.Join(":", valueKeys)}", entity, type);
            }
        }

        public void SetHashMemory<T>(string key, T entity, Type type = null)
        {
            type ??= typeof(T);
            List<HashEntry> hashEntntries = new();
            PropertyInfo[] props = type.GetProperties();
            foreach (var prop in props)
            {
                string name = prop.Name;
                object value = prop.GetValue(entity);
                if (value.GetType().Name == "Boolean") value = (bool)value ? 1 : 0;
                {
                    hashEntntries.Add(new HashEntry(name, value?.ToString()));
                }
            }
            SetHashMemory(key, hashEntntries.ToArray());
        }

        public void SetHashMemory(string key, params HashEntry[] entries)
        {
            RedisCore.Db.HashSet(key, entries);
        }

        /// <summary>
        /// 获取单一的对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T GetOneHashMemory<T>(string key)where T : new()
        {
            T t = new();
            //这里拿到的是一个集合
            //循环这个集合,拿到里面的key和value
            var res = RedisCore.Db.HashGetAll(key);
            var props = t.GetType().GetProperties();
            foreach (var item in res)
            {
                foreach (var prop in props)
                {
                    //item.Name就是键值对的键key
                    if (prop.Name==item.Name)
                    {
                        //setValue(需要被设置值的实体,值的来源)
                        //Convert.ChangeType(当前类型,需要的类型)将一个类型转换成另一个类型
                        prop.SetValue(t, Convert.ChangeType(item.Value,prop.PropertyType));
                        break;
                    }
                }
            }
            return t;
        }

        /// <summary>
        /// 获取对象的集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public List<T> GetHashMemory<T>(string keyLike) where T : new()
        {
            var keys = GetKeys(keyLike);
            List<T> ts = new List<T>();
            foreach (var key in keys)
            {
                T t = new();
                var res = RedisCore.Db.HashGetAll(key);
                var props = t.GetType().GetProperties();
                foreach (var item in res)
                {
                    foreach (var prop in props)
                    {
                        if (prop.Name == item.Name)
                        {
                            prop.SetValue(t, Convert.ChangeType(item.Value, prop.PropertyType));
                            break;
                        }
                    }
                }
                ts.Add(t);
            }
            return ts;
        }


    }
}
