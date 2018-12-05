using System;
using Floomeen.Attributes;
using Floomeen.Meen.Interfaces;

namespace Floomeen.Utils
{
    public static class FellowExtensions
    {
        public static string GetPropNameByAttribute<TAttribute>(this IFellow fellow) where TAttribute : Attribute
        {
            return FloomineExtensions.GetPropNameByAttribute<TAttribute>(fellow);
        }

        public static object GetPropValueByAttribute<TAttribute>(this IFellow fellow) where TAttribute : Attribute
        {
            return FloomineExtensions.GetPropValueByAttribute<TAttribute>(fellow);
        }

        public static Type GetPropTypeByAttribute<TAttribute>(this IFellow fellow) where TAttribute : Attribute
        {
            return FloomineExtensions.GetPropTypeByAttribute<TAttribute>(fellow);
        }

        public static void SetPropValueByAttribute<TAttribute>(this IFellow fellow, object value) where TAttribute : Attribute
        {
            FloomineExtensions.SetPropValueByAttribute<TAttribute>(fellow, value);
        }


        public static string Id(this IFellow fellow)
        {
            return fellow.GetPropValueByAttribute<FloomineId>()?.ToString();
        }

        public static void Id(this IFellow fellow, object value)
        {
            fellow.SetPropValueByAttribute<FloomineId>(value);
        }

        public static string State(this IFellow fellow)
        {
            return fellow.GetPropValueByAttribute<FloomineState>()?.ToString();
        }

        public static void State(this IFellow fellow, object value)
        {
            fellow.SetPropValueByAttribute<FloomineState>(value);
        }

        public static string Machine(this IFellow fellow)
        {
            return fellow.GetPropValueByAttribute<FloomineMachine>()?.ToString();
        }

        public static void Machine(this IFellow fellow, object value)
        {
            fellow.SetPropValueByAttribute<FloomineMachine>(value);
        }

        public static string StateData(this IFellow fellow)
        {
            return fellow.GetPropValueByAttribute<FloomineStateData>()?.ToString();
        }

        public static void StateData(this IFellow fellow, object value)
        {
            fellow.SetPropValueByAttribute<FloomineStateData>(value);
        }

        public static DateTime ChangedOn(this IFellow fellow)
        {
            return (DateTime) fellow.GetPropValueByAttribute<FloomineChangedOn>();
        }

        public static void ChangedOn(this IFellow fellow, object value)
        {
            fellow.SetPropValueByAttribute<FloomineChangedOn>(value);
        }

    }
}
