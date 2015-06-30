using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Common.Web.Mvc
{
    /// <summary>
    /// Extension methods on IEnumerable.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Converts the source sequence into an IEnumerable of SelectListItem
        /// </summary>
        /// <param name="items">Source sequence</param>
        /// <param name="namePredicate">Lambda that specifies the name for the SelectList items</param>
        /// <param name="valuePredicate">Lambda that specifies the value for the SelectList items</param>
        /// <returns>IEnumerable of SelectListItem</returns>
        public static IEnumerable<SelectListItem> ToSelectList<TItem, TValue>(this IEnumerable<TItem> items, Func<TItem, TValue> valuePredicate, Func<TItem, string> namePredicate)
        {
            return items.ToSelectList(valuePredicate, namePredicate, x => false, x => false, null);
        }

        /// <summary>
        /// Converts the source sequence into an IEnumerable of SelectListItem
        /// </summary>
        /// <param name="items">Source sequence</param>
        /// <param name="namePredicate">Lambda that specifies the name for the SelectList items</param>
        /// <param name="valuePredicate">Lambda that specifies the value for the SelectList items</param>
        /// <param name="selectedValuePredicate">Lambda that specifies whether the item should be selected</param>
        /// <returns>IEnumerable of SelectListItem</returns>
        public static IEnumerable<SelectListItem> ToSelectList<TItem, TValue>(this IEnumerable<TItem> items, Func<TItem, TValue> valuePredicate, Func<TItem, string> namePredicate, Func<TItem, bool> selectedValuePredicate)
        {
            return items.ToSelectList(valuePredicate, namePredicate, selectedValuePredicate, x => false, null);
        }

        /// <summary>
        /// Converts the source sequence into an IEnumerable of SelectListItem
        /// </summary>
        /// <param name="items">Source sequence</param>
        /// <param name="namePredicate">Lambda that specifies the name for the SelectList items</param>
        /// <param name="valuePredicate">Lambda that specifies the value for the SelectList items</param>
        /// <param name="selectedValuePredicate">Lambda that specifies whether the item should be selected</param>
        /// <param name="disabledPredicate">Lambda that specifies whether the item should be disabled</param>
        /// <returns>IEnumerable of SelectListItem</returns>
        public static IEnumerable<SelectListItem> ToSelectList<TItem, TValue>(this IEnumerable<TItem> items, Func<TItem, TValue> valuePredicate, Func<TItem, string> namePredicate, Func<TItem, bool> selectedValuePredicate, Func<TItem, bool> disabledPredicate)
        {
            return items.ToSelectList(valuePredicate, namePredicate, selectedValuePredicate, disabledPredicate, null);
        }

        /// <summary>
        /// Converts the source sequence into an IEnumerable of SelectListItem
        /// </summary>
        /// <param name="items">Source sequence</param>
        /// <param name="namePredicate">Lambda that specifies the name for the SelectList items</param>
        /// <param name="valueSelector">Lambda that specifies the value for the SelectList items</param>
        /// <param name="selectedValuePredicate">Lambda that specifies whether the item should be selected</param>
        /// <param name="disabledPredicate">Lambda that specifies whether the item should be disabled</param>
        /// <param name="emptyText">Text of first empty item</param>
        /// <returns>IEnumerable of SelectListItem</returns>
        public static IEnumerable<SelectListItem> ToSelectList<TItem, TValue>(this IEnumerable<TItem> items, Func<TItem, TValue> valueSelector, Func<TItem, string> namePredicate, Func<TItem, bool> selectedValuePredicate, Func<TItem, bool> disabledPredicate, string emptyText)
        {
            if (!string.IsNullOrEmpty(emptyText))
                yield return new SelectListItem { Text = emptyText };

            foreach (var item in items)
            {
                var value = valueSelector(item);

                yield return new SelectListItem
                    {
                        Text = namePredicate(item),
                        Value = value.ToString(),
                        Selected = selectedValuePredicate(item),
                        Disabled = disabledPredicate(item)
                    };
            }
        }

        /// <summary>
        /// Converts the source sequence into an IEnumerable of ExtendedSelectListItem
        /// </summary>
        /// <returns>IEnumerable of ExtendedSelectListItem</returns>
        public static IEnumerable<ExtendedSelectListItem> ToExtendedSelectList<TItem, TValue>(this IEnumerable<TItem> items, Func<TItem, TValue> valuePredicate, Func<TItem, string> namePredicate, Func<TItem, bool> selectedValuePredicate)
        {
            return items.ToExtendedSelectList(valuePredicate, namePredicate, selectedValuePredicate, x => null);
        }

        /// <summary>
        /// Converts the source sequence into an IEnumerable of ExtendedSelectListItem
        /// </summary>
        /// <returns>IEnumerable of ExtendedSelectListItem</returns>
        public static IEnumerable<ExtendedSelectListItem> ToExtendedSelectList<TItem, TValue>(this IEnumerable<TItem> items, Func<TItem, TValue> valuePredicate, Func<TItem, string> namePredicate, Func<TItem, bool> selectedValuePredicate, Func<TItem, object> htmlAttributesPredicate)
        {
            foreach (var item in items)
            {
                var value = valuePredicate(item);

                yield return new ExtendedSelectListItem
                {
                    Text = namePredicate(item),
                    Value = value.ToString(),
                    Selected = selectedValuePredicate(item),
                    HtmlAttributes = htmlAttributesPredicate(item)
                };
            }
        }

        public static T PickRandom<T>(this IEnumerable<T> source)
        {
            return source.PickRandom(1).Single();
        }

        public static IEnumerable<T> PickRandom<T>(this IEnumerable<T> source, int count)
        {
            return source.Shuffle().Take(count);
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            return source.OrderBy(x => Guid.NewGuid());
        }
    }
}