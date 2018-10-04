namespace clean_full.WebApi.UseCases.Test
{
    using clean_full.Application.UseCases;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Xml.Linq;

    public sealed class Presenter2 { 
        public IActionResult ViewModel { get; private set; }

        public void Populate(string output)
        {
            if (output == null)
            {
                ViewModel = new NoContentResult();
                return;
            }

            ViewModel = new ObjectResult(output);
        }
    }
}
