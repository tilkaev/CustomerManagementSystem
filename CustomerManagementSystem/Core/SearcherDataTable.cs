using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerManagementSystem.Core
{
    public class SearcherDataTable
    {
        private static DataTable Search(object[] search_words, int[] search_columns, DataTable _dataTable)
        {
            var newDataTable = _dataTable.Copy();
            foreach (DataRow item in newDataTable.Rows) // Поиск по строкам
            {
                int indextodelete = 0;
                for (int i_column = 0; i_column < search_columns.Length; i_column++)
                {
                    bool exist = item[search_columns[i_column]].ToString().ToLower().Contains(search_words[i_column].ToString().ToLower());

                    if (exist)  // Если присутсвтует
                    {
                        indextodelete++;
                    }
                }
                if (indextodelete != search_words.Length) // если не прошел проверку
                {
                    item.Delete();  // удалить строку
                }

            }
            newDataTable.AcceptChanges();

            return newDataTable;
        }


        public static DataTable WordSearch(object search_word, DataTable dataTable) // Поиск по всей таблицы одного слова
        {

            var newDataTable = dataTable.Copy();
            int num_column = newDataTable.Columns.Count;


            foreach (DataRow item in newDataTable.Rows)
            {
                int skip_del = 0;

                for (int i = 0; i < num_column; i++) // Проход по строкам
                {
                    bool booling = item[i].ToString().ToLower().Contains(search_word.ToString().ToLower());

                    if (booling)
                    {
                        skip_del = 0;
                        break;
                    }
                    skip_del++;
                }
                if (skip_del == num_column)
                {
                    item.Delete();
                }
            }
            newDataTable.AcceptChanges();
            return newDataTable;
        }


        public static DataTable ColumnSearch(object search_word, int search_column, DataTable dataTable)
        {
            return Search(new object[] { search_word }, new int[] { search_column }, dataTable);
        }



        public object[] search_words { get; set; }
        public int[] search_columns { get; set; }
        public DataTable dataTable { get; set; }

        public DataTable MultiSearch()
        {
            if (search_words == null | search_columns == null | dataTable == null)
            {
                throw new Exception("Необходимо задать все параметры!");
            }
            if (search_words.Length != search_columns.Length)
            {
                throw new Exception("search_words.Length != search_columnss.Length");
            }

            return Search(search_words, search_columns, dataTable);
        }


    }
}
