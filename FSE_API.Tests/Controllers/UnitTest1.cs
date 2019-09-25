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

namespace FSE_API.Controllers.Tests
{
    [TestClass()]
    public class UnitTest1
    {

        List<Task> _randomTasks = null;
        List<TaskModel> _randomTaskModel = null;
        ITasksRepository taskRepository = null;

        public UnitTest1()
        {
            _randomTasks = SetupTasks();
            _randomTaskModel = SetupTaskModel();
            taskRepository = SetupTaskRepository();
        }

        public List<Task> SetupTasks()
        {
            var tasks = new List<Task>();
            tasks.Add(new Task
            {
                Project_ID = 1,
                Task_Name = "Test",
                Start_Date = DateTime.Now,
                End_Date = DateTime.Now,
                Priority = 1,
                //ParentTask_Name = "Test",
                Parent_ID = 1,
                Task_ID = 1,
                Status = "Test"
            });
            //tasks.Add(new Task { Task_ID = 2 });
            //tasks.Add(new Task { Task_ID = 3 });
            //tasks.Add(new Task { Task_ID = 4 });

            return tasks;


            
        }

        public List<TaskModel> SetupTaskModel()
        {
            var tasks = new List<TaskModel>();
            tasks.Add(new TaskModel()
            {
                Project_ID = 1,
                Task_Name = "Test",
                Start_Date = DateTime.Now,
                End_Date =DateTime.Now,
                Priority = 1,
                ParentTask_Name = "Test",
                Parent_ID = 1,
                Task_ID = 1,
                Status = "Test"
            });
            //tasks.Add(new Task { Task_ID = 2 });
            //tasks.Add(new Task { Task_ID = 3 });
            //tasks.Add(new Task { Task_ID = 4 });

            return tasks;



        }

        public ITasksRepository SetupTaskRepository()
        {
            // Init repository
            var repo = new Mock<ITasksRepository>();

            //var mockRepository = new Mock<ITasksRepository>();
            //repo.Setup(x => x.GetTask(1))
            //    .Returns(new Task { Task_ID = 1 });
            // Setup mocking behavior
            repo.Setup(r => r.Get()).Returns(_randomTasks);

            repo.Setup(r => r.GetTask(It.IsAny<int>()))
                .Returns(new Func<int, Task>(
                    id => _randomTasks.Find(a => a.Task_ID.Equals(id))));

            repo.Setup(r => r.Post(It.IsAny<Task>()))
                .Callback(new Action<Task>(newArticle =>
                {
                    dynamic maxArticleID = _randomTasks.Last().Task_ID;
                    dynamic nextArticleID = maxArticleID + 1;
                    newArticle.Task_ID = nextArticleID;
                    //newArticle.Start_Date = DateTime.Now;
                    _randomTasks.Add(newArticle);
                }));

            repo.Setup(r => r.Put(1, It.IsAny<Task>()))
                .Callback<int, Task>((o, x) =>
                {
                    var oldArticle = _randomTasks.Find(a => a.Task_ID == 1);
                    _randomTasks.Find(a => a.Task_ID == 1).Status = x.Status;
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
                    var _articleToRemove = _randomTasks.Find(a => a.Task_ID == x);

                    if (_articleToRemove != null)
                        _randomTasks.Remove(_articleToRemove);
                }));

            // Return mock implementation
            return repo.Object;
        }

        [TestMethod()]
        public void GetTest()
        {

            //var mockRepository = new Mock<ITasksRepository>();
            //mockRepository.Setup(x => x.GetTask(1))
            //    .Returns(new Task { Task_ID = 1 });

            var controller = new TasksController(taskRepository);
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
        public void GetTaskTest()
        {
            //var mockRepository = new Mock<ITasksRepository>();
            //mockRepository.Setup(x => x.GetTask(1))
            //    .Returns(new Task { Task_ID = 1 });

            var controller = new TasksController(taskRepository);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
            var response = controller.GetTask(1);

            Assert.IsNotNull(response.Content);
            Assert.IsNotNull(response.Content.Headers.ContentType);
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.OK);



            var responseString = response.Content.ReadAsStringAsync().Result;
            // Assert
            dynamic jsonObject = JObject.Parse(responseString);
            int status = (int)jsonObject.Task_ID;
            Assert.AreEqual(1, status);
            //Assert.IsTrue(response.TryGetContentValue<Task>(out task));
            //Assert.AreEqual(10, task.Task_ID);

        }

        [TestMethod()]
        public void PostTaskTest()
        {
            //var mockRepository = new Mock<ITasksRepository>();
            //mockRepository.Setup(x => x.GetTask(1))
            //    .Returns(new Task { Task_ID = 1 });

            int count =_randomTasks.Count;

            var controller = new TasksController(taskRepository);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();
            Task task = new Task() { Task_ID = 2 };
            // Act
            var response = controller.Post(task);

            Assert.IsNotNull(response.Content);
            Assert.IsNotNull(response.Content.Headers.ContentType);
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.OK);



            var responseString = response.Content.ReadAsStringAsync().Result;
            // Assert
            //dynamic jsonObject = JObject.Parse(responseString);
            //int status = (int)jsonObject.Task_ID;
            Assert.AreEqual(count + 1, _randomTasks.Count);
            //Assert.IsTrue(response.TryGetContentValue<Task>(out task));
            //Assert.AreEqual(10, task.Task_ID);
        }

        [TestMethod()]
        public void PutTest()
        {
            var controller = new TasksController(taskRepository);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();
            Task task = new Task() { Task_ID = 1,Status = "Test" };
            // Act
            var response = controller.Put(1,task);

            Assert.IsNotNull(response.Content);
            Assert.IsNotNull(response.Content.Headers.ContentType);
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.OK);
            Assert.AreEqual(_randomTasks.Find(a => a.Task_ID == 1).Status, "Test");


            var responseString = response.Content.ReadAsStringAsync().Result;
            // Assert
            //dynamic jsonObject = JObject.Parse(responseString);
            //int status = (int)jsonObject.Task_ID;
            //Assert.AreEqual(0, responseString);
        }

        [TestMethod()]
        public void DeleteTest()
        {
            var controller = new TasksController(taskRepository);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();
            Task task = new Task() { Task_ID = 1, Status = "Test" };
            // Act
            var response = controller.Delete(1);

            Assert.IsNotNull(response.Content);
            Assert.IsNotNull(response.Content.Headers.ContentType);
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.OK);
            Assert.AreEqual(_randomTasks.Count, 0);


        }
    }
}

