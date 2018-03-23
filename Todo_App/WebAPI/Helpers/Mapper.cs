using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace WebAPI.Helpers
{
    public interface IMapper
    {
        void Exclude<T>(Expression<Func<T, Object>> propertySelector);
        void IncludePropertyOfType<T>();
        void CopyProperties(Object src, Object dest);
        void UpdateCollection<TSrc, TDest>(ICollection<TSrc> srcList, ICollection<TDest> destList)
            where TDest : new();
        TDest CreateFrom<TDest>(Object src)
            where TDest : new();
    }

    public class Mapper : IMapper
    {
        Dictionary<string, List<string>> excludes = new Dictionary<string, List<string>>();
        List<string> includes = new List<string>();
        List<string> collectionIncludes = new List<string>();

        public void Exclude<T>(Expression<Func<T, Object>> propertySelector)
        {
            var expression = propertySelector.Body is MemberExpression
                ? (MemberExpression)propertySelector.Body
                : (MemberExpression)((UnaryExpression)propertySelector.Body).Operand;

            string name = expression.Member.Name;

            var t = typeof(T);

            if (this.excludes.TryGetValue(t.FullName, out var excludedSrcProperties))
            {
                excludedSrcProperties.Add(name);
            }
            else
            {
                this.excludes.Add(t.FullName, new List<string> { name });
            }

        }

        public void IncludePropertyOfType<T>()
        {
            this.includes.Add(typeof(T).FullName);
        }

        public void IncludeCollectionOfType<T>()
        {
            this.collectionIncludes.Add(typeof(T).FullName);
        }

        public void CopyProperties(Object src, Object dest)
        // where TDest : new()
        {
            var destType = dest.GetType();

            this.excludes.TryGetValue(src.GetType().FullName, out var excludedSrcProperties);
            this.excludes.TryGetValue(dest.GetType().FullName, out var excludedDestProperties);

            foreach (var srcProperty in src.GetType().GetProperties())
            {
                var isIncludedReferenceType = this.includes != null
                    && this.includes.Contains(srcProperty.PropertyType.FullName);

                var isCollection = srcProperty.PropertyType
                    .GetInterfaces()
                    .Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(ICollection<>));

                var isIncludedCollectionType = isCollection
                    && this.collectionIncludes.Contains(srcProperty.PropertyType.GetGenericArguments()[0].FullName);

                var isSupportedType = srcProperty.PropertyType.IsValueType
                    || srcProperty.PropertyType == typeof(string)
                    || isIncludedReferenceType
                    || isIncludedCollectionType;


                if (!isSupportedType
                    || !srcProperty.CanRead
                    || srcProperty.GetIndexParameters().Length > 0)
                    continue;

                if (excludedSrcProperties != null
                    && excludedSrcProperties.Contains(srcProperty.Name))
                    continue;


                var destProperty = destType.GetProperty(srcProperty.Name);


                // Collection-Handling
                if (isIncludedCollectionType)
                {
                    var srcValue = (ICollection)srcProperty.GetValue(src, null);
                    var destValue = destProperty.GetValue(dest);

                    var genericTypes = new[]{
                        srcValue.GetType().GetGenericArguments()[0],
                        destValue.GetType().GetGenericArguments()[0]
                    };

                    var ungenericUpdateCollection = this.GetType()
                        .GetMethod("UpdateCollection")
                        .MakeGenericMethod(genericTypes);
                    ungenericUpdateCollection.Invoke(this, new[] { srcValue, destValue });

                    continue;
                }

                if (isIncludedReferenceType)
                {
                    // Copy Reference Type
                    var destValue = destProperty.GetValue(dest);
                    if (destValue == null)
                    {
                        destProperty.SetValue(dest, Activator.CreateInstance(destProperty.PropertyType));
                    }

                    var srcValue = srcProperty.GetValue(src);
                    destValue = destProperty.GetValue(dest);
                    this.CopyProperties(srcValue, destValue);
                }
                else
                {
                    // Copy Value Type
                    if (destProperty != null && destProperty.CanWrite)
                    {

                        if (excludedDestProperties != null
                            && excludedDestProperties.Contains(destProperty.Name))
                            continue;

                        destProperty.SetValue(dest, srcProperty.GetValue(src, null), null);
                    }
                }
            }
        }

        public void UpdateCollection<TSrc, TDest>(
            ICollection<TSrc> srcList,
            ICollection<TDest> destList)
            where TDest : new()
        {
            Func<TSrc, TDest, bool> areEqual = (src, dest) =>
            {
                var srcId = typeof(TSrc).GetProperty("Id").GetValue(src, null);
                var destId = typeof(TDest).GetProperty("Id").GetValue(dest, null);
                // Wichtig! "==" funktioniert hier nicht, da Objects vergleichen werden!!!
                return srcId.Equals(destId);
            };

            foreach (var dest in destList)
            {
                var x = srcList.FirstOrDefault(src => areEqual(src, dest));
            }

            // remove removed
            var removedDests = new List<TDest>();
            foreach (var dest in destList)
            {
                if (!srcList.Any(src => areEqual(src, dest)))
                {
                    removedDests.Add(dest);
                }
            }

            removedDests.ForEach(r => destList.Remove(r));

            // add or update
            var newDestObjs = new List<TDest>();
            foreach (var srcObj in srcList)
            {
                var destObjToUpdate = destList.SingleOrDefault(destObj => areEqual(srcObj, destObj));

                if (destObjToUpdate == null)
                {
                    var newDestDto = new TDest();
                    newDestObjs.Add(newDestDto);
                }
                else
                {
                    this.CopyProperties(srcObj, destObjToUpdate);
                }
            }

            newDestObjs.ForEach(destList.Add);
        }

        public TDest CreateFrom<TDest>(Object src)
            where TDest : new()
        {
            var dest = new TDest();

            this.CopyProperties(src, dest);

            return dest;
        }

    }
}