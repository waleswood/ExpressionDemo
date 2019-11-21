using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ExpressionDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            //假如我们要拼接x=>x.Id==1，假如x的类型为SearchInfo
            var parameterExp = Expression.Parameter(typeof(SearchInfo), "x"); //参数表达式
            MemberExpression left1 = Expression.Property(parameterExp, "Id");
            var right1 = Expression.Constant(1);
            BinaryExpression where1 = Expression.Equal(left1, right1); // x=>x.Id==1

            MemberExpression left2 = Expression.Property(parameterExp, "Name");
            ConstantExpression right2 = Expression.Constant("张三");
            BinaryExpression where2 = Expression.Equal(left2, right2); //x => x.Name == "张三"
            BinaryExpression merge = Expression.And(where1, where2); // and 关系

            Expression<Func<SearchInfo, bool>> lambda = Expression.Lambda<Func<SearchInfo, bool>>(merge, parameterExp);//生成最后需要的带参数的表达式树.


            List<SearchInfo> list = new List<SearchInfo>();
            list.Add(new SearchInfo { Id = 1, Name = "张三" });
            list.Add(new SearchInfo { Id = 2, Name = "张三" });
            list.Add(new SearchInfo { Id = 1, Name = "李四" });
            var result = list.Where(lambda.Compile()).ToList();
            Console.WriteLine(result.Count);
            Console.ReadLine();
        }
        // 参考：https://www.cnblogs.com/bubugao/p/lambda.html
    }

    /// <summary>
    /// 测试类
    /// </summary>
    public class SearchInfo
    {
        public string Name { get; set; }

        public string Code { get; set; }

        public int Id { get; set; }

        public string Addr { get; set; }

        public string Res { get; set; }
    }
}
