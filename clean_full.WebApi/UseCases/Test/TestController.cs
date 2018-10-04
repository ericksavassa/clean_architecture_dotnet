namespace clean_full.WebApi.UseCases.Test
{
    using clean_full.Application.UseCases.GetAccountDetails;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("api/[controller]")]
    public class TestController : Controller
    {
        private readonly IGetAccountDetailsUseCase _getAccountDetailsUseCase;
        private readonly Presenter _presenter;

        public TestController(
            IGetAccountDetailsUseCase getAccountDetailsUseCase,
            Presenter presenter)
        {
            _getAccountDetailsUseCase = getAccountDetailsUseCase;
            _presenter = presenter;
        }

        /// <summary>
        /// Get FILE FROM A FOLÇDER WITH 1 FILE
        /// </summary>
        [HttpGet("{fileId}", Name = "Test")]
        public IActionResult Get(string fileId)
        {
            string response = "file found";
            string filePath = @"C:\\oneFile\\orders.xml";

            if (System.IO.File.Exists(filePath) == false)
            {
                response = "file not found";
            }
            _presenter.Populate(response);
            return _presenter.ViewModel;
        }

        /// <summary>
        /// Get FILE FROM A FOLDER WITH 400 FILES
        /// </summary>
        [HttpGet("test2/{fileId}", Name = "Test2")]
        public IActionResult Get2(string fileId)
        {
            string response = "file found";
            string filePath = @"C:\\nFiles\\orders.xml";

            if (System.IO.File.Exists(filePath) == false)
            {
                response = "file not found";
            }
            _presenter.Populate(response);
            return _presenter.ViewModel;
        }

        /// <summary>
        /// Get STRING FROM XML FILE
        /// </summary>
        [HttpGet("test3/{fileId}", Name = "Test3")]
        public async Task<IActionResult> Get3(string fileId)
        {
            string response = "file found";
            string filePath = @"C:\\oneFile\\orders.xml";

            if (System.IO.File.Exists(filePath) == false)
            {
                response = "file not found";
            }

            object responseObject = await LoadTextAsync(filePath);

            if (responseObject == null)
                response = "File not loaded";
            else
                response = responseObject.ToString();

            _presenter.Populate(response);
            return _presenter.ViewModel;
        }

        /// <summary>
        /// Get Md5 HASH FROM FILE
        /// </summary>
        [HttpGet("test4/{fileId}", Name = "Test4")]
        public async Task<IActionResult> Get4(string fileId)
        {
            string response = "file found";
            string filePath = @"C:\\oneFile\\orders.xml";

            if (System.IO.File.Exists(filePath) == false)
            {
                response = "file not found";
            }

            response = GetMD5HashFromFile(filePath);

            //var xmlBytes = new UnicodeEncoding().GetBytes(xmlAsString);
            //var hashedXmlBytes = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(xmlBytes);
            //var hashedString = BitConverter.ToString(hashedXmlbytes);

            _presenter.Populate(response);
            return _presenter.ViewModel;
        }

        /// <summary>
        /// Get SHA 512 HASH FROM FILE
        /// </summary>
        [HttpGet("test5/{fileId}", Name = "Test5")]
        public async Task<IActionResult> Get5(string fileId)
        {
            string response = "file found";
            string filePath = @"C:\\oneFile\\orders.xml";

            if (System.IO.File.Exists(filePath) == false)
            {
                response = "file not found";
            }

            response = GenerateSHA512String(filePath);

            _presenter.Populate(response);
            return _presenter.ViewModel;
        }

        /// <summary>
        /// Compare a list of value based on a list of conditions
        /// </summary>
        [HttpGet("test6/{fileId}", Name = "Test6")]
        public async Task<IActionResult> Get6(string fileId)
        {
            var listTest = new List<Entity>() {
                new Entity("EXTENDED_FIELD1", "value1"),
                new Entity("EXTENDED_FIELD2", "0"),
                new Entity("EXTENDED_FIELD3", "50")
            };
            
            var conditionsList = new ListOfConditions()
            {
                conditions =
                    new List<Condition>() { new Condition("EXTENDED_FIELD1", "value1", "=="),
                    new Condition("EXTENDED_FIELD2","1", "=="),
                    new Condition("EXTENDED_FIELD3", "25", "==") },
                fullCondition = "(({0} OR {1}) AND ({2}))"
            };

            foreach (var item in conditionsList.conditions)
            {
                item.result = validateCondition(entities: listTest, condition: item);
            }

            string[] arrayOfResult = conditionsList.conditions.Select(c => c.result.ToString().ToLower()).ToArray();
            string newResult = string.Format(conditionsList.fullCondition, arrayOfResult);

            var ep = new ExprParser();

            string lambdaExpression = newResult.Replace("OR", "||").Replace("AND", "&&");

            LambdaExpression lambda = ep.Parse(lambdaExpression);
            bool result = (bool)ep.Run(lambda);

            _presenter.Populate(result.ToString());
            return _presenter.ViewModel;
        }

        /// <summary>
        /// Compare a list of value based on a list of conditions
        /// </summary>
        [HttpPost("test7/", Name = "Test7")]
        public async Task<IActionResult> Get7([FromBody] ConditionsRequest request)
        {
            List<Entity> listTest = request.entities;
            ListOfConditions conditionsList = request.ListOfConditions;

            foreach (var item in conditionsList.conditions)
            {
                item.result = validateCondition(entities: listTest, condition: item);
            }

            string[] arrayOfResult = conditionsList.conditions.Select(c => c.result.ToString().ToLower()).ToArray();
            string newResult = string.Format(conditionsList.fullCondition, arrayOfResult);

            var ep = new ExprParser();
            string lambdaExpression = newResult.Replace("OR", "||").Replace("AND", "&&");
            LambdaExpression lambda;// = ep.Parse(lambdaExpression);

            lambda = ep.Parse("(10 * 15 – 11) > 100 ? 'right result' : 'wrong result'"); // string constant is enclosed by either ‘ or \”
            int result = (int)ep.Run(lambda);

            //string result = (string)ep.Run(lambda);

            _presenter.Populate(result.ToString());
            return _presenter.ViewModel;
        }

        private bool validateCondition(List<Entity> entities, Condition condition)
        {
            Entity entity = entities.Find((e) => e.key == condition.key);
            
            if(entity == null)
            {
                return false;
            }

            switch (condition.valueOperator)
            {
                case ">":
                    return (Convert.ToDecimal(entity.value) > Convert.ToDecimal(condition.value));
                case "<":
                    return (Convert.ToDecimal(entity.value) < Convert.ToDecimal(condition.value));
                case ">=":
                    return (Convert.ToDecimal(entity.value) >= Convert.ToDecimal(condition.value));
                case "<=":
                    return (Convert.ToDecimal(entity.value) <= Convert.ToDecimal(condition.value));
                case "==":
                    return (entity.value.Equals(condition.value));
                default:
                    return false;
            }
        }

        protected string GetMD5HashFromFile(string fileName)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = (System.IO.File.OpenRead(fileName)))
                {
                    return BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", string.Empty);
                }
            }
        }

        public static string GenerateSHA512String(string fileName)
        {
            using (var sha512 = SHA512.Create())
            {
                using (var stream = (System.IO.File.OpenRead(fileName)))
                {
                    return BitConverter.ToString(sha512.ComputeHash(stream)).Replace("-", string.Empty);
                }
            }
        }

        public async Task<object> LoadTextAsync(string filePath)
        {
            object records = null;
            await Task.Run(() =>
            {
                XElement xElement = XElement.Load(filePath);
                records = xElement;
            });
            return records;
        }

    }

    public sealed class Presenter
    {
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

    public class Entity
    {
        public Entity(string key, string value)
        {
            this.key = key;
            this.value = value;
        }

        public string key { get; set; }
        public string value { get; set; }
    }

    public class Condition
    {
        public Condition(string key, string value, string valueOperator)
        {
            this.key = key;
            this.value = value;
            this.valueOperator = valueOperator;
        }

        public string key { get; set; }
        public string value { get; set; }
        public string valueOperator { get; set; }
        public bool result { get; set; }

    }

    public class ListOfConditions
    {
        public List<Condition> conditions { get; set; }
        public string fullCondition { get; set; }

    }

    public class ConditionsRequest
    {
        public List<Entity> entities { get; set; }
        public ListOfConditions ListOfConditions { get; set; }
    }

}
