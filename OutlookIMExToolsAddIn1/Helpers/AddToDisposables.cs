using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutlookIMExToolsAddIn1.Helpers
{
    internal static class AddToDisposables
    {
        public static T AddTo<T>(this T item, List<IDisposable> disposables) where T : IDisposable
        {
            if (disposables == null)
            {
                throw new ArgumentNullException(nameof(disposables), "The disposables list cannot be null.");
            }
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item), "The item to add cannot be null.");
            }
            disposables.Add(item);
            return item;
        }
    }
}
