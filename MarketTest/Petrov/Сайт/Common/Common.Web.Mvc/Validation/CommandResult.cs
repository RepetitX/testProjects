using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Common.Web.Mvc
{
    public class CommandResult
    {
        public bool Successful { get; set; }
        public object Data { get; set; }
        public Dictionary<string, string> Errors { get; set; }

        public CommandResult()
        {
            Successful = true;
            Errors = new Dictionary<string, string>();
        }

        public CommandResult(ModelStateDictionary modelState) : this(modelState,null)
        {
        }

        public CommandResult(ModelStateDictionary modelState, object data)
        {
            Errors = new Dictionary<string, string>();
            Data = data;

            if (modelState.IsValid)
                Successful = true;
            else
            {
                Successful = false;

                foreach(var state in modelState.Where(s => s.Value.Errors.Any()))
                {
                    Errors.Add(state.Key, string.Join("; ", state.Value.Errors.Select(e => !string.IsNullOrEmpty(e.ErrorMessage) ? e.ErrorMessage : e.Exception.Message).ToArray()));
                }
            }
        }
    }
}
