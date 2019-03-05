using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Demo
{
    public static class AzureFunctionDockerDemo
    {
        [FunctionName("AzureFunctionDockerDemo")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var name = req.Query["name"];
            var a = req.Query["a"];
            var b = req.Query["b"];
            var c = req.Query["c"];

            // Validating the parameters received
            if (string.IsNullOrWhiteSpace(name) ||
                string.IsNullOrWhiteSpace(a) ||
                string.IsNullOrWhiteSpace(b) ||
                string.IsNullOrWhiteSpace(c)
                )
                return new BadRequestObjectResult("Please provide a name and the 3 numbers (a,b,c) to resolve quadratic formula.");

            //Parsing numbers
            double aParsed, bParsed, cParsed = 0;
            try
            {
                aParsed = double.Parse(a);
                bParsed = double.Parse(b);
                cParsed = double.Parse(c);
            }
            catch (FormatException)
            {
                return new BadRequestObjectResult("The numbers (a,b,c) must be a double!");
            }

            return (ActionResult)new OkObjectResult($"Hello { name }, { GetQuadraticRoots(aParsed, bParsed, cParsed) }");
        }

        private static string GetQuadraticRoots(double a, double b, double c)
        {
            double deltaRoot = Math.Sqrt(b*b-4*a*c);

            if (a == 0)
               return "If A = 0, the equation is not quadratic";

            if (deltaRoot > 0)
            {
                double ans1 = (-1 * b + Math.Sqrt(Math.Pow(b, 2) - 4 * a * c)) / (2 * a);
                double ans2 = (-1 * b - Math.Sqrt(Math.Pow(b, 2) - 4 * a * c)) / (2 * a);

                return $"The solutions are: Answer 1 = { ans1 } and Answer 2 = { ans2 }";
            }
            else
                return "There is no anwser for given numbers";            
       }
    }
}
