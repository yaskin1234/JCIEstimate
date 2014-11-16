using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Web.Mvc;
using JCIEstimate.Models;
using System.Data.Entity;
using System.Net.Mail;


namespace JCIExtensions
{
    public static class MCVExtensions
    {

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
            sClient.Send(m);
            
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
                Value = value(f)
            }).ToList();
            items.Insert(0, new SelectListItem()
            {
                Text = "-- Choose --",
                Value = "-1"
            });
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
    }
}