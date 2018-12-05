using System;
using Floomeen.Attributes;
using Floomeen.Meen.Interfaces;

namespace Floomeen.Utils
{
    public static class FellowExtensions
    {
        public static string GetPropNameByAttribute<TAttribute>(this IFellow fellow) where TAttribute : Attribute
        {
            return FloomeenExtensions.GetPropNameByAttribute<TAttribute>(fellow);
        }

        public static object GetPropValueByAttribute<TAttribute>(this IFellow fellow) where TAttribute : Attribute
        {
            return FloomeenExtensions.GetPropValueByAttribute<TAttribute>(fellow);
        }

        public static Type GetPropTypeByAttribute<TAttribute>(this IFellow fellow) where TAttribute : Attribute
        {
            return FloomeenExtensions.GetPropTypeByAttribute<TAttribute>(fellow);
        }

        public static void SetPropValueByAttribute<TAttribute>(this IFellow fellow, object value) where TAttribute : Attribute
        {
            FloomeenExtensions.SetPropValueByAttribute<TAttribute>(fellow, value);
        }


        public static object Id(this IFellow fellow)
        {
            return fellow.GetPropValueByAttribute<FloomeenId>();
        }

        public static void Id(this IFellow fellow, object value)
        {
            fellow.SetPropValueByAttribute<FloomeenId>(value);
        }

        public static string State(this IFellow fellow)
        {
            return fellow.GetPropValueByAttribute<FloomeenState>()?.ToString();
        }

        public static void State(this IFellow fellow, object value)
        {
            fellow.SetPropValueByAttribute<FloomeenState>(value);
        }

        public static string Machine(this IFellow fellow)
        {
            return fellow.GetPropValueByAttribute<FloomeenMachine>()?.ToString();
        }

        public static void Machine(this IFellow fellow, object value)
        {
            fellow.SetPropValueByAttribute<FloomeenMachine>(value);
        }

        public static string StateData(this IFellow fellow)
        {
            return fellow.GetPropValueByAttribute<FloomeenStateData>()?.ToString();
        }

        public static void StateData(this IFellow fellow, object value)
        {
            fellow.SetPropValueByAttribute<FloomeenStateData>(value);
        }

        public static DateTime ChangedOn(this IFellow fellow)
        {
            return (DateTime) fellow.GetPropValueByAttribute<FloomeenChangedOn>();
        }

        public static void ChangedOn(this IFellow fellow, object value)
        {
            fellow.SetPropValueByAttribute<FloomeenChangedOn>(value);
        }

    }
}
