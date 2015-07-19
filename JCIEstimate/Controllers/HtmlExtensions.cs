﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Web.Mvc;
using JCIEstimate.Models;
using System.Data.Entity;
using System.Net.Mail;
using System.IO;


namespace JCIExtensions
{
    public static class MCVExtensions
    {
        public static Guid pseudoNull = new Guid("00000000-0000-0000-0000-000000000000");

        public static void InsertOrUpdate(DbContext context, EquipmentToDo entity)
        {
            if (entity.equipmentToDoUid == Guid.Empty)
            {
                context.Entry(entity).State = EntityState.Added;
                entity.equipmentToDoUid = Guid.NewGuid();
            }
            else
            {
                context.Entry(entity).State = EntityState.Modified;
            }
            context.SaveChanges();
        }

        public static void InsertOrUpdate(DbContext context, EquipmentAttributeValue entity)
        {            
            if (entity.equipmentAttributeValueUid == Guid.Empty)
            {
                context.Entry(entity).State = EntityState.Added;
                entity.equipmentAttributeValueUid = Guid.NewGuid();
            }
            else
            {
                context.Entry(entity).State = EntityState.Modified;
            }
            context.SaveChanges();
        }

        public static void InsertOrUpdate(DbContext context, ProjectMilestone entity)
        {
            if (entity.projectMilestoneUid == Guid.Empty)
            {
                context.Entry(entity).State = EntityState.Added;
                entity.projectMilestoneUid = Guid.NewGuid();
            }
            else
            {
                context.Entry(entity).State = EntityState.Modified;
            }
            context.SaveChanges();
        }

        public static void InsertOrUpdate(DbContext context, ProjectMilestoneAction entity)
        {
            if (entity.projectMilestoneActionUid == Guid.Empty)
            {
                context.Entry(entity).State = EntityState.Added;
                entity.projectMilestoneActionUid = Guid.NewGuid();
            }
            else
            {
                context.Entry(entity).State = EntityState.Modified;
            }
            context.SaveChanges();
        }
        public static void sendMail(string message, string subject, string fromAddress, string toAddress)
        {
            SmtpClient sClient = new SmtpClient("localhost");
            sClient.Credentials = CredentialCache.DefaultNetworkCredentials;
            MailMessage m = new MailMessage(fromAddress, toAddress, subject, message);
            if (!HttpContext.Current.Request.IsLocal)
            {
                sClient.Send(m);
            }
        }
        

        public static void InsertOrUpdate(DbContext context, ExpenseMiscellaneousProject entity)
        {
            if (entity.expenseMiscellaneousProjectUid == Guid.Empty)
            {
                context.Entry(entity).State = EntityState.Added;
                entity.expenseMiscellaneousProjectUid = Guid.NewGuid();
            }
            else
            {
                context.Entry(entity).State = EntityState.Modified;
            }
            context.SaveChanges();
        }


        public static List<SelectListItem> ToSelectList<T>(
            this IEnumerable<T> enumerable,
            Func<T, string> text,
            Func<T, string> value,
            string defaultOption)
        {
            var items = enumerable.Select(f => new SelectListItem()
            {
                Text = text(f),
                Value = value(f),
                Selected = value(f).Equals(defaultOption)
            }).ToList();
            items.Insert(0, new SelectListItem()
            {
                Text = "-- Choose --",
                Value = "00000000-0000-0000-0000-000000000000"
            });
            return items;
        }

        public static List<SelectListItem> ToSelectList<T>(
                    this IEnumerable<T> enumerable,
                    Func<T, string> text,
                    Func<T, string> value,
                    string defaultOption,
                    bool includeChoose)
        {
            var items = enumerable.Select(f => new SelectListItem()
            {
                Text = text(f),
                Value = value(f),
                Selected = value(f).Equals(defaultOption)
            }).ToList();
            if (includeChoose)
            {
                items.Insert(0, new SelectListItem()
                {
                    Text = "-- Choose --",
                    Value = "00000000-0000-0000-0000-000000000000"
                });
            }
            
            return items;
        }

        public static Guid getSessionProject()
        {
            Guid sessionProject;

            sessionProject = Guid.Empty;

            if (HttpContext.Current.Session["projectUid"] != null)
            {
                sessionProject = new System.Guid(HttpContext.Current.Session["projectUid"].ToString());
            }
            else
            {
                HttpContext.Current.Response.Redirect("~/Home/Index");
            }

            return sessionProject;
        }

        public static void SendEmail(string emailAddress, string subject, string body, bool isHtml)
        {
            
            SmtpClient smtpClient = new SmtpClient("localhost", 25);
            
            smtpClient.UseDefaultCredentials = true;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.EnableSsl = false;
            MailMessage mail = new MailMessage();

            //Setting From , To and CC
            mail.From = new MailAddress("info@bernservices.com", "Info");
            mail.To.Add(new MailAddress(emailAddress));
            mail.Subject = subject;
            if (isHtml)
            {
                body += "<br/><br/><i><b>";
            }
            else
            {
                body += "\n\n";
            }
            body += "This email is not monitored for incoming messages.";
            if (isHtml)
            {
                body += "</b></i>";
            }

            mail.Body = body;
            mail.IsBodyHtml = isHtml;

            try
            {
                smtpClient.Send(mail);

            }
            catch (Exception ex)
            {                
                StreamWriter f = new StreamWriter("mailErrors.log", true);                
                f.WriteLine("--------------" + DateTime.Now.ToString() + ": " + ex.Message);
                f.Close();
             //silent error for dev failures   
            }

        }

        public static void sendEmailToProjectUsers(JCIEstimateEntities db, Guid projectUid, string subject, string body, bool isHtml)
        {

            var users = from cc in db.ProjectUsers
                        join au in db.AspNetUsers on cc.aspNetUserUid equals au.Id
                        where cc.projectUid == projectUid
                        select au;
            foreach (var item in users)
            {
                SendEmail(item.Email, subject, body, isHtml);
            }
        }

        public static string MakeValidFileName(string name)
        {
            string invalidChars = System.Text.RegularExpressions.Regex.Escape(new string(System.IO.Path.GetInvalidFileNameChars()));
            string invalidRegStr = string.Format(@"([{0}]*\.+$)|([{0}]+)", invalidChars);

            return System.Text.RegularExpressions.Regex.Replace(name, invalidRegStr, "_");
        }
    }
}