using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DearlerPlatform.Common.RedisModule
{
    public partial class RedisWorker:IRedisWorker
    {
        public RedisCore RedisCore { get; }

        public RedisWorker(RedisCore redisCore)
        {
            RedisCore = redisCore;
        }

        /// <summary>
        /// 通过Scan获取有适配的KEY
        /// </summary>
        /// <param name="key">可以带适配符的key</param>
        /// <returns></returns>
        public List<string> GetKeys(string key)
        {
            //这里需要再连接数据库的时候加上
            //configurationOptions.AllowAdmin = true;

            List<string> keyList = new();
            var eps = RedisCore.Conn.GetEndPoints();
            var ep = eps[0];
            //通过EndPoints拿到Redis服务
            var server = RedisCore.Conn.GetServer(ep);
            //通过Serviver拿到Redis中所有符合条件的Key
            var keys = server.Keys(0, key).ToList();
            keys.ForEach(k =>
            {
                keyList.Add(k.ToString());
            });
            return keyList;
        }

        /// <summary>
        /// 根据主key来删除元素
        /// </summary>
        /// <param name="key"></param>
        public void RemoveKey(string key)
        {
            RedisCore.Db.KeyDelete(key);
        }

    }
}
