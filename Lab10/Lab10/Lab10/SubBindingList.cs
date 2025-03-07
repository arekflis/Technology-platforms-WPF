using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;

namespace Lab10
{
    public class SubBindingList<T> : BindingList<T>
    {

        protected override bool SupportsSortingCore => true;
        protected override bool SupportsSearchingCore => true;


        public SubBindingList(){ }

        public SubBindingList(List<T> list) {
            Clear();
            foreach(var element in list)
            {
                Add(element);
            }
        }


        protected override void ApplySortCore(PropertyDescriptor propertyDescriptor, ListSortDirection direction)
        {
            List<T> itemsList = (List<T>)Items;

            if (propertyDescriptor.PropertyType.GetInterface("IComparable") != null)
            {
                if(direction == ListSortDirection.Ascending)
                {
                    itemsList = itemsList.OrderBy(item => propertyDescriptor.GetValue(item)).ToList();
                }
                else
                {
                    itemsList = itemsList.OrderByDescending(item => propertyDescriptor.GetValue(item)).ToList();
                }

                Clear();

                foreach(var item in itemsList)
                {
                    Add(item);
                }
            }
            else
            {
                Environment.Exit(-1);
            }
        }


        protected virtual void SortSubToo(PropertyDescriptor propertyDescriptor, PropertyDescriptor propertyDescriptor1, ListSortDirection direction)
        {
            List<T> itemsList = (List<T>)Items;
            if (propertyDescriptor.PropertyType.GetInterface("IComparable") != null)
            {
                if (direction == ListSortDirection.Ascending)
                {
                    if(propertyDescriptor1 == null) itemsList = itemsList.OrderBy(item => propertyDescriptor.GetValue(item)).ToList();
                    else
                    {
                        itemsList = itemsList.OrderBy(item => { var value = propertyDescriptor.GetValue(item); var value2 = propertyDescriptor1.GetValue(value); return value2; }).ToList();
                    }
                }
                else
                {
                    if(propertyDescriptor1 == null) itemsList = itemsList.OrderByDescending(item => propertyDescriptor.GetValue(item)).ToList();
                    else
                    {
                        itemsList = itemsList.OrderByDescending(item => { var value = propertyDescriptor.GetValue(item); var value2 = propertyDescriptor1.GetValue(value); return value2; }).ToList();
                    }
                }

                Clear();

                foreach (var item in itemsList)
                {
                    Add(item);
                }
            }
            else
            {
                Environment.Exit(-1);
            }
        }

        public void Sort(string propertyName, ListSortDirection direction)
        {
            string[] paths = propertyName.Split('.');
            if (paths.Length == 1)
            {
                PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(typeof(T))[paths[0]];
                SortSubToo(propertyDescriptor, null, direction);
            }
            else
            {
                PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(typeof(T))[paths[0]];
                PropertyDescriptor propertyDescriptor1 = TypeDescriptor.GetProperties(propertyDescriptor.PropertyType)[paths[1]];
                SortSubToo(propertyDescriptor, propertyDescriptor1, direction);
            }
        }


        protected override int FindCore(PropertyDescriptor propertyDescriptor, object obj)
        {

            if (propertyDescriptor.PropertyType == typeof(string) || propertyDescriptor.PropertyType == typeof(int))
            {
                foreach (T item in Items)
                {
                    var value = propertyDescriptor.GetValue(item);
                    if (value.Equals(obj))
                    {
                        return IndexOf(item);
                    }
                }
                return -1;
            }
            else
            {
                Environment.Exit(-1);
                return -1;
            }
        }


        protected virtual List<int> FindCoreElements(PropertyDescriptor propertyDescriptor, PropertyDescriptor propertyDescriptor1, object obj)
        {
            List<int> indexes = new List<int>();

            if (propertyDescriptor.PropertyType == typeof(string) || propertyDescriptor.PropertyType == typeof(int) || propertyDescriptor.PropertyType == typeof(double) || propertyDescriptor.PropertyType == typeof(Engine))
            {
                foreach (T item in Items)
                {
                    var value = propertyDescriptor.GetValue(item);
                    if (propertyDescriptor1 == null)
                    {
                        if (value.Equals(obj))
                        {
                            indexes.Add(IndexOf(item));
                        }
                    }
                    else
                    {
                        var value2 = propertyDescriptor1.GetValue(value);
                        if (value2.Equals(obj))
                        {
                            indexes.Add(IndexOf(item));
                        }
                    }
                }
                return indexes;
            }
            else
            {
                Environment.Exit(-1);
                return indexes;
            }
        }

        public List<int> Find(string propertyName, object obj)
        { 
            string[] paths = propertyName.Split('.');
            if(paths.Length == 1)
            {
                PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(typeof(T))[paths[0]];
                return FindCoreElements(propertyDescriptor, null, obj);
            }
            else
            {
                PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(typeof(T))[paths[0]];
                PropertyDescriptor propertyDescriptor1 = TypeDescriptor.GetProperties(propertyDescriptor.PropertyType)[paths[1]];
                return FindCoreElements(propertyDescriptor, propertyDescriptor1, obj);
            }
            
        }


    }
}
