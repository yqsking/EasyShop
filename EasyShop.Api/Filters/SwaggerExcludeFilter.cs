using EasyShop.Appliction.Commands.User;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace EasyShop.Api.Filters
{
    /// <summary>
    ///配置请求命令忽略参数
    /// </summary>
    public class SwaggerExcludeFilter : ISchemaFilter
    {
        private static readonly IDictionary<Type, IList<string>> _typeExcludeFieldsDict = new Dictionary<Type, IList<string>>();

        static SwaggerExcludeFilter()
        {
            //配置需要忽略的属性
            IgnoreField<UpdateUserCommand>(dto=>dto.Id);

        }

        /// <summary>
        /// 应用过滤规则
        /// </summary>
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            var type = context.ApiModel.Type.GetTypeInfo();

            if (schema?.Properties == null || type == null)
                return;

            var pInfos = new List<PropertyInfo>();

            if (pInfos.Any())
            {
                foreach (var pInfo in pInfos)
                {
                    if (schema.Properties.ContainsKey(pInfo.Name))
                    {
                        schema.Properties.Remove(pInfo.Name);
                    }
                    else
                    {
                        //部分版本，用了首字母小写
                        //属性名，首字母小写
                        var titleCaseLowerPropertieName = pInfo.Name.Substring(0, 1).ToLower() + pInfo.Name.Substring(1); ;

                        if (schema.Properties.ContainsKey(titleCaseLowerPropertieName))
                        {
                            schema.Properties.Remove(titleCaseLowerPropertieName);
                        }
                    }
                }
            }

            if (_typeExcludeFieldsDict.ContainsKey(type))
            {
                var excludeFields = _typeExcludeFieldsDict[type];
                foreach (var field in excludeFields)
                {
                    if (schema.Properties.ContainsKey(field))
                    {
                        schema.Properties.Remove(field);
                    }
                    else
                    {
                        //部分版本，用了首字母小写
                        //属性名，首字母小写
                        var titleCaseLowerPropertieName = field.Substring(0, 1).ToLower() + field.Substring(1); ;

                        if (schema.Properties.ContainsKey(titleCaseLowerPropertieName))
                        {
                            schema.Properties.Remove(titleCaseLowerPropertieName);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 配置忽略的属性
        /// </summary>
        private static void IgnoreField<TDestination>(Expression<Func<TDestination, object>> destinationMemberLambdaExpression)
        {
            var pInfo = GetFieldPropertyInfo(destinationMemberLambdaExpression);

            if (_typeExcludeFieldsDict.ContainsKey(typeof(TDestination)))
            {
                _typeExcludeFieldsDict[typeof(TDestination)].Add(pInfo.Name);
            }
            else
            {
                _typeExcludeFieldsDict.Add(typeof(TDestination), new List<string>() { pInfo.Name });
            }
        }

        /// <summary>
        /// 通过lambdaExpression获取属性PropertyInfo
        /// </summary>
        private static PropertyInfo GetFieldPropertyInfo<TDestination>(Expression<Func<TDestination, object>> destinationMemberLambdaExpression)
        {
            //获取LambdaExpression 的主体 如x => x.b  则获取到 x.b
            var lambdaExpressionBody = ((LambdaExpression)destinationMemberLambdaExpression).Body;
            // x.b 属于 MemberExpression 或者 UnaryExpression 

            if (lambdaExpressionBody is MemberExpression expression)
            {
                //获取Member , 这里不做具体校验，所以调用配置必须是 属性 
                var memberInfo = expression.Member;
                var pInfo = (PropertyInfo)memberInfo;

                return pInfo;
            }
            else
            {
                var unaryExpression = ((UnaryExpression)lambdaExpressionBody);

                var memberExpression = unaryExpression.Operand as MemberExpression;
                var memberInfo = memberExpression.Member;
                var pInfo = (PropertyInfo)memberInfo;

                return pInfo;
            }
        }

    }
}
