using Dapper;
using OutlookIMExToolsAddIn1.Helpers;
using System;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;

namespace OutlookIMExToolsAddIn1.Usecases
{
    public class ThunderbirdAddrBookUsecase
    {
        public async Task<ThunderbirdAddrBookSqlite> LoadAsync(string sqliteFile)
        {
            using (var db = new SQLiteConnection(
                new SQLiteConnectionStringBuilder
                {
                    DataSource = sqliteFile,
                }
                    .ConnectionString
            ))
            {
                db.Open();

                var ds = new ThunderbirdAddrBookSqlite
                {
                    Properties = (await db.QueryAsync<ThunderbirdAddrBookSqlite.CProperty>(
                        "SELECT card as 'Card', name as 'Name', value as 'Value' FROM properties"
                    )).ToList(),

                    Lists = (await db.QueryAsync<ThunderbirdAddrBookSqlite.CList>(
                        "SELECT uid as 'Uid', name as 'Name', nickName as 'NickName', description as 'Description' FROM lists"
                    )).ToList(),

                    ListCards = (await db.QueryAsync<ThunderbirdAddrBookSqlite.CListCard>(
                        "SELECT list as 'List', card as 'Card' FROM list_cards"
                    )).ToList()
                };

                return ds;
            }
        }
    }
}