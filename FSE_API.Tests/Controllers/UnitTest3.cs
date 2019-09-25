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
    public class UnitTest3
    {
        List<ParentTask> _randomParentTask = null;
        //List<ParentTaskModel> _randomParentTaskModel = null;
        IParentTaskRepository ParentTaskRepository = null;

        public UnitTest3()
        {
            _randomParentTask = SetupParentTask();
            // _randomProjectModel = SetupProjectModel();
            ParentTaskRepository = SetupTaskRepository();
        }

        public List<ParentTask> SetupParentTask()
        {
            var ParentTask = new List<ParentTask>();
            ParentTask.Add(new ParentTask
            {
                Parent_ID = 1,
                Parent_Task = "Test"
               
            });
            //tasks.Add(new Task { Task_ID = 2 });
            //tasks.Add(new Task { Task_ID = 3 });
            //tasks.Add(new Task { Task_ID = 4 });

            return ParentTask;

        }

        //public List<ProjectModel> SetupProjectModel()
        //{
        //    var ParentTask = new List<ProjectModel>();
        //    ParentTask.Add(new ProjectModel()
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

        //    return ParentTask;



        //}

        public IParentTaskRepository SetupTaskRepository()
        {
            // Init repository
            var repo = new Mock<IParentTaskRepository>();

            //var mockRepository = new Mock<ITasksRepository>();
            //repo.Setup(x => x.GetTask(1))
            //    .Returns(new Task { Task_ID = 1 });
            // Setup mocking behavior
            repo.Setup(r => r.Get()).Returns(_randomParentTask);

            repo.Setup(r => r.GetParentTask(It.IsAny<int>()))
                .Returns(new Func<int, ParentTask>(
                    id => _randomParentTask.Find(a => a.Parent_ID.Equals(id))));

            repo.Setup(r => r.Post(It.IsAny<ParentTask>()))
                .Callback(new Action<ParentTask>(newArticle =>
                {
                    dynamic maxArticleID = _randomParentTask.Last().Parent_ID;
                    dynamic nextArticleID = maxArticleID + 1;
                    newArticle.Parent_ID = nextArticleID;
                    //newArticle.Start_Date = DateTime.Now;
                    _randomParentTask.Add(newArticle);
                }));

            repo.Setup(r => r.Put(1, It.IsAny<ParentTask>()))
                .Callback<int, ParentTask>((o, x) =>
                {
                    var oldArticle = _randomParentTask.Find(a => a.Parent_ID == 1);
                    _randomParentTask.Find(a => a.Parent_ID == 1).Parent_Task = x.Parent_Task;
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
                    var _articleToRemove = _randomParentTask.Find(a => a.Parent_ID == x);

                    if (_articleToRemove != null)
                        _randomParentTask.Remove(_articleToRemove);
                }));

            // Return mock implementation
            return repo.Object;
        }

        [TestMethod()]
        public void GetParentTaskTest()
        {

            //var mockRepository = new Mock<ITasksRepository>();
            //mockRepository.Setup(x => x.GetTask(1))
            //    .Returns(new Task { Task_ID = 1 });

            var controller = new ParentTaskController(ParentTaskRepository);
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
        public void GetParentTaskByIDTest()
        {
            //var mockRepository = new Mock<ITasksRepository>();
            //mockRepository.Setup(x => x.GetTask(1))
            //    .Returns(new Task { Task_ID = 1 });

            var controller = new ParentTaskController(ParentTaskRepository);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
            var response = controller.GetParentTask(1);

            Assert.IsNotNull(response.Content);
            Assert.IsNotNull(response.Content.Headers.ContentType);
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.OK);



            var responseString = response.Content.ReadAsStringAsync().Result;
            // Assert
            dynamic jsonObject = JObject.Parse(responseString);
            int status = (int)jsonObject.Parent_ID;
            Assert.AreEqual(1, status);
            //Assert.IsTrue(response.TryGetContentValue<Task>(out task));
            //Assert.AreEqual(10, task.Task_ID);

        }

        [TestMethod()]
        public void PostParentTaskTest()
        {
            //var mockRepository = new Mock<ITasksRepository>();
            //mockRepository.Setup(x => x.GetTask(1))
            //    .Returns(new Task { Task_ID = 1 });

            int count = _randomParentTask.Count;

            var controller = new ParentTaskController(ParentTaskRepository);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();
            ParentTask task = new ParentTask() { Parent_ID = 2 };
            // Act
            var response = controller.Post(task);

            Assert.IsNotNull(response.Content);
            Assert.IsNotNull(response.Content.Headers.ContentType);
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.OK);



            var responseString = response.Content.ReadAsStringAsync().Result;
            // Assert
            //dynamic jsonObject = JObject.Parse(responseString);
            //int status = (int)jsonObject.Task_ID;
            Assert.AreEqual(count + 1, _randomParentTask.Count);
            //Assert.IsTrue(response.TryGetContentValue<Task>(out task));
            //Assert.AreEqual(10, task.Task_ID);
        }

        [TestMethod()]
        public void PutParentTaskTest()
        {
            var controller = new ParentTaskController(ParentTaskRepository);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();
            ParentTask task = new ParentTask() { Parent_ID = 1, Parent_Task = "Test1" };
            // Act
            var response = controller.Put(1, task);

            Assert.IsNotNull(response.Content);
            Assert.IsNotNull(response.Content.Headers.ContentType);
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.OK);
            Assert.AreEqual(_randomParentTask.Find(a => a.Parent_ID == 1).Parent_Task, "Test1");


            var responseString = response.Content.ReadAsStringAsync().Result;
            // Assert
            //dynamic jsonObject = JObject.Parse(responseString);
            //int status = (int)jsonObject.Task_ID;
            //Assert.AreEqual(0, responseString);
        }

        [TestMethod()]
        public void DeleteParentTaskTest()
        {
            var controller = new ParentTaskController(ParentTaskRepository);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();
            ParentTask task = new ParentTask() { Parent_ID = 1, Parent_Task = "Test" };
            // Act
            var response = controller.Delete(1);

            Assert.IsNotNull(response.Content);
            Assert.IsNotNull(response.Content.Headers.ContentType);
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.OK);
            Assert.AreEqual(_randomParentTask.Count, 0);


        }
    }
}
