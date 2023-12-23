using AutoMapper;


using FrameworkDev.Web.Areas.Workflow.Models;
using FrameworkDev.Web.Models; using FrameworkDev.Web.Helpers;
using System.Data.Entity;
using System.Linq;

namespace FrameworkDev.Web.Areas.Workflow.Repos
{
    public class WorkFlowTypeRepository : CustomRepository<VM_WorkFlowType, int>
    {
        //public override IQueryable<VM_WorkFlowType> GetList()
        //{
        //    IQueryable<VM_WorkFlowType> dataset = context.WorkFlowTypes.Select(WorkFlowType => new VM_WorkFlowType
        //    {
        //        WorkFlowTypeID = WorkFlowType.WorkFlowTypeID,
        //        Name = WorkFlowType.Name,
        //        FaName = WorkFlowType.FaName
        //    });

        //    return dataset;
        //}

        //public override VM_WorkFlowType GetByID(int id)
        //{
        //    WorkFlowType entity = context.WorkFlowTypes.Find(id);
        //    VM_WorkFlowType vm = Mapper.Map<WorkFlowType, VM_WorkFlowType>(entity);
        //    return vm;
        //}

        //public override VM_WorkFlowType Insert(VM_WorkFlowType vm)
        //{
        //    WorkFlowType entity = Mapper.Map<VM_WorkFlowType, WorkFlowType>(vm);

        //    context.WorkFlowTypes.Add(entity);
        //    context.SaveChanges();

        //    VM_WorkFlowType resultVM = Mapper.Map<WorkFlowType, VM_WorkFlowType>(entity);

        //    return resultVM;
        //}

        //public override VM_WorkFlowType Update(VM_WorkFlowType vm)
        //{
        //    WorkFlowType entity = Mapper.Map<VM_WorkFlowType, WorkFlowType>(vm);
        //    context.WorkFlowTypes.Attach(entity);
        //    context.Entry(entity).State = EntityState.Modified;
        //    Save();
        //    return Mapper.Map<WorkFlowType, VM_WorkFlowType>(entity);
        //}

        //public override VM_WorkFlowType Delete(int id)
        //{
        //    WorkFlowType entity = context.WorkFlowTypes.FirstOrDefault(p => p.WorkFlowTypeID == id);
        //    context.WorkFlowTypes.Remove(entity);
        //    Save();
        //    return Mapper.Map<WorkFlowType, VM_WorkFlowType>(entity);
        //}
    }
}
