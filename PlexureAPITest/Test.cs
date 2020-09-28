using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace PlexureAPITest
{
    [TestFixture]
    public class Test
    {
        Service service;

        [SetUp]
        public void Setup()
        {
            service = new Service();
        }

        [TearDown]
        public void Cleanup()
        {
            if (service != null)
            {
                service.Dispose();
                service = null;
            }
        }

        [TestCase("Testar", "Plexure123", HttpStatusCode.Unauthorized)]
        [TestCase("Tester", "Plexure", HttpStatusCode.Unauthorized)]
        [TestCase(null, "Plexure", HttpStatusCode.Unauthorized)]
        [TestCase("Tester", null, HttpStatusCode.Unauthorized)]
        public void TEST_001_Login_With_Invalid_User(string username, string password, HttpStatusCode httpStatusCode)
        {
            var response = service.Login(username, password);

            response.Expect(httpStatusCode);
           
        }

        [TestCase("Tester", "Plexure123")]
        public void TEST_002_Login_With_Valid_User(string username, string password)
        {
            var response = service.Login(username, password);

            response.Expect(HttpStatusCode.OK);

        }

        [Test]
        public void TEST_003_Get_Points_For_Logged_In_User()
        {
            var points = service.GetPoints();
            int pointsBalance = points.Entity.Value;
            Assert.IsNotNull(pointsBalance);
        }

        [TestCase(2, HttpStatusCode.BadRequest)]
        [TestCase(1, HttpStatusCode.Accepted)]
        public void TEST_004_Purchase_Product(int productId, HttpStatusCode httpStatusCode)
        {
            var response = service.Purchase(productId);
            response.Expect(httpStatusCode);
        }

        [Test]
        public void TEST_005_Earn_100_Points_On_Product_Purchase()
        {
            var points = service.GetPoints();
            int initialPointsBalance = points.Entity.Value;
            var response = service.Purchase(1);
            points = service.GetPoints();
            int finalPointsBalance = points.Entity.Value;
            Assert.AreEqual(100, finalPointsBalance - initialPointsBalance);
        }
    }
}
