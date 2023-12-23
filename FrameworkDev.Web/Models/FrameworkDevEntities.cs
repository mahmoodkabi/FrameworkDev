using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace FrameworkDev.Web.Models
{
    public partial class FrameworkDevEntities
    {
        public string Username { get; set; }
        public string IP { get; set; }

        public override int SaveChanges()
        {
            var tempChangeTracker = ChangeTracker;
            var res = base.SaveChanges();
            DisplayTrackedEntities(tempChangeTracker);

            return res;
        }

        private void DisplayTrackedEntities(DbChangeTracker changeTracker)
        {
            //try
            //{
            //    IEnumerable<DbEntityEntry> entries = changeTracker.Entries();
            //    foreach (DbEntityEntry item in entries)
            //    {
            //        System.Data.Entity.Core.Objects.ObjectParameter outID = new System.Data.Entity.Core.Objects.ObjectParameter("outID", typeof(int));

            //        if (item.State == EntityState.Added)
            //        {
            //            string table = item.Entity.GetType().Name;

            //            { USP_ChangeTracking(table, "ایجاد", Username, IP, outID); }

            //        }

            //        if (item.State == EntityState.Modified)
            //        {
            //            string table = item.Entity.GetType().Name;
            //            USP_ChangeTracking(table, "ویرایش", Username, IP, outID);
            //        }

            //        if (item.State == EntityState.Deleted)
            //        {
            //            string table = item.Entity.GetType().Name;
            //            USP_ChangeTracking(table, "حذف", Username, IP, outID);
            //        }

            //        foreach (PropertyInfo p in item.Entity.GetType().GetProperties())
            //        {
            //            try
            //            {
            //                string oldValue = "";
            //                string newValue = p.GetValue(item.Entity)?.ToString();
            //                if (item.State != EntityState.Added)
            //                {
            //                    oldValue = item.OriginalValues[p.Name]?.ToString();
            //                }
            //                if (item.State != EntityState.Unchanged)
            //                {
            //                    USP_ChangeTrackingDetails(Convert.ToInt32(outID.Value), p.Name, newValue, oldValue);
            //                }
            //            }
            //            catch (Exception ex)
            //            {
            //                Exception ex1 = ex;
            //            }
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Exception ex1 = ex;
            //}


        }

    }
}