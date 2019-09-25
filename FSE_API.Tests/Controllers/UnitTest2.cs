using FSE_API.Controllers;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FSE_API.DBContext;
using FSE_API.Repository;
using System.Collections.Generic;
using Moq;
using System.Linq;
using System.Web.Http.Results;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using FSE_API.Models;

namespace FSE_API.Tests.Controllers
{
    [TestClass]
    public class UnitTest2
    {
        List<User> _randomUsers = null;
        //List<UserModel> _randomUserModel = null;
        IUsersRepository usersRepository = null;

        public UnitTest2()
        {
            _randomUsers = SetupUsers();
            // _randomProjectModel = SetupProjectModel();
            usersRepository = SetupTaskRepository();
        }

        public List<User> SetupUsers()
        {
            var Users = new List<User>();
            Users.Add(new User
            {
                User_ID = 1,
                First_Name = "Test",
                Last_Name = "Test",
                Employee_ID = 1
            });
            //tasks.Add(new Task { Task_ID = 2 });
            //tasks.Add(new Task { Task_ID = 3 });
            //tasks.Add(new Task { Task_ID = 4 });

            return Users;

        }

        //public List<ProjectModel> SetupProjectModel()
        //{
        //    var Users = new List<ProjectModel>();
        //    Users.Add(new ProjectModel()
        //    {
        //        Project_ID = 1,
        //        Project_Name = "Test",
        //        Start_Date = DateTime.Now,
        //        End_Date = DateTime.Now,
        //        Priority = 1,
        //        NoOfTasks = 1,
        //        CompletedTask = 1

        //    });
        //    //tasks.Add(new Task { Task_ID = 2 });
        //    //tasks.Add(new Task { Task_ID = 3 });
        //    //tasks.Add(new Task { Task_ID = 4 });

        //    return Users;



        //}

        public IUsersRepository SetupTaskRepository()
        {
            // Init repository
            var repo = new Mock<IUsersRepository>();

            //var mockRepository = new Mock<ITasksRepository>();
            //repo.Setup(x => x.GetTask(1))
            //    .Returns(new Task { Task_ID = 1 });
            // Setup mocking behavior
            repo.Setup(r => r.Get()).Returns(_randomUsers);

            repo.Setup(r => r.GetUser(It.IsAny<int>()))
                .Returns(new Func<int, User>(
                    id => _randomUsers.Find(a => a.User_ID.Equals(id))));

            repo.Setup(r => r.Post(It.IsAny<User>()))
                .Callback(new Action<User>(newArticle =>
                {
                    dynamic maxArticleID = _randomUsers.Last().User_ID  ;
                    dynamic nextArticleID = maxArticleID + 1;
                    newArticle.User_ID = nextArticleID;
                    //newArticle.Start_Date = DateTime.Now;
                    _randomUsers.Add(newArticle);
                }));

            repo.Setup(r => r.Put(1, It.IsAny<User>()))
                .Callback<int, User>((o, x) =>
                {
                    var oldArticle = _randomUsers.Find(a => a.User_ID == 1);
                    _randomUsers.Find(a => a.User_ID == 1).Employee_ID = x.Employee_ID;
                    oldArticle = x;
                });


            //repo.Setup(r => r.Put(1, It.IsAny<Task>()))
            //    .Callback<int, Task>(new Action<int,Task>((1,x) =>
            //    {
            //        var oldArticle = _randomArticles.Find(a => a.Task_ID == 1);
            //        //oldArticle.Start_Date = DateTime.Now;
            //        oldArticle = x;
            //    }));

            repo.Setup(r => r.Delete(1))
                .Callback(new Action<int>(x =>
                {
                    var _articleToRemove = _randomUsers.Find(a => a.User_ID == x);

                    if (_articleToRemove != null)
                        _randomUsers.Remove(_articleToRemove);
                }));

            // Return mock implementation
            return repo.Object;
        }

        [TestMethod()]
        public void GetUserTest()
        {

            //var mockRepository = new Mock<ITasksRepository>();
            //mockRepository.Setup(x => x.GetTask(1))
            //    .Returns(new Task { Task_ID = 1 });

            var controller = new UsersController(usersRepository);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
            var response = controller.Get();

            Assert.IsNotNull(response.Content);
            Assert.IsNotNull(response.Content.Headers.ContentType);
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.OK);



            //var responseString = response.Content.ReadAsStringAsync().Result;
            //// Assert
            //dynamic jsonObject = JObject.Parse(responseString);
            //int status = (int)jsonObject.Task_ID;
            //Assert.AreEqual(1, status);
            //Assert.IsTrue(response.TryGetContentValue<Task>(out task));
            //Assert.AreEqual(10, task.Task_ID);

        }

        [TestMethod()]
        public void GetUserByIDTest()
        {
            //var mockRepository = new Mock<ITasksRepository>();
            //mockRepository.Setup(x => x.GetTask(1))
            //    .Returns(new Task { Task_ID = 1 });

            var controller = new UsersController(usersRepository);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
            var response = controller.GetUser(1);

            Assert.IsNotNull(response.Content);
            Assert.IsNotNull(response.Content.Headers.ContentType);
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.OK);



            var responseString = response.Content.ReadAsStringAsync().Result;
            // Assert
            dynamic jsonObject = JObject.Parse(responseString);
            int status = (int)jsonObject.User_ID;
            Assert.AreEqual(1, status);
            //Assert.IsTrue(response.TryGetContentValue<Task>(out task));
            //Assert.AreEqual(10, task.Task_ID);

        }

        [TestMethod()]
        public void PostUserTest()
        {
            //var mockRepository = new Mock<ITasksRepository>();
            //mockRepository.Setup(x => x.GetTask(1))
            //    .Returns(new Task { Task_ID = 1 });

            int count = _randomUsers.Count;

            var controller = new UsersController(usersRepository);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();
            User task = new User() { User_ID = 2 };
            // Act
            var response = controller.Post(task);

            Assert.IsNotNull(response.Content);
            Assert.IsNotNull(response.Content.Headers.ContentType);
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.OK);



            var responseString = response.Content.ReadAsStringAsync().Result;
            // Assert
            //dynamic jsonObject = JObject.Parse(responseString);
            //int status = (int)jsonObject.Task_ID;
            Assert.AreEqual(count + 1, _randomUsers.Count);
            //Assert.IsTrue(response.TryGetContentValue<Task>(out task));
            //Assert.AreEqual(10, task.Task_ID);
        }

        [TestMethod()]
        public void PutUserTest()
        {
            var controller = new UsersController(usersRepository);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();
            User task = new User() { User_ID = 1, Employee_ID = 1 };
            // Act
            var response = controller.Put(1, task);

            Assert.IsNotNull(response.Content);
            Assert.IsNotNull(response.Content.Headers.ContentType);
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.OK);
            Assert.AreEqual(_randomUsers.Find(a => a.User_ID == 1).Employee_ID, 1);


            var responseString = response.Content.ReadAsStringAsync().Result;
            // Assert
            //dynamic jsonObject = JObject.Parse(responseString);
            //int status = (int)jsonObject.Task_ID;
            //Assert.AreEqual(0, responseString);
        }

        [TestMethod()]
        public void DeleteUserTest()
        {
            var controller = new UsersController(usersRepository);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();
            User task = new User() { User_ID = 1, Employee_ID = 1 };
            // Act
            var response = controller.Delete(1);

            Assert.IsNotNull(response.Content);
            Assert.IsNotNull(response.Content.Headers.ContentType);
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.OK);
            Assert.AreEqual(_randomUsers.Count, 0);


        }
    }
}
