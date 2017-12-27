namespace FluorineFx.Util
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Reflection;

    public abstract class CollectionUtils
    {
        protected CollectionUtils()
        {
        }

        public static void AddRange<T>(IList<T> initial, IEnumerable<T> collection)
        {
            if (initial == null)
            {
                throw new ArgumentNullException("initial");
            }
            if (collection != null)
            {
                foreach (T local in collection)
                {
                    initial.Add(local);
                }
            }
        }

        public static Array CreateArray(Type type, ICollection collection)
        {
            ValidationUtils.ArgumentNotNull(collection, "collection");
            if (collection is Array)
            {
                return (collection as Array);
            }
            List<object> list = new List<object>();
            foreach (object obj2 in collection)
            {
                list.Add(obj2);
            }
            return list.ToArray();
        }

        public static object CreateGenericList(Type listType)
        {
            ValidationUtils.ArgumentNotNull(listType, "listType");
            return ReflectionUtils.CreateGeneric(typeof(List<>), listType, new object[0]);
        }

        public static List<T> CreateList<T>(params T[] values)
        {
            return new List<T>(values);
        }

        public static List<T> CreateList<T>(ICollection collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }
            T[] array = new T[collection.Count];
            collection.CopyTo(array, 0);
            return new List<T>(array);
        }

        public static IList CreateList(Type listType)
        {
            IList list;
            ValidationUtils.ArgumentNotNull(listType, "listType");
            bool flag = false;
            if (listType.IsArray)
            {
                list = new List<object>();
                flag = true;
            }
            else
            {
                Type type;
                if (!ReflectionUtils.IsSubClass(listType, typeof(ReadOnlyCollection<>), out type))
                {
                    if (!typeof(IList).IsAssignableFrom(listType) || !ReflectionUtils.IsInstantiatableType(listType))
                    {
                        throw new Exception(string.Format("Cannot create and populate list type {0}.", listType));
                    }
                    list = (IList) Activator.CreateInstance(listType);
                }
                else
                {
                    Type type2 = type.GetGenericArguments()[0];
                    Type type3 = ReflectionUtils.MakeGenericType(typeof(IEnumerable<>), new Type[] { type2 });
                    bool flag2 = false;
                    foreach (ConstructorInfo info in listType.GetConstructors())
                    {
                        IList<ParameterInfo> parameters = info.GetParameters();
                        if ((parameters.Count == 1) && type3.IsAssignableFrom(parameters[0].ParameterType))
                        {
                            flag2 = true;
                            break;
                        }
                    }
                    if (!flag2)
                    {
                        throw new Exception(string.Format("Readonly type {0} does not have a public constructor that takes a type that implements {1}.", listType, type3));
                    }
                    list = (IList) CreateGenericList(type2);
                    flag = true;
                }
            }
            if (flag)
            {
                if (listType.IsArray)
                {
                    return ((List<object>) list).ToArray();
                }
                if (ReflectionUtils.IsSubClass(listType, typeof(ReadOnlyCollection<>)))
                {
                    list = (IList) Activator.CreateInstance(listType, new object[] { list });
                }
            }
            return list;
        }

        public static List<T> Distinct<T>(List<T> collection)
        {
            List<T> list = new List<T>();
            foreach (T local in collection)
            {
                if (!list.Contains(local))
                {
                    list.Add(local);
                }
            }
            return list;
        }

        public static List<List<T>> Flatten<T>(params IList<T>[] lists)
        {
            List<List<T>> flattenedResult = new List<List<T>>();
            Dictionary<int, T> currentSet = new Dictionary<int, T>();
            Recurse<T>(new List<IList<T>>(lists), 0, currentSet, flattenedResult);
            return flattenedResult;
        }

        public static object GetSingleItem(IList list)
        {
            return GetSingleItem(list, false);
        }

        public static object GetSingleItem(IList list, bool returnDefaultIfEmpty)
        {
            if (list.Count == 1)
            {
                return list[0];
            }
            if (!returnDefaultIfEmpty || (list.Count != 0))
            {
                throw new Exception(string.Format("Expected single item in list but got {1}.", list.Count));
            }
            return null;
        }

        public static bool IsListType(Type type)
        {
            ValidationUtils.ArgumentNotNull(type, "listType");
            return (type.IsArray || (typeof(IList).IsAssignableFrom(type) || ReflectionUtils.IsSubClass(type, typeof(IList<>))));
        }

        public static bool IsNullOrEmpty<T>(ICollection<T> collection)
        {
            if (collection != null)
            {
                return (collection.Count == 0);
            }
            return true;
        }

        public static bool IsNullOrEmpty(ICollection collection)
        {
            if (collection != null)
            {
                return (collection.Count == 0);
            }
            return true;
        }

        public static bool IsNullOrEmptyOrDefault<T>(IList<T> list)
        {
            return (IsNullOrEmpty<T>(list) || ReflectionUtils.ItemsUnitializedValue<T>(list));
        }

        public static bool ListEquals<T>(IList<T> a, IList<T> b)
        {
            if ((a == null) || (b == null))
            {
                return ((a == null) && (b == null));
            }
            if (a.Count != b.Count)
            {
                return false;
            }
            EqualityComparer<T> comparer = EqualityComparer<T>.Default;
            for (int i = 0; i < a.Count; i++)
            {
                if (!comparer.Equals(a[i], b[i]))
                {
                    return false;
                }
            }
            return true;
        }

        public static IList<T> Minus<T>(IList<T> list, IList<T> minus)
        {
            ValidationUtils.ArgumentNotNull(list, "list");
            List<T> list2 = new List<T>(list.Count);
            foreach (T local in list)
            {
                if (!((minus != null) && minus.Contains(local)))
                {
                    list2.Add(local);
                }
            }
            return list2;
        }

        private static void Recurse<T>(IList<IList<T>> global, int current, Dictionary<int, T> currentSet, List<List<T>> flattenedResult)
        {
            IList<T> list = global[current];
            for (int i = 0; i < list.Count; i++)
            {
                currentSet[current] = list[i];
                if (current == (global.Count - 1))
                {
                    List<T> item = new List<T>();
                    for (int j = 0; j < currentSet.Count; j++)
                    {
                        item.Add(currentSet[j]);
                    }
                    flattenedResult.Add(item);
                }
                else
                {
                    Recurse<T>(global, current + 1, currentSet, flattenedResult);
                }
            }
        }

        public static IList<T> Slice<T>(IList<T> list, int? start, int? end)
        {
            return Slice<T>(list, start, end, null);
        }

        public static IList<T> Slice<T>(IList<T> list, int? start, int? end, int? step)
        {
            if (list == null)
            {
                throw new ArgumentNullException("list");
            }
            if (step == 0)
            {
                throw new ArgumentException("Step cannot be zero.", "step");
            }
            List<T> list2 = new List<T>();
            if (list.Count != 0)
            {
                int? nullable = step;
                int num = nullable.HasValue ? nullable.GetValueOrDefault() : 1;
                nullable = start;
                int num2 = nullable.HasValue ? nullable.GetValueOrDefault() : 0;
                nullable = end;
                int num3 = nullable.HasValue ? nullable.GetValueOrDefault() : list.Count;
                num2 = (num2 < 0) ? (list.Count + num2) : num2;
                num3 = (num3 < 0) ? (list.Count + num3) : num3;
                num2 = Math.Max(num2, 0);
                num3 = Math.Min(num3, list.Count - 1);
                for (int i = num2; i < num3; i += num)
                {
                    list2.Add(list[i]);
                }
            }
            return list2;
        }
    }
}

