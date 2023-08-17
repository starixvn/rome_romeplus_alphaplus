using System;
using System.Data;

namespace AlphaPlus.User.Models
{
    public class SubAccount
    {
        public SubAccount(DataRow dataRow)
            : this(dataRow, true)
        {
        }

        public SubAccount(DataRow dataRow, bool isGetCreatedDate)
        {
            Id = (int)dataRow["subaccid"];
            UserName = dataRow["username"] as string;

            if (ColumnExists(dataRow, "nickname"))
            {
                NickName = dataRow["nickname"] as string;
            }

            FirstName = dataRow["firstname"] as string;
            LastName = dataRow["lastname"] as string;
            if (isGetCreatedDate)
            {
                CreateDate = (DateTime)dataRow["createdate"];
            }

            IsClosed = (bool)dataRow["closed"];
            PageAccess = dataRow["PageAccess"] as string;
        }

        public SubAccount(IDataReader reader)
            : this(reader, true)
        {
        }

        public SubAccount(IDataReader reader, bool isGetCreatedDate)
        {
            Id = (int)reader["subaccid"];
            UserName = reader["username"] as string;

            if (ColumnExists(reader, "nickname"))
            {
                NickName = reader["nickname"] as string;
            }

            FirstName = reader["firstname"] as string;
            LastName = reader["lastname"] as string;
            if (isGetCreatedDate)
            {
                CreateDate = (DateTime)reader["createdate"];
            }

            IsClosed = (bool)reader["closed"];
            PageAccess = reader["PageAccess"] as string;
        }

        public SubAccount()
        {
            PageAccess = new PageAccess();
        }

        public int Id { get; set; }

        public string UserName { get; set; }

        public string NickName { get; set; }

        public string PassWord { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime CreateDate { get; set; }

        public bool IsClosed { get; set; }

        public PageAccess PageAccess { get; set; }

        public static implicit operator SubAccount(DataRow dataRow)
        {
            return new SubAccount(dataRow);
        }

        private static bool ColumnExists(DataRow dataRow, string columnName)
            => dataRow.Table.Columns.Contains(columnName);

        private static bool ColumnExists(IDataReader reader, string columnName)
        {
            for (int i = 0, fieldCount = reader.FieldCount; i < fieldCount; i++)
            {
                if (reader.GetName(i).Equals(columnName, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }
    }
}