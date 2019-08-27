using HrPayroll.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HrPayroll
{
    public static class ImageRemove
    {
        public static void PhotoPathDelete(string photo,string path)
        {
            if(photo != null)
            {
                string Fullpath = Path.Combine(path, photo);
                if (File.Exists(Fullpath))
                {
                    File.Delete(Fullpath);
                }
            }
        }
    }
}
