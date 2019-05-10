using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Octacom.Odiss.Library;
using Octacom.Odiss.Library.Auth;

namespace Octacom.Odiss.Odiss5Adapters
{
    public interface IDocumentsAdapter
    {
        dynamic GetResults(Settings.Application app, FormCollection form, AuthPrincipal user, int page = 0, string sort = "");
        dynamic SearchAutoComplete(string query, App app, string mapTo);
        void SetMappings(IDictionary<Guid, Type> mappings);
    }

    public interface IDocumentsAdapter<TDocument>
    {
        dynamic GetResults(Settings.Application app, FormCollection form, AuthPrincipal user, int page = 0, string sort = "");
        dynamic SearchAutoComplete(string query, App app, string mapTo);
    }
}
