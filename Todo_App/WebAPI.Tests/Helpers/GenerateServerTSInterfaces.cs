using System;
using Xunit;
using System.Linq;
using WebAPI.Controllers;
using TypeScriptBuilder;
using System.IO;
using System.Reflection;

namespace WebAPI.Tests
{
    public class GenerateServerTSInterfaces
    {
        [Fact(Skip="Nur manuell")]
        public void Generate()
        {
            var ts = new TypeScriptGenerator();
            ts.AddCSType(typeof(Models.Requests.AlleTodos));

            // Assembly.GetAssembly(typeof(Program))
            //     .GetTypes()
            //     .Where(t => t.IsClass && t.Namespace == "WebAPI.Models")
            //     .ToList()
            //     .ForEach(t => ts.AddCSType(t));

            // ts.ExcludeType(typeof(ApplicationUser));

            ts.Store("../../../Helpers/Server.ts");
        }
    }
}
