using Face.Contract;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceStudioClient.Model
{
    class DepartmentUI
    {
        public Department Department
        {
            get;set;
        }

        public ObservableCollection<DepartmentUI> SubDepartments
        {
            get;set;
        }

        public void Add(DepartmentUI depart)
        {
            SubDepartments.Add(depart);
        }

        public void Remove(DepartmentUI depart)
        {
            SubDepartments.Remove(depart);
        }

        /// <summary>
        /// 遍历查找某个节点
        /// </summary>
        /// <returns></returns>
        public DepartmentUI Enumerate(Guid id)
        {
            if (this.Department.ID == id)
                return this;

            if (this.SubDepartments == null)
                return null;

            foreach(var item in SubDepartments)
            {
                var ret = item.Enumerate(id);
                if (ret != null)
                    return ret;
            }

            return null;
        }

        string _title = null;
        public override string ToString()
        {
            if(string.IsNullOrEmpty(_title))
            {
                Stack<string> stack = new Stack<string>();
                stack.Push(this.Department.Name);
                var parent = this.Department.ParentDepartment;
                while (parent != null)
                {
                    stack.Push("/");
                    stack.Push(parent.Name);
                    parent = parent.ParentDepartment;
                }
                StringBuilder sb = new StringBuilder();
                while(stack.Count > 0)
                {
                    sb.Append(stack.Pop());
                }

                _title = sb.ToString();
            }
            return _title;
        }

        public DepartmentUI()
        {
            this.SubDepartments = new ObservableCollection<DepartmentUI>();
        }
    }
}
