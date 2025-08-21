using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutlookIMExToolsAddIn1.Helpers
{
    public class ThunderbirdAddrBookSqlite
    {
        public List<CProperty> Properties { get; set; } = new List<CProperty>();
        public List<CList> Lists { get; set; } = new List<CList>();
        public List<CListCard> ListCards { get; set; } = new List<CListCard>();

        public class CProperty
        {
            public string Card { get; set; }
            public string Name { get; set; }
            public string Value { get; set; }
        }

        public class CList
        {
            /// <summary>
            /// This links to {@link CListCard.List}
            /// </summary>
            public string Uid { get; set; }
            public string Name { get; set; }
            public string NickName { get; set; }
            public string Description { get; set; }
        }

        public class CListCard
        {
            /// <summary>
            /// This links to {@link CList.Uid}
            /// </summary>
            public string List { get; set; }

            /// <summary>
            /// This links to {@link CProperty.Card}
            /// </summary>
            public string Card { get; set; }
        }
    }
}
