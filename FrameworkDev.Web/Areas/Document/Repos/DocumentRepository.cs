using AutoMapper;


using FrameworkDev.Web.Areas.Document.Models;
using FrameworkDev.Web.Models;
using FrameworkDev.Web.Helpers;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;

namespace FrameworkDev.Web.Areas.Document.Repos
{
    public class DocumentRepository : CustomRepository<VM_Document, int>
    {
        public override IQueryable<VM_Document> GetList()
        {
            List<VM_Document> vmList = new List<VM_Document>();
            DbSet<FrameworkDev.Web.Models.Document> entitys = context.Documents;
            foreach (FrameworkDev.Web.Models.Document item in entitys)
            {
                VM_Document vm = Mapper.Map<FrameworkDev.Web.Models.Document, VM_Document>(item);
                vmList.Add(vm);
            }
            return vmList.AsQueryable();
        }

        public override VM_Document GetByID(int id)
        {
            FrameworkDev.Web.Models.Document entity = context.Documents.Find(id);
            VM_Document vm = Mapper.Map<FrameworkDev.Web.Models.Document, VM_Document>(entity);
            return vm;
        }
        
        public IQueryable<VM_Document> GetByTableRow(VM_Document TblRow)
        {
            IQueryable<VM_Document> DocList = context.Documents
                        .Where(t => t.TableNameId == TblRow.TableNameId
                            && t.TableRowId == TblRow.TableRowId)
                        .Select(p => new VM_Document
                        {
                            DocId = p.DocId,
                            TableNameId = p.TableNameId,
                            TableRowId = p.TableRowId,
                            DocHRDocId_fk = p.DocHRDocId_fk,
                            DocDesc = p.DocDesc,
                            DocType = p.DocType,
                            DocActive = p.DocActive,
                            DocStatus = p.DocStatus,
                            DocCreDate = p.DocCreDate
                        });

            List<VM_Document> ResultDocList = new List<VM_Document>();
            foreach (VM_Document item in DocList)
            {
                //get persian date---------------
                PersianCalendar pc = new PersianCalendar();
                item.DocCrePersianDate = string.Format("{0}/{1}/{2} - {3}:{4}:{5}",
                                            pc.GetYear(item.DocCreDate),
                                            pc.GetMonth(item.DocCreDate),
                                            pc.GetDayOfMonth(item.DocCreDate),
                                            pc.GetHour(item.DocCreDate),
                                            pc.GetMinute(item.DocCreDate),
                                            pc.GetSecond(item.DocCreDate)
                                         ).ToString();

                //get file name------------------
                item.DocName = new FrameworkDevDocsRepository().SelectById(item.DocHRDocId_fk).name;

                ResultDocList.Add(item);
            }
            return ResultDocList.AsQueryable();
        }

        public override VM_Document Insert(VM_Document vM_Document)
        {
            FrameworkDev.Web.Models.Document entity = Mapper.Map<VM_Document, FrameworkDev.Web.Models.Document>(vM_Document);

            context.Documents.Add(entity);
            context.SaveChanges();

            VM_Document resultVM = Mapper.Map<FrameworkDev.Web.Models.Document, VM_Document>(entity);

            return resultVM;
        }

        public override VM_Document Update(VM_Document vm)
        {
            FrameworkDev.Web.Models.Document entity = Mapper.Map<VM_Document, FrameworkDev.Web.Models.Document>(vm);
            context.Documents.Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
            Save();
            return Mapper.Map<FrameworkDev.Web.Models.Document, VM_Document>(entity);
        }

        public override VM_Document Delete(int id)
        {
            FrameworkDev.Web.Models.Document entity = context.Documents.FirstOrDefault(p => p.DocId == id);
            context.Documents.Remove(entity);
            Save();
            return Mapper.Map<FrameworkDev.Web.Models.Document, VM_Document>(entity);
        }
    }
}
