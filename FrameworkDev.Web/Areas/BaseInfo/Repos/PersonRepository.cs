using AutoMapper;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using FrameworkDev.Web.Areas.BaseInfo.Models;
using FrameworkDev.Web.Helpers;
using FrameworkDev.Web.Models;

namespace FrameworkDev.Web.Areas.BaseInfo.Repos
{
    public class PersonRepository : CustomRepository<VM_Person, int>
    {


        public override IQueryable<VM_Person> GetList()
        {
            IQueryable<VM_Person> dataset = context.Persons.Select(Person => new VM_Person
            {
                PersonId = Person.PersonId,
                AccountOwnerId = Person.PersonId
                  ,
                PersonMelliCode = Person.PersonMelliCode
                  ,
                Name = Person.Name
                  ,
                Family = Person.Family
                  ,
                FatherName = Person.FatherName
                  ,
                Sex = Person.Sex
                  ,
                IDNo = Person.IDNo
                  ,
                BirthDate = Person.BirthDate
                 ,
                BirthPlcId = Person.BirthPlcId
                ,
                ExportPlcId = Person.ExportPlcId
                 ,
                TelNo = Person.TelNo
                ,
                MobileNo = Person.MobileNo
                ,
                Address = Person.Address
                ,
                PostCode = Person.PostCode
              ,
                IsAccountOwner = Person.IsAccountOwner
              ,
                CompanyName = Person.CompanyName
              ,
                CompanyMelliCode = Person.CompanyMelliCode
              ,
                NameManager = Person.Name
              ,
                FamilyManager = Person.Family
              ,
                IDNoCompany = Person.IDNo
              ,
                BirthDateCompany = Person.BirthDate
              ,
                ExportPlcIdCompany = Person.ExportPlcId
              ,
                TelNoCompany = Person.TelNo
              ,
                MobileNoCompany = Person.MobileNo
              ,
                AddressCompany = Person.Address
              ,
                PostCodeCompany = Person.PostCode
              ,
                IsAccountOwnerCompany = Person.IsAccountOwner
              ,
                CreDate = Person.CreDate
              ,
                UserId = Person.UserId,
                PersonType = Person.PersonType,
                FullPersonName = Person.PersonType == false ? (Person.Name + " " + Person.Family + " " + Person.PersonMelliCode) : Person.CompanyName + " " + Person.CompanyMelliCode,

            });
            
            return dataset;
        }

        public VM_Person GetPersonAllInfo(int PersonId)
        {
            PersonRepository rep = new PersonRepository();
            VM_Person res = rep.GetByID(PersonId);
            return res;
        }


        public override VM_Person GetByID(int id)
        {
            var dataset = context.Persons.Where(p => p.PersonId == id).Select(Person => new VM_Person
            {
                PersonId = Person.PersonId
                      ,
                PersonMelliCode = Person.PersonMelliCode
                      ,
                TempMelliCode = Person.PersonMelliCode
                      ,
                Name = Person.Name
                      ,
                Family = Person.Family
                      ,
                FatherName = Person.FatherName
                      ,
                Sex = Person.Sex
                      ,
                IDNo = Person.IDNo
                      ,
                BirthDate = Person.BirthDate
                     ,
                BirthPlcId = Person.BirthPlcId
                    ,
                ExportPlcId = Person.ExportPlcId
                     ,
                TelNo = Person.TelNo
                    ,
                MobileNo = Person.MobileNo
                    ,
                Address = Person.Address
                    ,
                PostCode = Person.PostCode
                  ,
                IsAccountOwner = Person.IsAccountOwner
                  ,
                CompanyName = Person.CompanyName
                  ,
                CompanyMelliCode = Person.CompanyMelliCode
                  ,
                NameManager = Person.Name
                  ,
                FamilyManager = Person.Family
                  ,
                IDNoCompany = Person.IDNo
                  ,
                BirthDateCompany = Person.BirthDate
                  ,
                ExportPlcIdCompany = Person.ExportPlcId
                  ,
                TelNoCompany = Person.TelNo
                  ,
                MobileNoCompany = Person.MobileNo
                  ,
                AddressCompany = Person.Address
                  ,
                PostCodeCompany = Person.PostCode
                  ,
                IsAccountOwnerCompany = Person.IsAccountOwner
                  ,
                CreDate = Person.CreDate
                  ,
                UserId = Person.UserId,
                PersonType = Person.PersonType

            }).First();

            return dataset;
        }



        public override VM_Person Insert(VM_Person vm)
        {
            //if (context.Persons.Any(x => x.BirthDateCompany == BirthDateCompany && x.IDNoCompany == IDNoCompany  && x.ExportPlcIdCompany== ExportPlcIdCompany))
            //{
            //    vm.Message.Result = "Bad";
            //    vm.Message.Message = Resources.Messages.InsertError;
            //}
            Person entity = null;
            if (vm.PersonType == false)
            {
                entity = new Person()
                {
                    PersonId = vm.PersonId
                   ,
                    PersonMelliCode = vm.PersonMelliCode
                   ,
                    Name = vm.Name
                   ,
                    Family = vm.Family
                   ,
                    FatherName = vm.FatherName
                   ,
                    Sex = vm.Sex
                   ,
                    IDNo = vm.IDNo
                   ,
                    BirthDate = vm.BirthDate
                  ,
                    BirthPlcId = vm.BirthPlcId
                 ,
                    ExportPlcId = vm.ExportPlcId
                  ,
                    TelNo = vm.TelNo
                 ,
                    MobileNo = vm.MobileNo
                 ,
                    Address = vm.Address
                 ,
                    PostCode = vm.PostCode
               ,
                    IsAccountOwner = vm.IsAccountOwner
               ,
                    CreDate = vm.CreDate
               ,
                    UserId = vm.UserId,
                    PersonType = vm.PersonType,
                };
            }
            else
            {
                entity = new Person()
                {
                    PersonId = vm.PersonId
                   ,
                    Name = vm.NameManager
                   ,
                    Family = vm.FamilyManager
                   ,
                    IDNo = vm.IDNoCompany
                   ,
                    BirthDate = vm.BirthDateCompany
                 ,
                    ExportPlcId = vm.ExportPlcIdCompany
                  ,
                    TelNo = vm.TelNoCompany
                 ,
                    MobileNo = vm.MobileNoCompany
                 ,
                    Address = vm.AddressCompany
                 ,
                    PostCode = vm.PostCodeCompany
               ,
                    IsAccountOwner = vm.IsAccountOwnerCompany
               ,
                    CompanyName = vm.CompanyName
               ,
                    CompanyMelliCode = vm.CompanyMelliCode
               ,
                    CreDate = vm.CreDate
               ,
                    UserId = vm.UserId,
                    PersonType = vm.PersonType,
                };
            }


            context.Persons.Add(entity);
            context.SaveChanges();

            vm = Mapper.Map<VM_Person>(entity);
            vm.Message.Result = "Ok";
            vm.Message.Message = Resources.Messages.InsertSuccessful;

            return vm;
        }


        public override VM_Person Update(VM_Person vm)
        {
            var entity = context.Persons.First(p => p.PersonId == vm.PersonId);
            entity.PersonId = vm.PersonId;

            if (vm.PersonType) // حقوقی 
            {
                entity.Name = vm.NameManager;
                entity.Family = vm.FamilyManager;
                entity.CompanyName = vm.CompanyName;
                entity.CompanyMelliCode = vm.CompanyMelliCode;
                entity.IDNo = vm.IDNoCompany;
                entity.BirthDate = vm.BirthDateCompany;
                entity.ExportPlcId = vm.ExportPlcIdCompany;
                entity.TelNo = vm.TelNoCompany;
                entity.MobileNo = vm.MobileNoCompany;
                entity.Address = vm.AddressCompany;
                entity.PostCode = vm.PostCodeCompany;
                //entity.IsAccountOwner = vm.IsAccountOwnerCompany;
                entity.CreDate = vm.CreDate;
                entity.UserId = vm.UserId;
                entity.PersonType = vm.PersonType;
            }
            else //حقیقی
            {

                entity.PersonMelliCode = vm.PersonMelliCode;
                entity.Name = vm.Name;
                entity.Family = vm.Family;
                entity.FatherName = vm.FatherName;
                entity.Sex = vm.Sex;
                entity.IDNo = vm.IDNo;
                entity.BirthDate = vm.BirthDate;
                entity.BirthPlcId = vm.BirthPlcId;
                entity.ExportPlcId = vm.ExportPlcId;
                entity.TelNo = vm.TelNo;
                entity.MobileNo = vm.MobileNo;
                entity.Address = vm.Address;
                entity.PostCode = vm.PostCode;
                //entity.IsAccountOwner = vm.IsAccountOwner;
            }



            Save();

            vm = Mapper.Map<VM_Person>(entity);
            vm.Message.Result = "Ok";
            vm.Message.Message = Resources.Messages.UpdateSuccessful;
            return vm;
        }

        public override VM_Person Delete(int id)
        {
            VM_Person vm = new VM_Person();
            Person entity = context.Persons.FirstOrDefault(p => p.PersonId == id);

            try
            {
                context.Persons.Remove(entity);
                Save();
                vm.Message.Result = "Ok";
                vm.Message.Message = Resources.Messages.DeleteSuccessful;
                return vm;
            }
            catch (System.Exception)
            {
                vm.Message.Result = "Error";
                vm.Message.Message = "به دلیل در گردش بودن اطلاعات این ركورد قابل حذف نمی باشد";
                return vm;
            }

            //   return Mapper.Map<Person, VM_Person>(entity);
        }
        //public override VM_Person Update(int id)
        //{
        //    Person entity = context.Persons.FirstOrDefault(p => p.PersonId == id);
        //    context.Persons.Remove(entity);
        //    Save();
        //    return Mapper.Map<Person, VM_Person>(entity);
        //}
        public VM_Person Select(string MelliCode)
        {
            var dataset = context.Persons.Where(p => p.PersonMelliCode == MelliCode || p.CompanyMelliCode == MelliCode).Select(Person => new VM_Person
            {
                PersonId = Person.PersonId
                      ,
                PersonMelliCode = Person.PersonMelliCode
                      ,
                Name = Person.Name
                      ,
                Family = Person.Family

                   ,
                CompanyMelliCode = Person.CompanyMelliCode
                  ,
                NameManager = Person.Name
                  ,
                FamilyManager = Person.Family

            }).First();

            return dataset;
        }
        public Person GetPersonByMeliCode(string MelliCode)
        {
            var dataset = context.Persons.FirstOrDefault(x => x.PersonMelliCode == MelliCode || x.CompanyMelliCode == MelliCode);

            return dataset;
        }
    }
}


