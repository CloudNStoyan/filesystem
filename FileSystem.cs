using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Drawing;

namespace FileSystem
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class FileSystem
    {
        private readonly RequestDelegate next;

        public FileSystem(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            string fileName = context.Request.Query["file"];
            var allFileNamesDic = new DirectoryInfo(".\\global").GetFiles().ToDictionary(x => x.Name, x => x.FullName);
            
            if (!string.IsNullOrEmpty(fileName))
            {
                if (allFileNamesDic.ContainsKey(fileName))
                {
                    await context.Response.SendFileAsync(allFileNamesDic[fileName]);
                }
                else
                {
                    await this.next.Invoke(context);
                }
            }
            
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class FileSystemExtensions
    {
        public static IApplicationBuilder UseFileSystem(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<FileSystem>();
        }
    }
}
