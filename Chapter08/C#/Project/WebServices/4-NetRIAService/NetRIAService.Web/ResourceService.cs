
namespace NetRIAService.Web
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Ria;
    using System.Web.Ria.Data;
    using System.Web.DomainServices;
    using System.Data;
    using System.Web.DomainServices.LinqToEntities;


    // Implements application logic using the SilverlightResourceEntities context.
    // TODO: Add your application logic to these methods or in additional methods.
    [EnableClientAccess()]
    public class ResourceService : LinqToEntitiesDomainService<SilverlightResourceEntities>
    {

        // TODO: Consider
        // 1. Adding parameters to this method and constraining returned results, and/or
        // 2. Adding query methods taking different parameters.
        public IQueryable<Resource> GetResource()
        {
            return this.Context.Resource;
        }
    }
}


