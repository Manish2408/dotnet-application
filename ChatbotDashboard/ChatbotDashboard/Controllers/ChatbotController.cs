using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Web.Administration;
using ChatbotDashboard.Models;
using System.Linq;
using System.Web.SessionState;
using System.Diagnostics;
using System.DirectoryServices;
using System.Text;

namespace ChatbotDashboard.Controllers
{

    public class ChatbotController : Controller
    {
        
        // GET: Chatbot
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult getChatbotList()
        {
            ServerManager server = new ServerManager();
            List<Chatbot> lstChatbot = new List<Chatbot>();
            ChatbotStatus objstatus = new ChatbotStatus();

            Chatbots objchat = new Chatbots();

            DirectoryEntries appPools =
                new DirectoryEntry("IIS://localhost/W3SVC/AppPools").Children;

            foreach (DirectoryEntry appPool in appPools)
            {
                Chatbot objchat1 = new Chatbot();
                objchat1.Name = appPool.Name.ToString();
                //objchat1.Status=appPool.
                lstChatbot.Add(objchat1);
            }



            //ApplicationPoolCollection applicationPools = server.ApplicationPools;
            //foreach (ApplicationPool pool in applicationPools)
            //{
            //    Chatbot objchat1 = new Chatbot();
            //    //get the AutoStart boolean value
            //    string status = pool.AutoStart.ToString();

            //    if (status.ToLower() == "true")
            //    {
            //        objchat1.Status = "Running";

            //    }
            //    else
            //    {
            //        objchat1.Status = "Stopped";
            //    }



            //    //get the name of the ManagedRuntimeVersion
            //    string runtime = pool.ManagedRuntimeVersion;

            //    //get the name of the ApplicationPool
            //    objchat1.Name = pool.Name.ToString();

            //    //get the identity type
            //    ProcessModelIdentityType identityType = pool.ProcessModel.IdentityType;

            //    //get the username for the identity under which the pool runs
            //    string userName = pool.ProcessModel.UserName;

            //    //get the password for the identity under which the pool runs
            //    string password = pool.ProcessModel.Password;
            //    lstChatbot.Add(objchat1);

            //}
            //objchat.ChatbotsStatus = (from i in lstChatbot
                                      //select new SelectListItem { Text = i.Status, Value = i.Status }).Distinct().ToList();

            //objchat.chatbots = lstChatbot;
            //ViewData["lstitems"] = objchat.ChatbotsStatus;
            return View(lstChatbot);
        }

        [HttpGet]
        public ActionResult ChatbotList1()
        {
            ServerManager server = new ServerManager();
            List<Chatbot> lstChatbot = new List<Chatbot>();
            ChatbotStatus objstatus = new ChatbotStatus();

            Chatbots objchat = new Chatbots();
            ApplicationPoolCollection applicationPools = server.ApplicationPools;

            
            foreach (ApplicationPool pool in applicationPools)
            {
                Chatbot objchat1 = new Chatbot();
                //get the AutoStart boolean value
                string status = pool.AutoStart.ToString();

                if (status.ToLower() == "true")
                {
                    objchat1.Status = "Running";

                }
                else
                {
                    objchat1.Status = "Stopped";
                }



                //get the name of the ManagedRuntimeVersion
                string runtime = pool.ManagedRuntimeVersion;

                //get the name of the ApplicationPool
                objchat1.Name = pool.Name.ToString();

                //get the identity type
                ProcessModelIdentityType identityType = pool.ProcessModel.IdentityType;

                //get the username for the identity under which the pool runs
                string userName = pool.ProcessModel.UserName;

                //get the password for the identity under which the pool runs
                string password = pool.ProcessModel.Password;
                lstChatbot.Add(objchat1);

            }
            List<SelectListItem> items = new List<SelectListItem> { new SelectListItem { Text = "Running", Value = "Running" }, new SelectListItem { Text = "Stopped", Value = "Stopped" } };
            objchat.ChatbotsStatus = items;
            objchat.chatbots = lstChatbot;
            TempData["CHATBOTS"] = objchat;
            ViewData["lstitems"] = objchat.ChatbotsStatus;
            return View(objchat);
        }


        public ActionResult bindDropdown()
        {
            ChatbotStatus obj = new ChatbotStatus();
            List<SelectListItem> items = new List<SelectListItem> { new SelectListItem { Text = "Running", Value = "Running" }, new SelectListItem { Text = "Stopped", Value = "Stopped" } };
            obj.lstStatus = items;
            return View(obj);
        }

       
        [HttpPost]
        public ActionResult ChatbotList1(FormCollection form)
        {
            Chatbot obj = new Chatbot();
            string a =  form["Status"].ToString();
            string str_value = form["Search"].ToString();


            TempData["ddlstatus"] = a;
            List<Chatbot> lstChatbot = new List<Chatbot>();
            List<Chatbot> lstChatbotfinl = new List<Chatbot>();
            Chatbots objchat = new Chatbots();
            try
            {

                objchat = GetChatbots();
                TempData.Keep();
                lstChatbot = objchat.chatbots;
                //objchat.ChatbotsStatus= new List<SelectListItem> { new SelectListItem { Text = "Running", Value = "Running" }, new SelectListItem { Text = "Stopped", Value = "Stopped" } };
                //var abc = lstChatbot.Select(c => c.Status = a).Cast<Chatbot>().ToList();
                
                //objchat.chatbots = lstChatbot.Select(c => c.Status = a);

                if(a!="")
                {
                    var enu = lstChatbot.Where<Chatbot>(c => c.Status == a);
                    foreach (var i in enu)
                    {

                        lstChatbotfinl.Add(i);
                    }
                    objchat.chatbots = lstChatbotfinl;
                }
                else
                {
                    lstChatbotfinl = lstChatbot;

                }
                
                if(str_value!="")
                {
                    List<Chatbot> lstChatbotfilter = new List<Chatbot>();
                    var enu = lstChatbotfinl.Where<Chatbot>(c => c.Name.ToLower().Contains(str_value));
                    foreach (var i in enu)
                    {

                        lstChatbotfilter.Add(i);
                    }
                    objchat.chatbots = lstChatbotfilter;
                }
                

            }
            catch (Exception e)
            {

            }

            return View(objchat);
            //return PartialView("_chatbotList", objchat);
        }


        public Chatbots GetChatbots()
        {
            ServerManager server = new ServerManager();
            List<Chatbot> lstChatbot = new List<Chatbot>();
            ChatbotStatus objstatus = new ChatbotStatus();

            Chatbots objchat = new Chatbots();
            ApplicationPoolCollection applicationPools = server.ApplicationPools;
            foreach (ApplicationPool pool in applicationPools)
            {
                Chatbot objchat1 = new Chatbot();
                //get the AutoStart boolean value
                string status = pool.AutoStart.ToString();

                if (status.ToLower() == "true")
                {
                    objchat1.Status = "Running";

                }
                else
                {
                    objchat1.Status = "Stopped";
                }



                //get the name of the ManagedRuntimeVersion
                string runtime = pool.ManagedRuntimeVersion;

                //get the name of the ApplicationPool
                objchat1.Name = pool.Name.ToString();

                //get the identity type
                ProcessModelIdentityType identityType = pool.ProcessModel.IdentityType;

                //get the username for the identity under which the pool runs
                string userName = pool.ProcessModel.UserName;

                //get the password for the identity under which the pool runs
                string password = pool.ProcessModel.Password;
                lstChatbot.Add(objchat1);

            }
            List<SelectListItem> items = new List<SelectListItem> { new SelectListItem { Text = "Running", Value = "Running" }, new SelectListItem { Text = "Stopped", Value = "Stopped" } };
            objchat.ChatbotsStatus = items;
            objchat.chatbots = lstChatbot;
            //TempData["CHATBOTS"] = objchat;
            //ViewData["lstitems"] = objchat.ChatbotsStatus;
            return objchat;
        }



        [HttpGet]
        public string ChangeStatus(string id)
        {

            string id1 = id;

            var serverManager = new ServerManager();

            //string command = "cd %windir%\\system32\\inetsrv";
            //string command1 = "appcmd start apppool /apppool.name:" + id;
            //ExecuteCommand(command, 180000);
            //ExecuteCommand(command1, 180000);
            var appPool = serverManager.ApplicationPools.FirstOrDefault(ap => ap.Name.Equals(id));
            appPool.Start();
            return "";
        }


        public static int ExecuteCommand(string command, int timeout)
        {
            var exitCode = 0;
            try
            { 
                var processInfo = new ProcessStartInfo("cmd.exe", "/C " + command)
                {
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    WorkingDirectory = "C:\\",
                };

                var process = Process.Start(processInfo);
                process.WaitForExit(timeout);
                exitCode = process.ExitCode;
                process.Close();

            }
            catch (Exception ex)
            {
                
            }
            return exitCode;
        }
    }
}