using AutoMapper;


using FrameworkDev.Web.Areas.Document.Models;
using FrameworkDev.Web.Models;
using FrameworkDev.Web.Helpers;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace FrameworkDev.Web.Areas.Document.Repos
{
    public class TableNameRepository : CustomRepository<VM_TableName, int>
    {
        public override IQueryable<VM_TableName> GetList()
        {
            List<VM_TableName> vmList = new List<VM_TableName>();
            DbSet<TableName> entitys = context.TableNames;
            foreach (TableName item in entitys)
            {
                VM_TableName vm = Mapper.Map<TableName, VM_TableName>(item);
                vmList.Add(vm);
            }
            return vmList.AsQueryable();
        }

        public override VM_TableName GetByID(int id)
        {
            TableName entity = context.TableNames.Find(id);
            VM_TableName vm = Mapper.Map<TableName, VM_TableName>(entity);
            return vm;
        }

        public VM_TableName GetByTableName(string TableName)
        {
            return context.TableNames
                        .Where(t => t.TBNName == TableName)
                        .Select(TBL_TablesName => new VM_TableName
                        {
                            TablesNameID = TBL_TablesName.TablesNameID,
                            TBNParentID_fk = TBL_TablesName.TBNParentID_fk,
                            TBSubSystemID_fK = TBL_TablesName.TBSubSystemID_fK,
                            TBNName = TBL_TablesName.TBNName,
                            TBNPrimaryKey = TBL_TablesName.TBNPrimaryKey,
                            TBNNote = TBL_TablesName.TBNNote,
                            TBNType = TBL_TablesName.TBNType,
                            TBNActive = TBL_TablesName.TBNActive,
                            TBNStatus = TBL_TablesName.TBNStatus,
                            TBNCreDate = TBL_TablesName.TBNCreDate
                        }).First();
        }

        public override VM_TableName Insert(VM_TableName Tbl)
        {
            TableName entity = Mapper.Map<VM_TableName, TableName>(Tbl);

            context.TableNames.Add(entity);
            context.SaveChanges();

            VM_TableName resultVM = Mapper.Map<TableName, VM_TableName>(entity);

            return resultVM;
        }

        public override VM_TableName Update(VM_TableName vm)
        {
            TableName entity = Mapper.Map<VM_TableName, TableName>(vm);
            context.TableNames.Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
            Save();
            return Mapper.Map<TableName, VM_TableName>(entity);
        }

        public override VM_TableName Delete(int id)
        {
            TableName entity = context.TableNames.FirstOrDefault(p => p.TablesNameID == id);
            context.TableNames.Remove(entity);
            Save();
            return Mapper.Map<TableName, VM_TableName>(entity);
        }
    }
}
