using Face.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceStudioClient.Service
{
    class EmployeeService : BaseService
    {
        public event Action<Employee[]> OnQueryCompleted;
        public event Action<Employee> OnSaveCompleted;

        public void Query(Action<Exception> failFunc)
        {
            var url = "http://localhost:8888/api/Employee/Query";
            web.AsyncJsonPost<string>(url, null,
                (response, cookies, exp) =>
                {
                    if (null != response)
                    {
                        var ret = JsonSerializer<Employee[]>.Deserialize(response);
                        if (ret != null)
                        {
                            if (null != OnQueryCompleted)
                            {
                                OnQueryCompleted(ret);
                            }
                        }
                        else
                        {
                            failFunc(new Exception("Failed in Query Object!"));
                        }
                    }
                    else if (null != exp)
                    {
                        failFunc(exp);
                    }
                });
        }

        public void Save(Employee entity, Action<Exception> failFunc)
        {
            var url = "http://localhost:8888/api/Employee/Save";
            web.AsyncJsonPost(url, entity,
                (response, cookies, exp) =>
                {
                    if (null != response)
                    {
                        var ret = JsonSerializer<Employee>.Deserialize(response);
                        if (ret != null)
                        {
                            if (null != OnSaveCompleted)
                            {
                                OnSaveCompleted(ret);
                            }
                        }
                        else
                        {
                            failFunc(new Exception("Failed in Save Object!"));
                        }
                    }
                    else if (null != exp)
                    {
                        failFunc(exp);
                    }
                });
        }
    }
}
