using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DearlerPlatform.Common.RedisModule
{
    /// <summary>
    /// 核心类，连接redis并获取redis数据库
    /// </summary>
    public class RedisCore
    {
        //接收连接对象的
        public ConnectionMultiplexer Conn { get; set; }

        //拿到数据库对象
        public IDatabase Db { get; set; }

        public RedisCore(IConfiguration configuration)
        {
            var redisConnectionStr = configuration["Redis"];

            //对redis进行连接,这里是连接字符串
            ConfigurationOptions configurationOptions =
                                 ConfigurationOptions.Parse(redisConnectionStr);
            //如果没有这句话,我们无法模糊搜索key,因为权限不够
            configurationOptions.AllowAdmin = true;
            Conn = ConnectionMultiplexer.Connect(configurationOptions);

            Db = Conn.GetDatabase();
        }

        /// <summary>
        /// 根据主key来删除元素
        /// </summary>
        /// <param name="key"></param>
        public void RemoveKey(string key)
        {
            Db.KeyDelete(key);
        }
    }
}
