using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FrameworkDev.Web.Helpers.Utilites
{
    public static class ModelStateHelper
    {
        public static string GetErrorFromModelState
                                              (ModelStateDictionary modelState)
        {
            var query = from state in modelState.Values
                        from error in state.Errors
                        select error.ErrorMessage;

            string strError = "";
            var errorList = query.ToList();
            foreach (var item in errorList)
            {
                strError += item + " " + System.Environment.NewLine;
            }
            return strError;
        }

        public static List<string> GetErrorListFromModelState
                                              (ModelStateDictionary modelState)
        {
            var query = from state in modelState.Values
                        from error in state.Errors
                        select error.ErrorMessage;

            var errorList = query.ToList();
            return errorList;
        }
    }
}